using JasperFx.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Exceptions;
using NotificationService.Application.Notifications.Commands;
using NotificationService.Application.Notifications.Handlers;
using NotificationService.Application.Notifications.Middlerware;
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
                    .RetryWithCooldown(
                        1.Seconds(),
                        5.Seconds(),
                        15.Seconds());

                opts.Policies
                    .AddMiddleware<NotificationStatusMeddleware>();

                opts.DefaultExecutionTimeout = TimeSpan.FromMinutes(1);
                opts.DefaultRemoteInvocationTimeout = TimeSpan.FromMinutes(1);

                opts.Discovery.IncludeAssembly(typeof(SendNotificationHandler).Assembly);
            });

            services.AddScoped<Application.Abstractions.Messaging.IMessageBus, RabbitMqMessageBus>();
        }
    }
}
