using NotificationService.Core.Models;
using NotificationService.Core.Primitives.Enums;

namespace NotificationService.Application.Abstractions.Notifications
{
    public interface INotificationSender
    {
        Provider Provider { get; }

        Task SendAsync(Notification notification, CancellationToken cancellationToken = default);
    }
}
