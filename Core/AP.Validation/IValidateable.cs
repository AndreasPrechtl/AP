namespace AP.ComponentModel.Validation;

/// <summary>
/// Interface for objects that have their own validation code.
/// </summary>
public interface IValidateable
{
    ValidationResult Validate();
}