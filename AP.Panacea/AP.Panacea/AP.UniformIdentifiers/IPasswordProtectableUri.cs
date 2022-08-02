using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.UniformIdentifiers
{
    public interface IPasswordProtectableUri : IUriWithUserName
    {
        string Password { get; }
        //string PasswordSeparator { get; }
    }
}
