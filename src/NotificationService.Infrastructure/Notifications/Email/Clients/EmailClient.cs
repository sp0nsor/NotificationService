using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Infrastructure.Notifications.Email.Contracts;
using NotificationService.Infrastructure.Notifications.Email.Options;


namespace NotificationService.Infrastructure.Notifications.Email.Clients
{
    public sealed class EmailClient : IEmailClient
    {
        private readonly SmtpOptions _options;

        public EmailClient(IOptions<SmtpOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendMessageAsync(EmailMessage email, CancellationToken cancellationToken)
        {
            var messge = new MimeMessage();

            messge.From.Add(
                new MailboxAddress(
                    _options.SenderName,
                    _options.SenderEmail));

            messge.To.Add(
                MailboxAddress.Parse(email.To));

            messge.Subject = email.Subject;

            messge.Body = new TextPart("html")
            {
                Text = email.HtmlBody
            };

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

            await client
                .SendAsync(messge, cancellationToken);

            await client
                .DisconnectAsync(true, cancellationToken);
        }
    }
}
