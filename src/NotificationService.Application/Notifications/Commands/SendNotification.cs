namespace NotificationService.Application.Notifications.Commands
{
    public class SendNotification
    {
        public string Type { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string Recipient { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
    }
}
