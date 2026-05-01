using NotificationService.Core.Models;
using NotificationService.Core.Primitives.Enums;

namespace NotificationService.DataAccess.Data
{
    public class NotificationsData
    {
        public IEnumerable<Notification> Notifications =>
        [
            new Notification(
                NotificationType.Email,
                "noreply@company.com",
                "user@example.com",
                "Подтверждение регистрации",
                "Добро пожаловать! Пожалуйста, подтвердите ваш email.",
                Priority.Normal,
                new Dictionary<string, string>
                {
                    { "TemplateId", "welcome_email_v1" },
                    { "Campaign", "onboarding" }
                }),

            new Notification(
                NotificationType.Telegram,
                "@AlertBot",
                "440921342",
                "Критическая ошибка",
                "Сервер API-01 недоступен более 5 минут!",
                Priority.Critical,
                new Dictionary<string, string>
                {
                    { "ServerId", "nodes-cluster-1" },
                    { "ErrorCode", "ERR_TIMEOUT" },
                    { "ParseMode", "MarkdownV2" }
                }),

            new Notification(
                NotificationType.Email,
                "billing@service.io",
                "customer@domain.net",
                "Счет на оплату",
                "Ваша подписка истекает через 3 дня.",
                Priority.High,
                new Dictionary<string, string>
                {
                    { "InvoiceId", "INV-2023-001" },
                    { "Amount", "29.99" },
                    { "Currency", "USD" }
                }),

            new Notification(
                NotificationType.Telegram,
                "@MarketingBot",
                "99238411",
                "Акция!",
                "Только сегодня скидки на все курсы до 50%.",
                Priority.Low,
                new Dictionary<string, string>
                {
                    { "PromoCode", "SPRING2024" },
                    { "ButtonLink", "https://mysite.com" }
                })
        ];
    }
}