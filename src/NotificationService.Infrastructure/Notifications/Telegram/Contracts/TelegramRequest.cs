namespace NotificationService.Infrastructure.Notifications.Telegram.Contracts
{
    public record TelegramRequest(
        string chat_id,
        string text);
}
