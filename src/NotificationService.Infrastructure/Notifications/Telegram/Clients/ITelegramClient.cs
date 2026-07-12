
using NotificationService.Infrastructure.Notifications.Telegram.Contracts;

namespace NotificationService.Infrastructure.Notifications.Telegram.Clients
{
    public interface ITelegramClient
    {
        Task SendMessageAsync(TelegramRequest request, CancellationToken cancellationToken);
    }
}