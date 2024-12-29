using System;
using System.Collections.Generic;

namespace AP.Collections.ObjectModel;

public abstract class ListBase<T> : CollectionBase<T>, AP.Collections.IList<T>
{
    #region IList<T> Members

    public abstract void Add(params IEnumerable<T> items);

    public virtual void Remove(T item, SelectionMode mode = SelectionMode.First)
    {
        int index = -1;
        
        switch (mode)
        {
            case SelectionMode.First:
            case SelectionMode.Last:                    
                if (this.Contains(item, out index, mode))
                    this.Remove(index, 1);
                break;
            case SelectionMode.All:
                while (this.Contains(item, out index, SelectionMode.First))
                    this.Remove(index, 1);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(mode));
        }            
    }

    public abstract void Remove(int index, int count = 1);

    public virtual IEnumerable<T> this[int index, int count = 1]
    {
        get
        {
            if (index < 0 || index >= this.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (index + count > this.Count)
                throw new ArgumentOutOfRangeException(nameof(count));

            for (int i = 0; i < count; ++i)
                yield return this[index++];
        }
    }

    public virtual void Clear() => this.Remove(0, this.Count);

    #endregion

    #region IListView<T> Members

    public int IndexOf(T item, SelectionMode mode = SelectionMode.First)
    {
        switch (mode)
        {
            case SelectionMode.First:
                {
                    int i = 0;                    
                    foreach (T current in this)
                    {
                        if (object.Equals(current, item))
                            return i;

                        ++i;
                    }
                    return -1;
                }
            case SelectionMode.Last:

                for (int i = this.Count; i --> 0; )
                {
                    if (object.Equals(this[i], item))
                        return i;
                }
                return -1;

            default:                    
                throw new ArgumentOutOfRangeException(nameof(mode));
        }
    }

    public bool Contains(T item, out int index, SelectionMode mode = SelectionMode.First) => (index = this.IndexOf(item, mode)) > -1;

    protected abstract T GetItem(int index);
  
    public bool TryGetItem(int index, out T item)
    {
        bool b = index > 0 && index < this.Count;

        if (b)
            item = this.GetItem(index);
        else
            item = default!;

        return b;
    }

    #endregion

    #region ICollection<T> Members

    public virtual bool Contains(T item, SelectionMode mode = SelectionMode.First) => this.IndexOf(item, mode) > -1;

    #endregion

    #region IReadOnlyList<T> Members

    public T this[int index]
    {
        get { return this.GetItem(index); }
    }

    #endregion

    #region ICloneable Members

    public new ListBase<T> Clone() => (ListBase<T>)this.OnClone();

    #endregion

    #region System.Collections.Generic.ICollection<T> Members

    void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex) => CollectionsHelper.CopyTo(this, array, arrayIndex);

    void System.Collections.Generic.ICollection<T>.Add(T item) => this.Add(item);

    bool System.Collections.Generic.ICollection<T>.Remove(T item)
    {
        int index = -1;

        if (this.Contains(item, out index, SelectionMode.First))
        {
            this.Remove(index);
            return true;
        }

        return false;
    }

    protected virtual bool IsReadOnly => false;

    bool System.Collections.Generic.ICollection<T>.IsReadOnly => this.IsReadOnly;

    bool System.Collections.Generic.ICollection<T>.Contains(T item) => this.Contains(item, SelectionMode.First);

    #endregion          
}