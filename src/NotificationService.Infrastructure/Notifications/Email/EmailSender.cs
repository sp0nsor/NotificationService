using NotificationService.Application.Abstractions.Notifications;
using NotificationService.Core.Models;
using NotificationService.Core.Primitives.Enums;

namespace NotificationService.Infrastructure.Notifications.Email
{
    public sealed class EmailSender
        : INotificationSender
    {
        public Provider Provider => Provider.Email;

        public async Task SendAsync(Notification notification, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1000, cancellationToken);
        }
    }
}
