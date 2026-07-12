using NotificationService.Application.Abstractions.DataAccess;
using NotificationService.Application.Abstractions.Notifications;
using NotificationService.Application.Notifications.Commands;
using NotificationService.Core.Exceptions;
using NotificationService.Core.Models;
using NotificationService.Core.Primitives.Enums;

namespace NotificationService.Application.Notifications.Handlers
{
    public sealed class SendNotificationHandler
    {
        private readonly IBaseRepository<Notification> _notificationRepository;
        private readonly INotificationSenderResolver _senderResolver;

        public SendNotificationHandler(
            IBaseRepository<Notification> notificationRepository,
            INotificationSenderResolver senderResolver)
        {
            _notificationRepository = notificationRepository;
            _senderResolver = senderResolver;
        }

        public async Task Handle(
            SendNotificationMessage message,
            CancellationToken cancellationToken = default)
        {
            var notification = await _notificationRepository.GetSingleAsync(
                n => n.Id == message.NotificationId,
                cancellationToken);

            if (notification is null)
            {
                throw new NotFoundException($"Notification with Id: {message.NotificationId} not found.");
            }

            notification.ChangeStatus(Status.Processing);

            await _notificationRepository.UpdateAsync(
                notification,
                cancellationToken);

            var sender = _senderResolver.Resolve(
                notification.Recipient.Provider);

            await sender.SendAsync(
                notification,
                cancellationToken);

            notification.ChangeStatus(Status.Sent);

            await _notificationRepository.UpdateAsync(
                notification,
                cancellationToken);
        }
    }
}
