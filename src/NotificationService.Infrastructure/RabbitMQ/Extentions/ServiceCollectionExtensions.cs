using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Notifications.Commands;
using NotificationService.Infrastructure.RabbitMQ.Services;
using NotificationService.Infrastructure.RabbitMQ.Settings;
using Wolverine;
using Wolverine.RabbitMQ;

namespace NotificationService.Infrastructure.RabbitMQ.Extentions
{
    public static class ServiceCollectionExtensions
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

                opts.PublishMessage<SendNotification>()
                    .ToRabbitRoutingKey(options.ExchangeName, options.RoutingKey);

                opts.ListenToRabbitQueue(options.QueueName);

                opts.UseSystemTextJsonForSerialization();

                opts.Policies.UseDurableOutboxOnAllSendingEndpoints();
                opts.Policies.UseDurableLocalQueues();

                opts.DefaultExecutionTimeout = TimeSpan.FromMinutes(5);
                opts.DefaultRemoteInvocationTimeout = TimeSpan.FromMinutes(5);

            });

            services.AddScoped<Application.Abstractions.Messaging.IMessageBus, RabbitMqMessageBus>();
        }

    }
}
