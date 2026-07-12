using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Abstractions.Notifications;
using NotificationService.Infrastructure.Notifications.Email.Clients;
using NotificationService.Infrastructure.Notifications.Email.Options;
using NotificationService.Infrastructure.Notifications.Email.Services;
using NotificationService.Infrastructure.Notifications.Telegram.Clients;
using NotificationService.Infrastructure.Notifications.Telegram.Options;
using NotificationService.Infrastructure.Notifications.Telegram.Services;

namespace NotificationService.Infrastructure.Notifications.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddNotificationSenders(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<TelegramOptions>(
                configuration.GetSection(nameof(TelegramOptions)));

            services.Configure<SmtpOptions>(
                configuration.GetSection(nameof(SmtpOptions)));

            services.AddHttpClient("telegram");

            services.AddScoped<ITelegramClient, TelegramClient>();

            services.AddScoped<IEmailClient, EmailClient>();

            services.AddScoped<INotificationSender, TelegramSender>();

            services.AddScoped<INotificationSender, EmailSender>();

            services.AddScoped<INotificationSenderResolver, NotificationSenderResolver>();
        }
    }
}
