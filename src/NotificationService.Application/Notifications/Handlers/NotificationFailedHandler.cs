using NotificationService.Application.Abstractions.DataAccess;
using NotificationService.Core.Models;
using NotificationService.Core.Primitives.Enums;

public sealed class NotificationFailedHandler
{
    private readonly IBaseRepository<Notification> _notificationRepository;

    public NotificationFailedHandler(IBaseRepository<Notification> notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task Handle(
        NotificationFailedMessage message,
        CancellationToken cancellationToken)
    {
        var notification =
            await _notificationRepository.GetSingleAsync(
                n => n.Id == message.NotificationId,
                cancellationToken);

        notification.ChangeStatus(Status.Failed);
        notification.SetLastError(message.Reason);

        await _notificationRepository
            .UpdateAsync(notification, cancellationToken);
    }
}