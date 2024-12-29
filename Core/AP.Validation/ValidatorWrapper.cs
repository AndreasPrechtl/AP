using System;

namespace AP.Validation;

public delegate ValidationResult<T> ValidatorDelegate<T>(T obj);

public sealed class ValidatorDelegateWrapper<T> : Validator<T>
{
    private readonly ValidatorDelegate<T> _validator;

    public ValidatorDelegateWrapper(ValidatorDelegate<T> validator)
        : base()
    {
        ArgumentNullException.ThrowIfNull(validator);

        _validator = validator;
    }

    public override ValidationResult<T> Validate(T value) => _validator(value);
}
