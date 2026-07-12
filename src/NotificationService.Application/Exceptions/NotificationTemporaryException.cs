namespace NotificationService.Application.Exceptions
{
    public sealed class NotificationTemporaryException(
        string message,
        Exception? innerException = null)
        : Exception(message, innerException);
}
