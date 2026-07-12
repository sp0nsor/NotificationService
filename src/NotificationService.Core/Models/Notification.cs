using CSharpFunctionalExtensions;
using NotificationService.Core.Primitives.Enums;
using NotificationService.Core.ValueObjects;

namespace NotificationService.Core.Models
{
    public class Notification : Primitives.Entity
    {
        private Notification(
            Guid id,
            Status status,
            Recipient recipient,
            Content content,
            DateTime createdAt)
        {
            Id = id;
            Status = status;
            Recipient = recipient;
            Content = content;
            CreatedAt = createdAt;
        }

        public Status Status { get; private set; }
        public Recipient Recipient { get; private set; }
        public Content Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? SentAt { get; private set; }
        public string? Error { get; private set; }

        public static Result<Notification> Create(Guid id, Status status, Recipient recipient, Content content)
        {
            return new Notification(id, status, recipient, content, DateTime.UtcNow);
        }

        public Result ChangeStatus(Status newStatus)
        {
            if (Status == Status.Sent)
            {
                return Result.Failure("Status cannot be changed after sending.");
            }

            Status = newStatus;

            if (newStatus == Status.Sent)
            {
                SentAt = DateTime.UtcNow;
            }

            return Result.Success();
        }

        public Result SetLastError(string error)
        {
            Error = error;

            return Result.Success();
        }
    }
}
