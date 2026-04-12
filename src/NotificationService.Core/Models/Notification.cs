using NotificationService.Core.Enums;

namespace NotificationService.Core.Models
{
    public class Notification
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

        public Guid Id { get; }
        public NotificationType Type { get; }
        public string Sender { get; }
        public string Recipient { get; }
        public string Subject { get; }
        public string Message { get; }
        public Priority Priority { get; }
        public DateTime ScheduledAt { get; }
        public Dictionary<string, string> Metadata { get; }
    }
}
