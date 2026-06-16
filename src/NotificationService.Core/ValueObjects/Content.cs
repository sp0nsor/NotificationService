using CSharpFunctionalExtensions;
using NotificationService.Core.Primitives.Enums;

namespace NotificationService.Core.ValueObjects
{
    public sealed class Content : ValueObject
    {
        public const int MAX_CONTENT_LENGTH = 3000;

        private Content(ContentType type, string value)
        {
            Type = type;
            Value = value;
        }

        public ContentType Type { get; private set; }
        public string Value { get; private set; }

        public static Result<Content> Create(
            ContentType type, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Result.Failure<Content>("Content can not be null or empty.");
            }

            if (value.Length > MAX_CONTENT_LENGTH)
            {
                return Result.Failure<Content>("Content is too long.");
            }

            return new Content(type, value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Type;
            yield return Value;
        }
    }
}
