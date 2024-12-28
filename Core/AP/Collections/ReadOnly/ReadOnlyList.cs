using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AP.Collections.ReadOnly;

[Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false), System.ComponentModel.ReadOnly(true)]
public class ReadOnlyList<T> : IListView<T>, IEqualityComparerUser<T>, System.Collections.Generic.IList<T>
{
    private static readonly ReadOnlyList<T> s_empty = new(null!, null!);

    public static ReadOnlyList<T> Empty => s_empty;

    private readonly AP.Collections.List<T> _inner;
 
    
    private static AP.Collections.List<T> CreateInner(IEnumerable<T> collection, IEqualityComparer<T> comparer)
    {
        ArgumentNullException.ThrowIfNull(collection);

        return new List<T>(collection, comparer);
    }
    
    public ReadOnlyList(IEnumerable<T> collection)
        : this(collection, null)
    { }

    public ReadOnlyList(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        : this(CreateInner(collection, comparer))
    { }

    protected ReadOnlyList(AP.Collections.List<T> inner)
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

    public void CopyTo(T[] array, int arrayIndex = 0, int listIndex = 0, int? count = null) => _inner.CopyTo(array, arrayIndex, listIndex, count);

    public int IndexOf(T item, SelectionMode mode = SelectionMode.First) => _inner.IndexOf(item, mode);

    public bool Contains(T item) => _inner.Contains(item);

    public bool Contains(T item, out int index, SelectionMode mode = SelectionMode.First) => _inner.Contains(item, out index, mode);

    public bool TryGetItem(int index, out T item) => _inner.TryGetItem(index, out item);

    #endregion

    #region ICollection<T> Members

    public int Count => _inner.Count;

    void ICollection<T>.CopyTo(T[] array, int arrayIndex) => ((ICollection<T>)_inner).CopyTo(array, arrayIndex);

    bool ICollection<T>.Contains(T item) => ((ICollection<T>)_inner).Contains(item);

    #endregion

    #region IEnumerable<T> Members

    public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

    #endregion

    #region IEnumerable Members

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    #endregion

    #region ICloneable Members

    public ReadOnlyList<T> Clone() => this;

    object ICloneable.Clone() => this.Clone();

    #endregion

    #region IEqualityComparerUser<T> Members

    public IEqualityComparer<T> Comparer => _inner.Comparer;

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

    void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int index) => this.CopyTo(array, index, 0, null);

    bool System.Collections.Generic.ICollection<T>.Remove(T item) => throw new NotSupportedException();

    #endregion
}
