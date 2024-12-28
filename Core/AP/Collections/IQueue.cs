using System.Collections.Generic;

namespace AP.Collections;

public interface IQueue<T> : ICollection<T>
{
    void Enqueue(T item);
    void Enqueue(IEnumerable<T> items);
    T Dequeue();
    IEnumerable<T> Dequeue(int count);
    T Peek();
    void Clear();
}
