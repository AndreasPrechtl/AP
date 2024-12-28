using System;
using System.Collections.Generic;
using System.Collections;

namespace AP.Collections.ReadOnly;

[Serializable, System.ComponentModel.ReadOnly(true)]
public class ReadOnlySet<T> : ISetView<T>, IEqualityComparerUser<T>, System.Collections.Generic.ISet<T>
{
    private readonly AP.Collections.Set<T> _inner;
    private static readonly ReadOnlySet<T> s_empty = new ReadOnlySet<T>([]);

    public static ReadOnlySet<T> Empty => s_empty;

    private static AP.Collections.Set<T> CreateInner(IEnumerable<T> collection, IEqualityComparer<T> comparer)
    {
        ArgumentNullException.ThrowIfNull(collection);

        return new Set<T>(collection, comparer);
    }

    public ReadOnlySet(IEnumerable<T> collection)
        : this(collection, null)
    { }

    public ReadOnlySet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        : this(CreateInner(collection, comparer))
    { }

    protected ReadOnlySet(AP.Collections.Set<T> inner)
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

    public ReadOnlySet<T> Clone() => this;

    object ICloneable.Clone() => this.Clone();

    #endregion

    #region IEqualityComparerUser<T> Members

    public IEqualityComparer<T> Comparer => _inner.Comparer;

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
