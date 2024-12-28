using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AP.Collections;

[Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false)]
public class List<T> : IUnsortedList<T>, IEqualityComparerUser<T>
{
    private readonly System.Collections.Generic.List<T> _inner;
    private readonly IEqualityComparer<T> _comparer;
    private readonly bool _hasDefaultComparer;

    public List()
        : this(0, null!)
    { }

    public List(IEqualityComparer<T> comparer)
        : this(0, comparer)
    { }

    public List(int capacity)
        : this(capacity, null!)
    { }

    public List(int capacity, IEqualityComparer<T> comparer)
        : this(new System.Collections.Generic.List<T>(capacity), comparer ?? EqualityComparer<T>.Default)
    { }

    public List(IEnumerable<T> collection)
        : this(collection, null!)
    { }

    public List(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        : this(collection == null ? [] : new System.Collections.Generic.List<T>(collection), comparer ?? EqualityComparer<T>.Default)
    { }

    protected List(System.Collections.Generic.List<T> inner, IEqualityComparer<T> comparer)
    {
        ArgumentNullException.ThrowIfNull(inner);

        ArgumentNullException.ThrowIfNull(comparer);

        _inner = inner;
        _comparer = comparer;
        _hasDefaultComparer = comparer == EqualityComparer<T>.Default;
    }

    public static List<T> Wrap(System.Collections.Generic.List<T> list, IEqualityComparer<T>? comparer = null) => new(list, comparer ?? EqualityComparer<T>.Default);

    public static implicit operator AP.Collections.List<T>(System.Collections.Generic.List<T> list)
    {
        return Wrap(list);
    }

    public static implicit operator System.Collections.Generic.List<T>(AP.Collections.List<T> list)
    {
        return list._inner;
    }

    public override string ToString() => _inner.ToString()!;

    #region IList<T> Members

    public void Remove(T item, SelectionMode mode = SelectionMode.First)
    {
        switch (mode)
        {
            case SelectionMode.First:
                if (_hasDefaultComparer)
                {
                    _inner.Remove(item);
                    break;
                }
                else
                {
                    for (int i = 0, c = _inner.Count; i < c; ++i)
                    {
                        if (_comparer.Equals(_inner[i], item))
                        {
                            _inner.RemoveAt(i);
                            break;
                        }
                    }
                }
                break;
            case SelectionMode.Last:
                for (int i = _inner.Count; --i > -1; )
                {
                    if (_comparer.Equals(_inner[i], item))
                    {
                        _inner.RemoveAt(i);
                        break;
                    }
                }
                break;
            case SelectionMode.All:
                for (int i = 0, c = _inner.Count; i < c; ++i)
                {
                    if (_comparer.Equals(_inner[i], item))
                        _inner.RemoveAt(i--);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode));
        }
    }

    public void Remove(int index, int count = 1)
    {
        if (count == 1)
            _inner.RemoveAt(index);
        else
            _inner.RemoveRange(index, count);
    }

    public void Move(int index, int newIndex, int count = 1)
    {
        int listCount = _inner.Count;

        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);

        ArgumentOutOfRangeException.ThrowIfEqual(index, newIndex);

        int maxIndex = _inner.Count - count;

        if (index < 0 || index > maxIndex)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (newIndex < 0 || newIndex > maxIndex)
            throw new ArgumentOutOfRangeException(nameof(newIndex));

        // special case
        if (count == 1)
        {
            T item = _inner[index];
            _inner.Insert(newIndex, item);

            if (index > newIndex)
                index--;

            _inner.RemoveAt(index);

            return;
        }
        else
        {
            T[] array = new T[count];

            for (int i = 0, j = index; i < count; ++i)
                array[i] = _inner[j++];

            _inner.InsertRange(newIndex, array);

            // re-calculate the index - if necessary
            if (index > newIndex)
                index += count;

            _inner.RemoveRange(index, count);
        }
    }

    public int Add(T item)
    {
        _inner.Add(item);
        return _inner.Count - 1;
    }

    public void Add(IEnumerable<T> items) => _inner.AddRange(items);

    public void Insert(int index, T item) => _inner.Insert(index, item);

    public void Insert(int index, IEnumerable<T> items) => _inner.InsertRange(index, items);

    public void Replace(int index, IEnumerable<T> items)
    {
        if (index < 0 || index >= _inner.Count)
            throw new IndexOutOfRangeException("index");

        foreach (T item in items)
        {
            if (index < _inner.Count)
                _inner[index] = item;
            else
                _inner.Add(item);

            ++index;
        }
    }

    public T this[int index]
    {
        get => _inner[index];
        set => _inner[index] = value;
    }

    public IEnumerable<T> this[int index, int count = 1]
    {
        get
        {
            if (index < 0 || index >= _inner.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (index + count > _inner.Count)
                throw new ArgumentOutOfRangeException(nameof(count));

            for (int i = 0; i < count; ++i)
                yield return _inner[index++];
        }
    }


    public void Clear() => _inner.Clear();

    #endregion

    #region IListView<T> Members

    public int IndexOf(T item, SelectionMode mode = SelectionMode.First)
    {
        switch (mode)
        {
            case SelectionMode.First:
                if (_hasDefaultComparer)
                    return _inner.IndexOf(item);
                else
                {
                    for (int i = 0, c = _inner.Count; i < c; ++i)
                    {
                        if (_comparer.Equals(_inner[i], item))
                            return i;
                    }
                }
                break;
            case SelectionMode.Last:
                if (_hasDefaultComparer)
                    return _inner.LastIndexOf(item);
                else
                {
                    for (int i = _inner.Count; --i > -1; )
                    {
                        if (_comparer.Equals(_inner[i], item))
                            return i;
                    }
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode));
        }

        return -1;
    }

    public bool Contains(T item, out int index, SelectionMode mode = SelectionMode.First) => (index = this.IndexOf(item, mode)) > -1;

    public bool Contains(T item) => this.IndexOf(item, SelectionMode.First) > -1;

    public bool TryGetItem(int index, out T item)
    {
        bool b = index > 0 && index < _inner.Count;

        if (b)
            item = _inner[index];
        else
            item = default!;

        return b;
    }

    #endregion

    #region ICollection<T> Members

    public int Count => _inner.Count;

    #endregion

    #region IEnumerable<T> Members

    public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _inner.GetEnumerator();

    #endregion

    #region ICloneable Members

    public List<T> Clone() => this.OnClone();

    protected virtual List<T> OnClone() => new(this, _comparer);

    object ICloneable.Clone() => this.OnClone();

    #endregion

    #region System.Collections.Generic.IList<T> Members

    int System.Collections.Generic.IList<T>.IndexOf(T item) => _inner.IndexOf(item);

    void System.Collections.Generic.IList<T>.RemoveAt(int index) => _inner.RemoveAt(index);

    #endregion

    #region System.Collections.Generic.ICollection<T> Members

    void System.Collections.Generic.ICollection<T>.Add(T item) => this.Add(item);

    bool System.Collections.Generic.ICollection<T>.Remove(T item)
    {
        int index = this.IndexOf(item);

        if (index > -1)
        {
            this.Remove(index, 1);
            return true;
        }

        return false;
    }

    bool System.Collections.Generic.ICollection<T>.Contains(T item) => this.Contains(item);

    bool System.Collections.Generic.ICollection<T>.IsReadOnly => false;

    void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex) => CollectionsHelper.CopyTo(this, array, arrayIndex);

    #endregion

    #region IEqualityComparerUser<T> Members

    public IEqualityComparer<T> Comparer => _comparer;

    #endregion
}
