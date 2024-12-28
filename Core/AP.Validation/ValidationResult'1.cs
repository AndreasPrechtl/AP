using System;
using System.Collections.Generic;

namespace AP.ComponentModel.Validation;

public class ValidationResult<TTarget> : ValidationResult
{
    /// <summary>
    /// cctor.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="messages"></param>
    protected ValidationResult(TTarget target, MessageSet messages)
        : base(target, messages)
    { }

    /// <summary>
    /// Creates a new ValidationResult.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="messages"></param>
    public ValidationResult(TTarget target, IEnumerable<ValidationMessage> messages)
        : base(target, messages, null)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));

        ArgumentNullException.ThrowIfNull(messages);
    }

    /// <summary>
    /// Gets the validation target.
    /// </summary>
    public new TTarget Target => (TTarget)base.Target;
}
