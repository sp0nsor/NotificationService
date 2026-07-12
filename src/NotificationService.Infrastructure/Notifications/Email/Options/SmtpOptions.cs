namespace NotificationService.Infrastructure.Notifications.Email.Options
{
    public class SmtpOptions
    {
        public required string Host { get; set; }
        public required int Port { get; set; }

        public required string UserName { get; set; }
        public required string Password { get; set; }

        public required string SenderEmail { get; set; }
        public required string SenderName { get; set; }
    }
}
