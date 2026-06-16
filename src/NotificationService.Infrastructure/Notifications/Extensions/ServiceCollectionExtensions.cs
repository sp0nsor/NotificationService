using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Abstractions.Notifications;
using NotificationService.Infrastructure.Notifications.Email;
using NotificationService.Infrastructure.Notifications.Telegram;

namespace NotificationService.Infrastructure.Notifications.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddNotificationSenders(this IServiceCollection services)
        {
            services.AddScoped<INotificationSender, TelegramSender>();
            services.AddScoped<INotificationSender, EmailSender>();

            services.AddScoped<INotificationSenderResolver, NotificationSenderResolver>();
        }
    }
}
