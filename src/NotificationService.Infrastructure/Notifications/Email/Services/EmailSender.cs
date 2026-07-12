using NotificationService.Application.Abstractions.Notifications;
using NotificationService.Core.Models;
using NotificationService.Core.Primitives.Enums;
using NotificationService.Infrastructure.Notifications.Email.Contracts;
using NotificationService.Infrastructure.Notifications.Email.Exceptions;

namespace NotificationService.Infrastructure.Notifications.Email.Services
{
    public sealed class EmailSender
        : INotificationSender
    {
        private readonly IEmailClient _emailClient;

        public EmailSender(
            IEmailClient emailClient)
        {
            _emailClient = emailClient;
        }

        public Provider Provider => Provider.Email;

        public async Task SendAsync(
            Notification notification,
            CancellationToken cancellationToken = default)
        {
            var email = new EmailMessage(
                notification.Recipient.Value,
                notification.Content.Subject,
                notification.Content.Value);

            try
            {
                await _emailClient.SendMessageAsync(
                    email,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                throw EmailExceptionMapper.Map(ex);
            }
        }
    }
}
