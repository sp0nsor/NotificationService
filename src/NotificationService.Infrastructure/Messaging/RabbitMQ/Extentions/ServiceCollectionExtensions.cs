using JasperFx.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Exceptions;
using NotificationService.Application.Notifications.Commands;
using NotificationService.Application.Notifications.Handlers;
using NotificationService.Core.Exceptions;
using NotificationService.Infrastructure.Messaging.RabbitMQ.Options;
using NotificationService.Infrastructure.Messaging.RabbitMQ.Services;
using Wolverine;
using Wolverine.ErrorHandling;
using Wolverine.RabbitMQ;

namespace NotificationService.Infrastructure.Messaging.RabbitMQ.Extentions
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddRabbitMqMessaging(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = configuration.GetRequiredSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>()!;

            services.AddWolverine(opts =>
            {
                opts.UseRabbitMq(options.ConnectionString)
                    .AutoProvision()
                    .ConfigureChannelCreation(o =>
                    {
                        o.PublisherConfirmationsEnabled = true;
                        o.PublisherConfirmationTrackingEnabled = true;
                    })
                    .DeclareExchange(options.ExchangeName, exchange =>
                    {
                        exchange.ExchangeType = ExchangeType.Direct;

                        exchange.BindQueue(
                            options.QueueName,
                            options.RoutingKey);
                    });

                opts.PublishMessage<SendNotificationMessage>()
                    .ToRabbitRoutingKey(options.ExchangeName, options.RoutingKey);

                opts.ListenToRabbitQueue(options.QueueName);

                opts.UseSystemTextJsonForSerialization();

                opts.Policies.UseDurableOutboxOnAllSendingEndpoints();
                opts.Policies.UseDurableLocalQueues();

                opts.Policies
                    .OnException<NotificationTemporaryException>()
                    .RetryWithCooldown(1.Seconds(), 5.Seconds(), 15.Seconds())
                    .And(async (_, context, exception) =>
                    {
                        if (context.Envelope?.Message is SendNotificationMessage message)
                        {
                            await context.SendAsync(
                                new NotificationFailedMessage(
                                        message.NotificationId,
                                        exception.Message));
                        }
                    });

                opts.Policies
                    .OnException<BadRequestException>()
                    .Discard()
                    .And(async (_, context, exception) =>
                    {
                        if (context.Envelope?.Message is SendNotificationMessage message)
                        {
                            await context.SendAsync(
                                new NotificationFailedMessage(
                                        message.NotificationId,
                                        exception.Message));
                        }
                    });

                opts.Policies
                    .OnException<Exception>()
                    .Discard();

                opts.DefaultExecutionTimeout = TimeSpan.FromMinutes(1);
                opts.DefaultRemoteInvocationTimeout = TimeSpan.FromMinutes(1);

                opts.Discovery.IncludeAssembly(typeof(SendNotificationHandler).Assembly);
            });

            services.AddScoped<Application.Abstractions.Messaging.IMessageBus, RabbitMqMessageBus>();
        }
    }
}
