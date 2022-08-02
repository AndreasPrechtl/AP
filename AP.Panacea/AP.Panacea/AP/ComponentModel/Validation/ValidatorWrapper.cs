using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.ComponentModel.Validation
{
    public delegate ValidationResult<T> ValidatorDelegate<T>(T obj);

    public sealed class ValidatorDelegateWrapper<T> : Validator<T>
    {
        private readonly ValidatorDelegate<T> _validator;

        public ValidatorDelegateWrapper(ValidatorDelegate<T> validator)
            : base()
        {
            if (validator == null)
                throw new ArgumentNullException();

            _validator = validator;
        }

        public override ValidationResult<T> Validate(T value)
        {
            return _validator(value);
        }
    }
}
