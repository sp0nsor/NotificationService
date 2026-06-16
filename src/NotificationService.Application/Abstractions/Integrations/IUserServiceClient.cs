using NotificationService.Application.DTOs;

namespace NotificationService.Application.Abstractions.Integrations
{
    public interface IUserServiceClient
    {
        Task<List<UserContactsDto>> GetContacts(Guid id);
    }
}