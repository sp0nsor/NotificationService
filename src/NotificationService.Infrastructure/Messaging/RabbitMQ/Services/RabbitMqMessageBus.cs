using NotificationService.Application.Abstractions.Messaging;

namespace NotificationService.Infrastructure.Messaging.RabbitMQ.Services
{
    public class RabbitMqMessageBus : IMessageBus
    {
        private readonly Wolverine.IMessageBus _bus;

        public RabbitMqMessageBus(Wolverine.IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task PublishAsync<T>(T message)
            => await _bus.PublishAsync(message);

        public async Task SendAsync<T>(T message)
            => await _bus.SendAsync(message);
    }
}
