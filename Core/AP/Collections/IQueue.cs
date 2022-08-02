using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections
{
    public interface IQueue<T> : ICollection<T>
    {
        void Enqueue(T item);
        void Enqueue(IEnumerable<T> items);
        T Dequeue();
        IEnumerable<T> Dequeue(int count);
        T Peek();
        void Clear();
    }
}
