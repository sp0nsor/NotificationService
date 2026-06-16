namespace NotificationService.Core.Exceptions
{
    public sealed class BadRequestException(string message) : Exception(message);
}
