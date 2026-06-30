using NotificationService.Application.Abstractions.Integrations;
using NotificationService.Application.DTOs;
using NotificationService.Core.Primitives.Enums;

namespace NotificationService.Infrastructure.Integrations.UserService
{
    internal sealed class UserServiceClient : IUserServiceClient
    {
        public async Task<List<UserContactsDto>> GetContacts(Guid id)
        {
            await Task.Delay(1000);

            return new List<UserContactsDto>() { new UserContactsDto(id, Provider.Email, "willis.orn@ethereal.email") };
        }
    }
}
