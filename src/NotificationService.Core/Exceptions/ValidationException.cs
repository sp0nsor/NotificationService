namespace NotificationService.Core.Exceptions
{
    public sealed class ValidationException(string message) : Exception(message);
}
