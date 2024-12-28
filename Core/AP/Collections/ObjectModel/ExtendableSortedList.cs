using AP.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AP.Collections.ObjectModel;

[Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false), Sorted(true)]
public abstract class ExtendableSortedList<T> : ListBase<T>, IComparerUser<T>
{
    private readonly AP.Collections.SortedList<T> _inner;

    public ExtendableSortedList()
        : this(0, null)
    { }

    public ExtendableSortedList(IComparer<T> comparer)
        : this(0, comparer)
    { }

    public ExtendableSortedList(int capacity)
        : this(capacity, null)
    { }

    public ExtendableSortedList(int capacity, IComparer<T> comparer)
        : this(new AP.Collections.SortedList<T>(capacity, comparer))
    { }

    public ExtendableSortedList(IEnumerable<T> collection)
        : this(collection, null)
    { }

    public ExtendableSortedList(IEnumerable<T> collection, IComparer<T> comparer)
        : this(new AP.Collections.SortedList<T>(collection, comparer))
    { }

    protected ExtendableSortedList(AP.Collections.SortedList<T> inner)
    {
        ArgumentNullException.ThrowIfNull(inner);

        _inner = inner;
    }

    public new ExtendableSortedList<T> Clone() => (ExtendableSortedList<T>)this.OnClone();

    public override string ToString() => _inner.ToString();

    public override int Add(T item) => _inner.Add(item);

    public override void Add(IEnumerable<T> items) => _inner.Add(items);

    public override void Remove(int index, int count = 1) => _inner.Remove(index, count);

    public override void CopyTo(T[] array, int arrayIndex = 0, int listIndex = 0, int? count = null) => _inner.CopyTo(array, arrayIndex, listIndex, count);

    public override void Remove(T item, SelectionMode mode = SelectionMode.First) => _inner.Remove(item, mode);

    public override void Clear() => _inner.Clear();

    protected sealed override T GetItem(int index) => _inner[index];

    public override IEnumerable<T> this[int index, int count = 1]
    {
        get
        {
            return _inner[index, count];
        }
    }

    public override int Count => _inner.Count;

    public override IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

    #region IComparerUser<T> Members

    public IComparer<T> Comparer => _inner.Comparer;

    #endregion
}
