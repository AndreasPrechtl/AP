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
        void CopyTo(T[] array, int arrayIndex = 0); 
        bool Contains(T item);
    }

    public interface ICollection : IEnumerable, ICloneable, System.Collections.ICollection
    {
        new void CopyTo(Array array, int arrayIndex = 0);
        bool Contains(object item);
    }
}