using AP.Collections;
using AP.Collections.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Linq;

namespace AP.ComponentModel.Validation
{
    public partial class ValidationResult
    {
        public partial class MessageSet : AP.Collections.ReadOnly.ReadOnlySet<ValidationMessage>
        {
            private static volatile MessageSet _empty;

            public new static MessageSet Empty
            {
                get
                {
                    MessageSet empty = _empty;

                    if (empty == null)
                        _empty = empty = new MessageSet();

                    return empty;
                }
            }

            protected MessageSet()
                : this(new Set<ValidationMessage>(ValidationMessageEqualityComparer.Default))
            { }
                        
            protected internal MessageSet(Set<ValidationMessage> messages)
                : base(messages)
            { }

            public new MessageSet Clone()
            {
                return this;
            }
        }
    }
}