using Microsoft.Extensions.Options;
using NotificationService.Core.Exceptions;
using NotificationService.Infrastructure.Notifications.Telegram.Contracts;
using NotificationService.Infrastructure.Notifications.Telegram.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace NotificationService.Infrastructure.Notifications.Telegram.Clients
{
    public sealed class TelegramClient : ITelegramClient
    {
        private readonly HttpClient _httpClient;
        private readonly TelegramOptions _options;

        public TelegramClient(
            IHttpClientFactory httpClientFactory,
            IOptions<TelegramOptions> options)
        {
            _httpClient = httpClientFactory.CreateClient("telegram");
            _options = options.Value;
        }

        public async Task SendMessageAsync(
            TelegramRequest request,
            CancellationToken cancellationToken)
        {
            var uri = new Uri($"https://api.telegram.org/bot{_options.BotToken}/sendMessage");

            var response = await _httpClient.PostAsJsonAsync(
                uri,
                request,
                cancellationToken);

            var result = await response.Content
                .ReadFromJsonAsync<TelegramResponse>(cancellationToken);


            if (result is null)
            {
                throw new JsonException("Telegram returned empty response.");
            }

            if (!result.Ok)
            {
                throw new BadRequestException(result.Description);
            }
        }
    }
}
