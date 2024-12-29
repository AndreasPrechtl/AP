using AP.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AP.Collections.ReadOnly;

[Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false), System.ComponentModel.ReadOnly(true), Sorted(true)]
public class ReadOnlySortedList<T> : IListView<T>, IComparerUser<T>, System.Collections.Generic.IList<T>
{
    public static readonly ReadOnlySortedList<T> Empty = new([]);

    private readonly AP.Collections.SortedList<T> _inner;
    
    private static AP.Collections.SortedList<T> CreateInner(IEnumerable<T> collection, IComparer<T> comparer)
    {
        ArgumentNullException.ThrowIfNull(collection);

        return new SortedList<T>(collection, comparer);
    }

    public ReadOnlySortedList(params IEnumerable<T> collection)
        : this(collection, null!)
    { }

    public ReadOnlySortedList(IEnumerable<T> collection, IComparer<T> comparer)
        : this(CreateInner(collection, comparer))
    { }

    protected ReadOnlySortedList(AP.Collections.SortedList<T> inner)
    {
        ArgumentNullException.ThrowIfNull(inner);

        _inner = inner;
    }

    public override string ToString() => _inner.ToString();

    #region IListView<T> Members

    public T this[int index]
    {
        get { return _inner[index]; }
    }

    public IEnumerable<T> this[int index, int count = 1]
    {
        get { return _inner[index, count]; }
    }

    public int IndexOf(T item, SelectionMode mode = SelectionMode.First) => _inner.IndexOf(item, mode);

    public bool Contains(T item) => _inner.Contains(item);

    public bool Contains(T item, out int index, SelectionMode mode = SelectionMode.First) => _inner.Contains(item, out index, mode);

    public bool TryGetItem(int index, out T item) => _inner.TryGetItem(index, out item);

    #endregion

    #region ICollection<T> Members

    public int Count => _inner.Count;
        
    bool ICollection<T>.Contains(T item) => ((ICollection<T>)_inner).Contains(item);

    #endregion

    #region IEnumerable<T> Members

    public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

    #endregion

    #region IEnumerable Members

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    #endregion

    #region ICloneable Members

    public ReadOnlySortedList<T> Clone() => this;

    object ICloneable.Clone() => this.Clone();

    #endregion

    #region IComparerUser<T> Members

    public IComparer<T> Comparer => _inner.Comparer;

    #endregion

    #region IList<T> Members

    int System.Collections.Generic.IList<T>.IndexOf(T item) => this.IndexOf(item, SelectionMode.First);

    void System.Collections.Generic.IList<T>.Insert(int index, T item) => throw new NotSupportedException();

    void System.Collections.Generic.IList<T>.RemoveAt(int index) => throw new NotSupportedException();

    T System.Collections.Generic.IList<T>.this[int index]
    {
        get => this[index];
        set => throw new NotSupportedException();
    }

    #endregion

    #region ICollection<T> Members

    void System.Collections.Generic.ICollection<T>.Add(T item) => throw new NotSupportedException();

    void System.Collections.Generic.ICollection<T>.Clear() => throw new NotSupportedException();

    bool System.Collections.Generic.ICollection<T>.IsReadOnly => true;

    void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex) => CollectionsHelper.CopyTo(this, array, arrayIndex);

    bool System.Collections.Generic.ICollection<T>.Remove(T item) => throw new NotSupportedException();

    #endregion
}
