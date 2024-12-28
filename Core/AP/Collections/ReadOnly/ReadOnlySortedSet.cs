using System;
using System.Collections.Generic;
using System.Collections;
using AP.ComponentModel;

namespace AP.Collections.ReadOnly;

[Serializable, System.ComponentModel.ReadOnly(true), Sorted(true)]
public class ReadOnlySortedSet<T> : ISetView<T>, IComparerUser<T>, System.Collections.Generic.ISet<T>
{
    private readonly AP.Collections.SortedSet<T> _inner;
    private static readonly ReadOnlySortedSet<T> s_empty = new ReadOnlySortedSet<T>([]);

    public static ReadOnlySortedSet<T> Empty => s_empty;

    private static AP.Collections.SortedSet<T> CreateInner(IEnumerable<T> collection, IComparer<T> comparer)
    {
        ArgumentNullException.ThrowIfNull(collection);

        return new SortedSet<T>(collection, comparer);
    }

    public ReadOnlySortedSet(IEnumerable<T> collection)
        : this(collection, null)
    { }

    public ReadOnlySortedSet(IEnumerable<T> collection, IComparer<T> comparer)
        : this(CreateInner(collection, comparer))
    { }

    protected ReadOnlySortedSet(AP.Collections.SortedSet<T> inner)
    {
        ArgumentNullException.ThrowIfNull(inner);

        _inner = inner;
    }

    public override string ToString() => _inner.ToString();

    #region ISetView<T> Members

    public bool IsSubsetOf(IEnumerable<T> other) => _inner.IsSubsetOf(other);

    public bool IsSupersetOf(IEnumerable<T> other) => _inner.IsSupersetOf(other);

    public bool IsProperSupersetOf(IEnumerable<T> other) => _inner.IsProperSupersetOf(other);

    public bool IsProperSubsetOf(IEnumerable<T> other) => _inner.IsProperSubsetOf(other);

    public bool Overlaps(IEnumerable<T> other) => _inner.Overlaps(other);

    public bool SetEquals(IEnumerable<T> other) => _inner.SetEquals(other);

    #endregion

    #region ICollection<T> Members

    public int Count => _inner.Count;

    public void CopyTo(T[] array, int arrayIndex = 0) => _inner.CopyTo(array, arrayIndex);

    public bool Contains(T item) => _inner.Contains(item);

    #endregion

    #region IEnumerable<T> Members

    public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

    #endregion

    #region IEnumerable Members

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    #endregion

    #region ICloneable Members

    public virtual ReadOnlySortedSet<T> Clone() => this;

    object ICloneable.Clone() => this.Clone();

    #endregion

    #region IComparerUser<T> Members

    public IComparer<T> Comparer => _inner.Comparer;

    #endregion

    #region ISet<T> Members

    bool System.Collections.Generic.ISet<T>.Add(T item) => throw new NotSupportedException();

    void System.Collections.Generic.ISet<T>.ExceptWith(IEnumerable<T> other) => throw new NotSupportedException();

    void System.Collections.Generic.ISet<T>.IntersectWith(IEnumerable<T> other) => throw new NotSupportedException();

    void System.Collections.Generic.ISet<T>.SymmetricExceptWith(IEnumerable<T> other) => throw new NotSupportedException();

    void System.Collections.Generic.ISet<T>.UnionWith(IEnumerable<T> other) => throw new NotSupportedException();

    #endregion

    #region ICollection<T> Members

    void System.Collections.Generic.ICollection<T>.Add(T item) => throw new NotSupportedException();

    void System.Collections.Generic.ICollection<T>.Clear() => throw new NotSupportedException();

    bool System.Collections.Generic.ICollection<T>.IsReadOnly => true;

    bool System.Collections.Generic.ICollection<T>.Remove(T item) => throw new NotSupportedException();

    #endregion
}
