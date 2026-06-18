using Microsoft.Extensions.Options;
using NotificationService.Application.Abstractions.Notifications;
using NotificationService.Application.Exceptions;
using NotificationService.Core.Exceptions;
using NotificationService.Core.Models;
using NotificationService.Core.Primitives.Enums;
using NotificationService.Infrastructure.Notifications.Telegram.Contracts;
using NotificationService.Infrastructure.Notifications.Telegram.Options;
using System.Net.Http.Json;

namespace NotificationService.Infrastructure.Notifications.Telegram.Services
{
    public sealed class TelegramSender
        : INotificationSender
    {
        private readonly HttpClient _httpClient;
        private readonly TelegramOptions _options;

        public Provider Provider => Provider.Telegram;

        public TelegramSender(
            HttpClient httpClient,
            IOptions<TelegramOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task SendAsync(
            Notification notification,
            CancellationToken cancellationToken = default)
        {
            var request = new
            {
                chat_id = notification.Recipient.Value,
                text = notification.Content.Value
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"https://api.telegram.org/bot{_options.BotToken}/sendMessage",
                request,
                cancellationToken);

            if ((int)response.StatusCode >= 500)
            {
                throw new NotificationTemporaryException(
                    "Telegram API temporarily unavailable.");
            }

            var result = await response.Content
                .ReadFromJsonAsync<TelegramResponse>(cancellationToken);

            if (result is null || !result.Ok)
            {
                throw new BadRequestException(
                    result?.Decsription ?? "Telegram send failed.");
            }
        }
    }
}
