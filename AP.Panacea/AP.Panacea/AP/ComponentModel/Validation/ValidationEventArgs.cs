using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.ComponentModel.Validation
{
    public class ValidationEventArgs : EventArgs
    {
        private readonly ValidationResult _result;
        public ValidationResult Result { get { return _result; } }

        public ValidationEventArgs(ValidationResult result)
        {
            _result = result;
        }
    }
}
