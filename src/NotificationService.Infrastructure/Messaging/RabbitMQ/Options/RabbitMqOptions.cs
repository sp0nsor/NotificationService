namespace NotificationService.Infrastructure.Messaging.RabbitMQ.Options
{
    internal sealed class RabbitMqOptions
    {
        public required string ConnectionString { get; set; }
        public required string ExchangeName { get; set; }
        public required string QueueName { get; set; }
        public required string RoutingKey { get; set; }
    }
}
