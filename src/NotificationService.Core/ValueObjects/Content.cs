using CSharpFunctionalExtensions;
using NotificationService.Core.Primitives.Enums;

namespace NotificationService.Core.ValueObjects
{
    public sealed class Content : ValueObject
    {
        public const int MAX_CONTENT_LENGTH = 5000;
        public const int MAX_SUBJECT_LENGTH = 500;

        private Content(ContentType type, string subject, string value)
        {
            Type = type;
            Subject = subject;
            Value = value;
        }

        public ContentType Type { get; private set; }
        public string Subject { get; set; }
        public string Value { get; private set; }

        public static Result<Content> Create(
            ContentType type,
            string subject,
            string value)
        {
            if (string.IsNullOrEmpty(subject))
            {
                return Result.Failure<Content>("Subject can not be null or empty.");
            }

            if (subject.Length > MAX_SUBJECT_LENGTH)
            {
                return Result.Failure<Content>("Subject is too long.");
            }

            if (string.IsNullOrEmpty(value))
            {
                return Result.Failure<Content>("Content can not be null or empty.");
            }

            if (value.Length > MAX_CONTENT_LENGTH)
            {
                return Result.Failure<Content>("Content is too long.");
            }

            return new Content(type, subject, value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Type;
            yield return Value;
        }
    }
}
