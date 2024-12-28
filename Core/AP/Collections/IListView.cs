using System.Collections.Generic;

namespace AP.Collections;

public enum SelectionMode
{
    All = 1,
    First = 2,
    Last = 3
}

public interface IListView<T> : ICollection<T>, System.Collections.Generic.IReadOnlyList<T>//, IStructuralComparable<T> removed - Equality comparison within an object is just wrong - comparers should be the way to go.
{
    IEnumerable<T> this[int index, int count] { get; }

    int IndexOf(T item, SelectionMode mode = SelectionMode.First);
    
    bool Contains(T item, out int index, SelectionMode mode = SelectionMode.First);

    bool TryGetItem(int index, out T item);
}
