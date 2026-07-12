namespace NotificationService.Infrastructure.Notifications.Email.Contracts
{
    public record EmailMessage(
        string To,
        string Subject,
        string HtmlBody);
}
