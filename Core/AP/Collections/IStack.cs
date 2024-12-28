using System.Collections.Generic;

namespace AP.Collections;

public interface IStack<T> : ICollection<T>
{
    T Pop();
    IEnumerable<T> Pop(int count);
    void Push(T item);
    void Push(IEnumerable<T> items);
    T Peek();
    void Clear();        
}
