using System;
using System.Collections.Generic;

namespace AP.Collections;

public interface ICollection<T> : IEnumerable<T>, ICloneable, System.Collections.Generic.IReadOnlyCollection<T>
{
#warning get rid of CopyTo - use default impl.
    void CopyTo(T[] array, int arrayIndex = 0);
    bool Contains(T item);
}