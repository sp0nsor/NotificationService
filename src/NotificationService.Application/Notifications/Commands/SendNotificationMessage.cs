using NotificationService.Core.Primitives.Enums;

namespace NotificationService.Application.Notifications.Commands
{
    public record SendNotificationMessage(
        Guid UserId,
        Provider Provider,
        string Content,
        ContentType ContentType);
}
