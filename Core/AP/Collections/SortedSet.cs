using AP.ComponentModel;
using System;
using System.Collections.Generic;

namespace AP.Collections;

[Serializable, Sorted]
public class SortedSet<T> : ISet<T>, IComparerUser<T>
{
    private readonly object SyncRoot = new();
    private readonly System.Collections.Generic.SortedSet<T> _inner;
    private readonly IComparer<T> _comparer;

    private static System.Collections.Generic.SortedSet<T> CreateInnerSet(IEnumerable<T> collection, IComparer<T> comparer)
    {            
        if (collection != null)
        {
            if (comparer != null)
                return new System.Collections.Generic.SortedSet<T>(collection, comparer);

            return new System.Collections.Generic.SortedSet<T>(collection);
        }

        if (comparer != null)
            return new System.Collections.Generic.SortedSet<T>(comparer);

        return [];
    }

    public SortedSet()
        : this(CreateInnerSet(null!, null!))
    { }

    public SortedSet(IEnumerable<T> collection)
        : this(CreateInnerSet(collection, null!))
    { }

    public SortedSet(IComparer<T> comparer)
        : this(CreateInnerSet(null!, comparer))
    { }
    
    public SortedSet(IEnumerable<T> collection, IComparer<T> comparer)
        : this(CreateInnerSet(collection, comparer))
    { }

    protected SortedSet(System.Collections.Generic.SortedSet<T> inner)
    {
        _comparer = inner.Comparer;
        _inner = inner;            
    }

    public static SortedSet<T> Wrap(System.Collections.Generic.SortedSet<T> set) => new(set);

    public static implicit operator SortedSet<T>(System.Collections.Generic.SortedSet<T> sortedSet)
    {
        return Wrap(sortedSet);
    }

    public static implicit operator System.Collections.Generic.SortedSet<T>(SortedSet<T> sortedSet)
    {
        return sortedSet._inner;
    }

    public override string ToString() => _inner.ToString()!;

    #region ISet<T> Members

    public bool Add(T item) => _inner.Add(item);

    public bool Remove(T item) => _inner.Remove(item);

    public void Add(IEnumerable<T> other) => _inner.UnionWith(other);

    public void UnionWith(IEnumerable<T> other) => _inner.UnionWith(other);

    public void IntersectWith(IEnumerable<T> other) => _inner.IntersectWith(other);

    public void ExceptWith(IEnumerable<T> other) => _inner.ExceptWith(other);

    public void Remove(IEnumerable<T> other) => _inner.ExceptWith(other);

    public void SymmetricExceptWith(IEnumerable<T> other) => _inner.SymmetricExceptWith(other);

    #endregion

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

    public bool Contains(T item) => _inner.Contains(item);

    #endregion

    #region IEnumerable<T> Members

    public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();

    #endregion

    #region ICloneable Members

    public SortedSet<T> Clone() => this.OnClone();

    protected virtual SortedSet<T> OnClone() => new(this, _comparer);

    object ICloneable.Clone() => this.Clone();

    #endregion

    #region ICollection<T> Members

    void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex) => CollectionsHelper.CopyTo(this, array, arrayIndex);

    void System.Collections.Generic.ICollection<T>.Add(T item) => ((System.Collections.Generic.ICollection<T>)_inner).Add(item);

    bool System.Collections.Generic.ICollection<T>.IsReadOnly => false;

    public void Clear() => _inner.Clear();

    #endregion

    #region IComparerUser<T> Members

    public IComparer<T> Comparer => _comparer;

    #endregion
}
