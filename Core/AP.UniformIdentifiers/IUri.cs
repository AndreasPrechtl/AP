using System;

namespace AP.UniformIdentifiers;

public interface IUri : IComparable<IUri>, IEquatable<IUri> //, IIdentifier
{
    string FullName { get; }
    string Scheme { get; }
    string OriginalString { get; }
    bool HasOriginalString { get; }

    bool Equals(object obj, bool ignoreCase = true);
    int GetHashCode();
    string ToString();
}
