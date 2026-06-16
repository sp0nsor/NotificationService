using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Abstractions.Integrations;
using NotificationService.Infrastructure.Integrations.UserService;

namespace NotificationService.Infrastructure.Integrations.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddIntegrations(this IServiceCollection services)
        {
            services.AddScoped<IUserServiceClient, UserServiceClient>();
        }
    }
}
