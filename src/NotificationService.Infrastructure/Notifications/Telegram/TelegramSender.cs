using NotificationService.Application.Abstractions.Notifications;
using NotificationService.Core.Models;
using NotificationService.Core.Primitives.Enums;

namespace NotificationService.Infrastructure.Notifications.Telegram
{
    public sealed class TelegramSender
        : INotificationSender
    {
        public Provider Provider => Provider.Telegram;

        public async Task SendAsync(Notification notification, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1000, cancellationToken);
        }
    }
}
