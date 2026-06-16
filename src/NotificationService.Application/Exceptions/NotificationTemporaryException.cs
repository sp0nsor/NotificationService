namespace NotificationService.Application.Exceptions
{
    public sealed class NotificationTemporaryException(string message) : Exception(message);
}
