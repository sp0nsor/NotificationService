namespace NotificationService.Application.Abstractions.Messaging
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(T message);
        Task SendAsync<T>(T message);
    }
}