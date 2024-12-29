using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Security
{
    public abstract class AuthenticatorBase
    {        
        public abstract bool TryAuthenticateByName(string userName, string password);
        public abstract bool TryAuthenticateByEmailAddress(string emailAddress, string password, out string userName);

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj == this)
                return true;

            return this.GetType() == obj.GetType();
        }

        public override int GetHashCode()
        {
            return this.GetType().GetHashCode();
        }
    }
}
