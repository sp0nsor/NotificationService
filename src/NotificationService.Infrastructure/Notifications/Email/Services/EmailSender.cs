using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Application.Abstractions.Notifications;
using NotificationService.Application.Exceptions;
using NotificationService.Core.Models;
using NotificationService.Core.Primitives.Enums;
using NotificationService.Infrastructure.Notifications.Email.Options;

namespace NotificationService.Infrastructure.Notifications.Email.Services
{
    internal sealed class EmailSender
        : INotificationSender
    {
        private readonly SmtpOptions _options;

        public EmailSender(IOptions<SmtpOptions> options)
        {
            _options = options.Value;
        }

        public Provider Provider => Provider.Email;

        public async Task SendAsync(
            Notification notification,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var message = new MimeMessage();

                message.From.Add(
                    new MailboxAddress(
                        _options.SenderName,
                        _options.SenderEmail));

                message.To.Add(
                    MailboxAddress.Parse(
                        notification.Recipient.Value));

                message.Body = new TextPart("html")
                {
                    Text = notification.Content.Value
                };

                message.Subject = "Super Puper Subject";

                using var client = new SmtpClient();

                await client.ConnectAsync(
                    _options.Host,
                    _options.Port,
                    MailKit.Security.SecureSocketOptions.StartTls,
                    cancellationToken);

                await client.AuthenticateAsync(
                    _options.UserName,
                    _options.Password,
                    cancellationToken);

                var response = await client.SendAsync(
                    message,
                    cancellationToken);

                await client.DisconnectAsync(

                    true,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                throw new NotificationTemporaryException(ex.Message);
            }
        }
    }
}
