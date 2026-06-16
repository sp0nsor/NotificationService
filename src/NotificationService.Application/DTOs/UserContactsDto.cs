using NotificationService.Core.Primitives.Enums;

namespace NotificationService.Application.DTOs
{
    public record UserContactsDto(
        Guid UserId,
        Provider Provider,
        string Value);
}
