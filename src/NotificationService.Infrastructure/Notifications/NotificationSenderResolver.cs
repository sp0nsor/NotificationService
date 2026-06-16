using NotificationService.Application.Abstractions.Notifications;
using NotificationService.Core.Primitives.Enums;

namespace NotificationService.Infrastructure.Notifications
{
    public sealed class NotificationSenderResolver
        : INotificationSenderResolver
    {
        private readonly IReadOnlyDictionary<Provider,
            INotificationSender> _senders;

        public NotificationSenderResolver(
            IEnumerable<INotificationSender> senders)
        {
            _senders = senders.ToDictionary(x => x.Provider);
        }

        public INotificationSender Resolve(Provider provider)
        {
            return _senders[provider];
        }
    }
}
