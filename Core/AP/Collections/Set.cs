using System;
using System.Collections.Generic;

namespace AP.Collections;

[Serializable]
public class Set<T> : ISet<T>, IEqualityComparerUser<T>
{
    private readonly object SyncRoot = new();
    private readonly System.Collections.Generic.HashSet<T> _inner;
    private readonly IEqualityComparer<T> _comparer;

    private static System.Collections.Generic.HashSet<T> CreateInnerSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
    {
        if (collection != null)
        {
            if (comparer != null)
                return new System.Collections.Generic.HashSet<T>(collection, comparer);

            return new System.Collections.Generic.HashSet<T>(collection);
        }

        if (comparer != null)
            return new System.Collections.Generic.HashSet<T>(comparer);

        return [];
    }

    public Set()
        : this(CreateInnerSet(null, null))
    { }

    public Set(IEnumerable<T> collection)
        : this(CreateInnerSet(collection, null))
    { }

    public Set(IEqualityComparer<T> comparer)
        : this(CreateInnerSet(null, comparer))
    { }

    public Set(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        : this(CreateInnerSet(collection, comparer))
    { }

    protected Set(System.Collections.Generic.HashSet<T> inner)
    {
        ArgumentNullException.ThrowIfNull(inner);

        _comparer = inner.Comparer;
        _inner = inner;
    }

    public static Set<T> Wrap(HashSet<T> set) => new(set);

    public static implicit operator Set<T>(System.Collections.Generic.HashSet<T> set)
    {
        return Wrap(set);
    }

    public static implicit operator System.Collections.Generic.HashSet<T>(Set<T> set)
    {
        return set._inner;
    }

    public override string ToString() => _inner.ToString();

    #region ISet<T> Members

    public bool Add(T item) => _inner.Add(item);

    public bool Remove(T item) => _inner.Remove(item);

    public void Add(IEnumerable<T> other) => _inner.UnionWith(other);

    public void UnionWith(IEnumerable<T> other) => _inner.UnionWith(other);

    public void IntersectWith(IEnumerable<T> other) => _inner.IntersectWith(other);

    public void ExceptWith(IEnumerable<T> other) => _inner.ExceptWith(other);

    public void SymmetricExceptWith(IEnumerable<T> other) => _inner.SymmetricExceptWith(other);

    public void Remove(IEnumerable<T> other) => _inner.ExceptWith(other);

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

    public void CopyTo(T[] array, int arrayIndex = 0) => _inner.CopyTo(array, arrayIndex);

    public bool Contains(T item) => _inner.Contains(item);

    #endregion

    #region IEnumerable<T> Members

    public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();

    #endregion

    #region ICloneable Members

    public Set<T> Clone() => this.OnClone();

    protected virtual Set<T> OnClone() => new(this, _comparer);

    object ICloneable.Clone() => this.OnClone();

    #endregion

    #region System.Collections.Generic.ICollection<T> Members

    void System.Collections.Generic.ICollection<T>.Add(T item) => ((System.Collections.Generic.ICollection<T>)_inner).Add(item);

    bool System.Collections.Generic.ICollection<T>.IsReadOnly => false;

    public void Clear() => _inner.Clear();

    #endregion

    #region IEqualityComparerUser<T> Members

    public IEqualityComparer<T> Comparer => _comparer;

    #endregion
}
