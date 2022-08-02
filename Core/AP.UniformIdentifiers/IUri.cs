using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AP.ComponentModel;
using System.ComponentModel;

namespace AP.UniformIdentifiers
{
    public interface IUri : ISerializable //: /*IIdentifier, IComparable<IUri>, IEquatable<IUri>*/
    {
        string FullName { get; }
        string Scheme { get; }
        string OriginalString { get; }
        bool HasOriginalString { get; }

        bool Equals(object obj);
        bool Equals(object obj, [DefaultValue(true)] bool ignoreCase);
        int GetHashCode();
        string ToString();
    }
}
