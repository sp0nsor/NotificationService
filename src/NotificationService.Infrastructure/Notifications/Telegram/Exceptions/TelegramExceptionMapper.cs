using NotificationService.Application.Exceptions;
using NotificationService.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace NotificationService.Infrastructure.Notifications.Telegram.Exceptions
{
    public static class TelegramExceptionMapper
    {
        public static Exception Map(Exception exception)
        {
            return exception switch
            {
                NotificationTemporaryException or BadRequestException
                    => exception,

                HttpRequestException httpEx => httpEx.StatusCode switch
                {
                    HttpStatusCode.BadRequest =>
                        new BadRequestException("Telegram rejected the request."),

                    HttpStatusCode.Unauthorized =>
                        new BadRequestException("Telegram bot token is invalid."),

                    HttpStatusCode.Forbidden =>
                        new BadRequestException("Bot has no access to this chat."),

                    HttpStatusCode.NotFound =>
                        new BadRequestException("Telegram endpoint was not found."),

                    HttpStatusCode.TooManyRequests =>
                        new NotificationTemporaryException(
                            "Telegram rate limit exceeded.",
                            httpEx),

                    HttpStatusCode.InternalServerError or
                    HttpStatusCode.BadGateway or
                    HttpStatusCode.ServiceUnavailable or
                    HttpStatusCode.GatewayTimeout =>
                        new NotificationTemporaryException(
                            "Telegram API is temporarily unavailable.",
                            httpEx),

                    _ => new NotificationTemporaryException(
                            "Telegram request failed.",
                            httpEx)
                },

                TaskCanceledException =>
                    new NotificationTemporaryException(
                        "Telegram request time out.",
                        exception),

                JsonException =>
                    new NotificationTemporaryException(
                        "Telegram returned invalid JSON.",
                        exception),

                _ =>
                    new NotificationTemporaryException(
                        "Unexpected Telegram error.",
                        exception)
            };
        }
    }
}
