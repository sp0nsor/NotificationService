using CSharpFunctionalExtensions;
using NotificationService.Application.Abstractions.DataAccess;
using NotificationService.Application.Abstractions.Integrations;
using NotificationService.Application.Abstractions.Notifications;
using NotificationService.Application.Notifications.Commands;
using NotificationService.Application.Notifications.Context;
using NotificationService.Core.Exceptions;
using NotificationService.Core.Models;
using NotificationService.Core.Primitives.Enums;
using NotificationService.Core.ValueObjects;

namespace NotificationService.Application.Notifications.Handlers
{
    public sealed class SendNotificationHandler
    {
        private readonly IBaseRepository<Notification> _notificationRepository;
        private readonly IUserServiceClient _userServiceClient;
        private readonly INotificationSenderResolver _senderResolver;
        private readonly NotificationExecutionContext _context;

        public SendNotificationHandler(
            IBaseRepository<Notification> notificationRepository,
            IUserServiceClient userServiceClient,
            INotificationSenderResolver senderResolver,
            NotificationExecutionContext context)
        {
            _notificationRepository = notificationRepository;
            _userServiceClient = userServiceClient;
            _senderResolver = senderResolver;
            _context = context;
        }

        public async Task Handle(
            SendNotificationMessage notificationRequest,
            CancellationToken cancellationToken = default)
        {

            var contacts = await _userServiceClient
                .GetContacts(notificationRequest.UserId);

            var recipientValue = contacts?
                .FirstOrDefault(x => x.Provider == notificationRequest.Provider)?
                .Value;

            if (string.IsNullOrWhiteSpace(recipientValue))
            {
                throw new NotFoundException($"Recipient contact with User ID: {notificationRequest.UserId}" +
                    $" and provider: {notificationRequest.Provider} not found .");
            }

            var notificationResult =
                Recipient.Create(
                    notificationRequest.UserId,
                    notificationRequest.Provider,
                    recipientValue)
                .Bind(recipient =>
                    Content.Create(
                        notificationRequest.ContentType,
                        notificationRequest.Content)
                    .Bind(content =>
                        Notification.Create(
                            Status.Processing,
                            recipient,
                            content)));

            if (notificationResult.IsFailure)
            {
                throw new ValidationException(notificationResult.Error);
            }

            var notificationModel = notificationResult.Value;

            await _notificationRepository
                .AddAsync(notificationModel, cancellationToken);

            _context.Notification = notificationModel;

            var sender = _senderResolver
                .Resolve(notificationRequest.Provider);

            await sender
                .SendAsync(notificationModel, cancellationToken);

            notificationModel.ChangeStatus(Status.Sent);
        }
    }
}
