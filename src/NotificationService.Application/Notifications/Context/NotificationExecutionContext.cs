using NotificationService.Core.Models;

namespace NotificationService.Application.Notifications.Context
{
    public sealed class NotificationExecutionContext
    {
        public Notification? Notification { get; set; }
    }
}
