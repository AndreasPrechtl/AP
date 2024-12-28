using AP.ComponentModel;
using System;
using System.Collections.Generic;

namespace AP.Collections.ObjectModel;

[Serializable, Sorted(true)]
public abstract class ExtendableSortedSet<T> : SetBase<T>, IComparerUser<T>
{
    private readonly SortedSet<T> _inner;

    protected SortedSet<T> Inner => _inner;

    protected ExtendableSortedSet()
        : this(null!, null!)
    { }

    protected ExtendableSortedSet(params IEnumerable<T> collection)
        : this(collection, null!)
    { }

    protected ExtendableSortedSet(IComparer<T> comparer)
        : this(null!, comparer)
    { }
    
    protected ExtendableSortedSet(IEnumerable<T> collection, IComparer<T> comparer)
        : this(new SortedSet<T>(collection, comparer))
    { }

    protected ExtendableSortedSet(SortedSet<T> inner)
    {
        ArgumentNullException.ThrowIfNull(inner);
        _inner = inner;
    }

    public override string ToString() => _inner.ToString();

    public new ExtendableSortedSet<T> Clone() => (ExtendableSortedSet<T>)this.OnClone();

    #region ISet<T> Members

    public override bool Add(T item) => _inner.Add(item);

    public override void ExceptWith(IEnumerable<T> other) => _inner.ExceptWith(other);

    public override void IntersectWith(IEnumerable<T> other) => _inner.IntersectWith(other);

    public override bool IsProperSubsetOf(IEnumerable<T> other) => _inner.IsProperSubsetOf(other);

    public override bool IsProperSupersetOf(IEnumerable<T> other) => _inner.IsProperSupersetOf(other);

    public override bool IsSubsetOf(IEnumerable<T> other) => _inner.IsSubsetOf(other);

    public override bool IsSupersetOf(IEnumerable<T> other) => _inner.IsSupersetOf(other);

    public override bool Overlaps(IEnumerable<T> other) => _inner.Overlaps(other);

    public override bool SetEquals(IEnumerable<T> other) => _inner.SetEquals(other);

    public override void SymmetricExceptWith(IEnumerable<T> other) => _inner.SymmetricExceptWith(other);

    public override void UnionWith(IEnumerable<T> other) => _inner.UnionWith(other);

    public override void Clear() => _inner.Clear();

    #endregion

    #region ICollection<T> Members

    public override bool Contains(T item) => _inner.Contains(item);

    public override int Count => _inner.Count;

    public override bool Remove(T item) => _inner.Remove(item);

    #endregion

    #region IEnumerable<T> Members

    public override IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

    #endregion

    #region IEnumerable Members

    #endregion

    #region IComparerUser<T> Members

    public IComparer<T> Comparer => _inner.Comparer;

    #endregion
}
