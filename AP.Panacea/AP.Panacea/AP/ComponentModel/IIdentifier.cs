using System;

namespace AP.ComponentModel
{
    public interface IIdentifier : IComparable<IIdentifier>, IEquatable<IIdentifier>, IComparable
    {
        bool Equals(object other);
        int GetHashCode();
        string ToString();
    }
}