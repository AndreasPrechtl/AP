using System.Collections.Generic;

namespace AP.Collections;

public interface IList<T> : IListView<T>, System.Collections.Generic.ICollection<T>
{
    new int Add(T item);
    void Add(IEnumerable<T> items);
    void Remove(int index, int count = 1);
    void Remove(T item, SelectionMode mode = SelectionMode.First);
    new void Clear();
}
