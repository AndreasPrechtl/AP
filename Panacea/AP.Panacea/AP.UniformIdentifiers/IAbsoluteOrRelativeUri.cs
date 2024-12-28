using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.UniformIdentifiers
{
    public interface IAbsoluteOrRelativeUri : IHierarchicalUri
    {
        bool IsAbsolute { get; }
    }
}
