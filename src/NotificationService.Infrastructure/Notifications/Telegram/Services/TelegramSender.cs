using NotificationService.Application.Abstractions.Notifications;
using NotificationService.Core.Models;
using NotificationService.Core.Primitives.Enums;
using NotificationService.Infrastructure.Notifications.Telegram.Clients;
using NotificationService.Infrastructure.Notifications.Telegram.Contracts;
using NotificationService.Infrastructure.Notifications.Telegram.Exceptions;

namespace NotificationService.Infrastructure.Notifications.Telegram.Services
{
    public sealed class TelegramSender
        : INotificationSender
    {
        private readonly ITelegramClient _telegramClient;

        public Provider Provider => Provider.Telegram;

        public TelegramSender(
            ITelegramClient telegramClient)
        {
            _telegramClient = telegramClient;
        }

        public async Task SendAsync(
            Notification notification,
            CancellationToken cancellationToken)
        {
            var request = new TelegramRequest(
                notification.Recipient.Value,
                notification.Content.Value);

            try
            {
                await _telegramClient.SendMessageAsync(
                    request,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                throw TelegramExceptionMapper.Map(ex);
            }
        }
    }
}
