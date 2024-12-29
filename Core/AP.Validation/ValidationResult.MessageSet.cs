using AP.Collections;

namespace AP.Validation;

public partial class ValidationResult
{
    public partial class MessageSet : AP.Collections.ReadOnly.ReadOnlySet<ValidationMessage>
    {
        public static new readonly MessageSet Empty = new();

        protected MessageSet()
            : this(new Set<ValidationMessage>(ValidationMessageEqualityComparer.Default))
        { }
                    
        protected internal MessageSet(Set<ValidationMessage> messages)
            : base(messages)
        { }

        public new MessageSet Clone() => this;
    }
}