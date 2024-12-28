using System;
using System.Collections.Generic;

namespace AP.Collections.ObjectModel;

public abstract class SetBase<T> : CollectionBase<T>, ISet<T>
{
    #region ISet<T> Members

    public abstract bool Add(T item);
    public abstract bool Remove(T item);

    public void Add(IEnumerable<T> items) => this.UnionWith(items);

    public void Remove(IEnumerable<T> items) => this.ExceptWith(items);

    public virtual void UnionWith(IEnumerable<T> other)
    {
        foreach (T item in other)
            this.Add(item);
    }

    public virtual void IntersectWith(IEnumerable<T> other)
    {
        List<T> list = new(this.Count);

        foreach (T item in other)
            if (this.Contains(item))
                list.Add(item);

        this.Clear();

        foreach (T item in list)
            this.Add(item);
    }

    public virtual void ExceptWith(IEnumerable<T> other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (this.Count != 0)
        {
            if (other == this)
                this.Clear();
            else
            {
                foreach (T item in other)
                    this.Remove(item);
            }
        }
    }

    public virtual void SymmetricExceptWith(IEnumerable<T> other)
    {
        ArgumentNullException.ThrowIfNull(other);

        foreach (T item in other)
            if (!this.Remove(item))
                this.Add(item);
    }

    #endregion

    #region ISetView<T> Members

    public virtual bool IsSubsetOf(IEnumerable<T> other)
    {
        int matches = 0;

        foreach (T x in this)
        {
            foreach (T y in other)
            {
                if (x.Equals(y))
                    ++matches;
            }
        }

        return matches == this.Count;             
    }

    public virtual bool IsSupersetOf(IEnumerable<T> other)
    {            
        foreach (T item in other)
            if (!this.Contains(item))
                return false;

        return true;
    }

    public virtual bool IsProperSupersetOf(IEnumerable<T> other) => this.IsSupersetOf(other);

    public virtual bool IsProperSubsetOf(IEnumerable<T> other) => this.IsProperSubsetOf(other);

    public virtual bool Overlaps(IEnumerable<T> other)
    {
        foreach (T item in other)
            if (this.Contains(item))
                return true;

        return false;
    }

    public virtual bool SetEquals(IEnumerable<T> other)
    {
        foreach (T item in other)
            if (!this.Contains(item))
                return false;

        return true;
    }

    #endregion

    #region ICloneable Members

    public new SetBase<T> Clone() => (SetBase<T>)this.OnClone();

    #endregion

    #region ICollection<T> Members

    void System.Collections.Generic.ICollection<T>.Add(T item) => this.Add(item);

    protected virtual bool IsReadOnly => false;

    bool System.Collections.Generic.ICollection<T>.IsReadOnly => this.IsReadOnly;

    public abstract void Clear();

    void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex) => CollectionsHelper.CopyTo(this, array, arrayIndex);

    #endregion
}