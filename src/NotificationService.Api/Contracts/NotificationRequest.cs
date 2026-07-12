using NotificationService.Core.Primitives.Enums;

namespace NotificationService.Api.Contracts
{
    public record NotificationRequest(
        Guid UserId,
        Provider Provider,
        string RecipientValue,
        ContentType ContentType,
        string Subject,
        string ContentValue);
}
