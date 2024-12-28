using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.ComponentModel.Validation
{
    public class ValidationMessageEqualityComparer : EqualityComparer<ValidationMessage>
    {
        private static volatile ValidationMessageEqualityComparer _default;

        public static new ValidationMessageEqualityComparer Default
        {
            get
            {
                ValidationMessageEqualityComparer d = _default;

                if (d == null)
                    _default = d = new ValidationMessageEqualityComparer();

                return d;
            }
        }

        public override bool Equals(ValidationMessage x, ValidationMessage y)
        {
            if (x == y)
                return true;

            if (x == null || y == null)
                return false;

            return x.MemberPath.Equals(y.MemberPath) && x.Type == y.Type && string.Equals(x.Message, y.Message, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode(ValidationMessage obj)
        {
            return obj.MemberPath.GetHashCode();
        }
    }
}
