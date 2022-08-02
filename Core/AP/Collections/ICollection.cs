using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections
{
    public interface ICollection<T> : IEnumerable<T>, ICloneable, System.Collections.Generic.IReadOnlyCollection<T>
    {
#warning get rid of CopyTo - use default impl.
        void CopyTo(T[] array, int arrayIndex = 0);
        bool Contains(T item);
    }
}