using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Integrations.Extensions;
using NotificationService.Infrastructure.Messaging.RabbitMQ.Extentions;
using NotificationService.Infrastructure.Notifications.Extensions;

namespace NotificationService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddIntegrations();
            services.AddNotificationSenders(configuration);
            services.AddRabbitMqMessaging(configuration);
        }
    }
}
