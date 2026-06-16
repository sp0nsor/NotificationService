using NotificationService.Core.Primitives.Enums;

namespace NotificationService.Application.Abstractions.Notifications
{
    public interface INotificationSenderResolver
    {
        INotificationSender Resolve(Provider provider);
    }
}
