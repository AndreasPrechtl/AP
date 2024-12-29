using AP.Validation;

namespace AP.Validation;

public interface IValidator
{
    ValidationResult Validate(object target);
}

public interface IValidator<TTarget>
{
    ValidationResult Validate(TTarget target);
}
