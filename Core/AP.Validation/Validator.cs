namespace AP.ComponentModel.Validation;

public class Validator<TTarget> : IValidator<TTarget>, IValidator
{
    private static volatile Validator<TTarget> _default;

    /// <summary>
    /// Gets the default validator.
    /// </summary>
    public static Validator<TTarget> Default
    {
        get 
        {
            Validator<TTarget> d = _default;

            if (d == null)
                _default = d = new Validator<TTarget>();

            return d;
        }
    }
           
    /// <summary>
    /// Validates an object.
    /// </summary>
    /// <param name="value">The object to validate, throws an ArgumentNullException when the value is null.</param>
    /// <returns>The validation result.</returns>
    public virtual ValidationResult<TTarget> Validate(TTarget value)
    {
        if (value is IValidateable)
            return (ValidationResult<TTarget>)((IValidateable)value).Validate();

        return DataAnnotationsValidator<TTarget>.Default.Validate(value);
    }

    #region IValidator<T> Members

    /// <summary>
    /// Validates an object.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <returns>The validation result.</returns>
    ValidationResult IValidator<TTarget>.Validate(TTarget value) => this.Validate(value);

    #endregion

    #region IValidator Members

    /// <summary>
    /// Validates an object.
    /// </summary>
    /// <param name="value">The object to validate.</param>
    /// <returns>The validation result.</returns>
    ValidationResult IValidator.Validate(object value) => this.Validate((TTarget)value);

    #endregion
}
