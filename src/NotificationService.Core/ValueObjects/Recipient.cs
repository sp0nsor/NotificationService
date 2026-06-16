using CSharpFunctionalExtensions;
using NotificationService.Core.Primitives.Enums;

namespace NotificationService.Core.ValueObjects
{
    public sealed class Recipient : ValueObject
    {
        private Recipient(Guid userId, Provider provider, string value)
        {
            UserId = userId;
            Provider = provider;
            Value = value;
        }

        public Guid UserId { get; private set; }
        public Provider Provider { get; private set; }
        public string Value { get; private set; }

        public static Result<Recipient> Create(Guid userId, Provider provider, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<Recipient>("Recipient is empty.");
            }

            return new Recipient(userId, provider, value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return UserId;
            yield return Provider;
            yield return Value;
        }
    }
}
