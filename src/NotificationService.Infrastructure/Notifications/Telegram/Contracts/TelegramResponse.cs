namespace NotificationService.Infrastructure.Notifications.Telegram.Contracts
{
    internal record TelegramResponse(
        bool Ok,
        string? Decsription);
}
