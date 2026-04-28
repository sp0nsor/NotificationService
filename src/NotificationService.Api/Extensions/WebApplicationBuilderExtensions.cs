using NotificationService.Core.Abstractions;

namespace NotificationService.Api.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var dbInitialize = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitialize.InitializeAsync();
        }
    }
}
