using MailKit.Net.Smtp;
using MailKit.Security;
using NotificationService.Application.Exceptions;
using NotificationService.Core.Exceptions;
using System.Net.Sockets;

namespace NotificationService.Infrastructure.Notifications.Email.Exceptions
{
    public static class EmailExceptionMapper
    {
        public static Exception Map(Exception exception)
        {
            return exception switch
            {
                NotificationTemporaryException or BadRequestException
                    => exception,

                SmtpCommandException smtpEx when smtpEx.StatusCode == SmtpStatusCode.MailboxUnavailable
                    => new BadRequestException(
                        "Recipient mailbox does not exist."),

                SmtpCommandException smtpEx when smtpEx.StatusCode == SmtpStatusCode.MailboxNameNotAllowed
                    => new BadRequestException(
                        "Recipient address is invalid."),

                SmtpCommandException
                    => new NotificationTemporaryException(
                        "SMTP server rejected the command.",
                        exception),

                SmtpProtocolException
                    => new NotificationTemporaryException(
                        "SMTP protocol error.",
                        exception),

                SocketException
                    => new NotificationTemporaryException(
                        "Unable to connect to SMTP server.",
                        exception),

                AuthenticationException
                    => new NotificationTemporaryException(
                        "SMTP authentication failed.",
                        exception),

                TimeoutException
                    => new NotificationTemporaryException(
                        "SMTP request timed out.",
                        exception),

                _ => new NotificationTemporaryException(
                        "Unexpected email sending error.",
                        exception)
            };
        }
    }
}
