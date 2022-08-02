using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections
{
    public interface IStack<T> : ICollection<T>
    {
        T Pop();
        IEnumerable<T> Pop(int count);
        void Push(T item);
        void Push(IEnumerable<T> items);
        T Peek();
        void Clear();        
    }
}
