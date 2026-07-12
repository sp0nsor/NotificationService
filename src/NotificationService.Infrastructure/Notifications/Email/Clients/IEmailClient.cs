using NotificationService.Infrastructure.Notifications.Email.Contracts;

public interface IEmailClient
{
    Task SendMessageAsync(EmailMessage email, CancellationToken cancellationToken);
}