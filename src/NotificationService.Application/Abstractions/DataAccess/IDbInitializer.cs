namespace NotificationService.Application.Abstractions.DataAccess
{
    public interface IDbInitializer
    {
        Task InitializeAsync();
    }
}