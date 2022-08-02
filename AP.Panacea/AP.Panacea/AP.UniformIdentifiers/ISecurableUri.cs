using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.UniformIdentifiers
{
    public interface ISecurableUri : IRemotableUri
    {
        bool IsSecure { get; }
    }
}
