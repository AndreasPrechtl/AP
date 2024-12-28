using System;
using System.Collections.Generic;

namespace AP.Collections;

public interface ICollection<T> : IEnumerable<T>, ICloneable, System.Collections.Generic.IReadOnlyCollection<T>
{
    bool Contains(T item);
}