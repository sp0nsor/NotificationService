using NotificationService.Core.Primitives;
using NotificationService.Core.Primitives.Enums;

namespace NotificationService.Core.Models
{
    public class Notification : Entity
    {
        public Notification(
            NotificationType type,
            string sender,
            string recipient,
            string subject,
            string message,
            Priority priority,
            Dictionary<string, string> metadata)
        {
            Id = Guid.NewGuid();
            Type = type;
            Sender = sender;
            Recipient = recipient;
            Subject = subject;
            Message = message;
            Priority = priority;
            ScheduledAt = DateTime.UtcNow;
            Metadata = metadata;
        }

        public NotificationType Type { get; init; }
        public string Sender { get; init; }
        public string Recipient { get; init; }
        public string Subject { get; init; }
        public string Message { get; init; }
        public Priority Priority { get; init; }
        public DateTime ScheduledAt { get; init; }
        public Dictionary<string, string> Metadata { get; init; }
    }
}
