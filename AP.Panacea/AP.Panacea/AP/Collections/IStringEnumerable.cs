using AP.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Collections
{
    public interface IStringEnumerable : IEnumerable<string>
    {
        string ToString(string separator = StringEnumerable.DefaultSeparator, bool filterWhitespaces = true);
        bool Contains(string value, StringComparison comparisonType = StringComparison.Ordinal);
    }
}
