using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AP.Collections;
using AP.Collections.ObjectModel;

namespace AP.Observable.Collections;

public class ObservableList<T> : ExtendableList<T>, INotifyListChanged<T>, INotifyListChanging<T>, INotifyCollectionChanged, INotifyPropertyChanging, INotifyPropertyChanged
{
    private const string CountString = "Count";
    private const string IndexerName = "Item[]";

    public ObservableList()
        : base()
    { }

    public ObservableList(int capacity)
        : base(capacity)
    { }

    public ObservableList(int capacity, IEqualityComparer<T> comparer)
        : base(capacity, comparer)
    { }

    public ObservableList(IEnumerable<T> collection)
        : base(collection)
    { }

    public ObservableList(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        : base(collection, comparer)
    { }

    public ObservableList(IEqualityComparer<T> comparer)
        : base(comparer)
    { }

    protected ObservableList(AP.Collections.List<T> inner)
        : base(inner)
    { }

    public new ObservableList<T> Clone() => (ObservableList<T>)OnClone();

    protected override CollectionBase<T> OnClone() => new ObservableList<T>(this, this.Comparer);

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override int Add(T item)
    {
        int i = this.Count;
        var newItems = new AP.Collections.SortedDictionary<int, T>(null, this.Comparer) { { i, item } };

        if (OnChanging(ListChangingEventArgs<T>.Add(this, newItems)))
            return -1;

        this.Inner.Insert(i, item);
        OnChanged(ListChangedEventArgs<T>.Add(this, newItems));

        return i;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override void Add(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        int c = this.Count;
        int i = c;
        var newItems = new AP.Collections.SortedDictionary<int, T>(null, this.Comparer);

        foreach (T item in items)
            newItems.Add(i++, item);

        if (newItems.Count < 1)
            return;

        if (OnChanging(ListChangingEventArgs<T>.Add(this, newItems)))
            return;

        this.Inner.Insert(i, items);
        OnChanged(ListChangedEventArgs<T>.Add(this, newItems));
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override void Insert(int index, IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        ArgumentOutOfRangeException.ThrowIfGreaterThan(index, this.Count);

        var newItems = new AP.Collections.SortedDictionary<int, T>(null, this.Comparer);

        int i = index;
        foreach (T item in items)
            newItems.Add(new KeyValuePair<int, T>(i++, item));

        if (newItems.Count == 0)
            return;

        if (OnChanging(ListChangingEventArgs<T>.Add(this, newItems)))
            return;

        foreach (T item in items)
            base.Insert(index++, item);

        OnChanged(ListChangedEventArgs<T>.Insert(this, newItems));
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override void Insert(int index, T item)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(index, this.Count);

        var newItems = new AP.Collections.SortedDictionary<int, T>(1, null, this.Comparer) { { index, item } };

        if (OnChanging(ListChangingEventArgs<T>.Insert(this, newItems)))
            return;

        this.Inner.Insert(index, item);
        OnChanged(ListChangedEventArgs<T>.Insert(this, newItems));
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override void Move(int index, int newIndex, int count = 1)
    {
        int listCount = this.Inner.Count;

        if (index < 0 || index >= listCount)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (newIndex < 0 || newIndex >= listCount)
            throw new ArgumentOutOfRangeException(nameof(newIndex));

        if (count < 0 || index + count >= listCount)
            throw new ArgumentOutOfRangeException(nameof(count));

        ArgumentOutOfRangeException.ThrowIfEqual(index, newIndex);

        var newItems = new AP.Collections.SortedDictionary<int, T>(count, null, this.Comparer);
        var oldItems = new AP.Collections.SortedDictionary<int, T>(count, null, this.Comparer);

        if (index < newIndex && index + count >= newIndex)
            throw new ArgumentOutOfRangeException(nameof(newIndex));

        T[] array = new T[count];

        for (int i = 0, j = index, t = newIndex; i < count; ++i)
        {
            T item = this.Inner[j];

            newItems.Add(t++, item);
            oldItems.Add(j, item);

            array[i] = this.Inner[j++];
        }

        if (OnChanging(ListChangingEventArgs<T>.Move(this, newItems, oldItems)))
            return;

        this.Inner.Insert(newIndex, array);

        // re-calculate the index - if necessary
        if (index > newIndex)
            index += count;

        this.Inner.Remove(index, count);

        OnChanged(ListChangedEventArgs<T>.Move(this, newItems, oldItems));
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override void Remove(int index, int count = 1)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index);

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);

        ArgumentOutOfRangeException.ThrowIfGreaterThan(count, this.Count - index);

        var removedItems = new AP.Collections.SortedDictionary<int, T>(count, null, this.Comparer);

        for (int i = index; i < count; ++i)
            removedItems.Add(i, this.Inner[i]);

        if (OnChanging(ListChangingEventArgs<T>.Remove(this, removedItems)))
            return;

        this.Inner.Remove(index, count);

        OnChanged(ListChangedEventArgs<T>.Remove(this, removedItems));
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override void Remove(T item, SelectionMode mode = SelectionMode.First)
    {
        switch (mode)
        {
            case SelectionMode.First:
            case SelectionMode.Last:
                int index;

                if (this.Contains(item, out index, mode))
                    Remove(index, 1);

                return;

            case SelectionMode.All:

                var removedItems = new AP.Collections.SortedDictionary<int, T>(null, this.Comparer);

                for (int i = 0, c = this.Count; i < c; ++i)
                {
                    if (this.Comparer.Equals(this.Inner[i], item))
                        removedItems.Add(i, item);
                }

                if (OnChanging(ListChangingEventArgs<T>.Remove(this, removedItems)))
                    return;

                int count = 0;
                foreach (int i in removedItems.Keys)
                {
                    this.Inner.Remove(i - count, 1);
                    count++;
                }

                OnChanged(ListChangedEventArgs<T>.Remove(this, removedItems));

                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(mode));
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    protected override void SetItem(int index, T item)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, this.Count);

        T current = this[index];

        // no change
        if (this.Comparer.Equals(current, item))
            return;

        var newItems = new AP.Collections.SortedDictionary<int, T>(1, null, this.Comparer) { { index, item } };
        var oldItems = new AP.Collections.SortedDictionary<int, T>(1, null, this.Comparer) { { index, current } };

        if (OnChanging(ListChangingEventArgs<T>.Replace(this, newItems, oldItems)))
            return;

        this.Inner[index] = item;
        OnChanged(ListChangedEventArgs<T>.Replace(this, newItems, oldItems));
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override void Clear()
    {
        int c = this.Count;

        if (c == 0)
            return;

        var clearedItems = new AP.Collections.SortedDictionary<int, T>(c, null, this.Comparer);

        for (int i = 0; i < c; ++i)
            clearedItems.Add(i, this.Inner[i]);

        if (OnChanging(ListChangingEventArgs<T>.Clear(this, clearedItems)))
            return;

        this.Inner.Clear();
        OnChanged(ListChangedEventArgs<T>.Clear(this, clearedItems));
    }

    protected virtual void OnChanged(ListChangedEventArgs<T> args)
    {
        var changed = Changed;

        if (changed != null)
            changed(this, args);

        var propertyChanged = PropertyChanged;

        if (propertyChanged != null)
        {
            propertyChanged(this, new PropertyChangedEventArgs(IndexerName));
            propertyChanged(this, new PropertyChangedEventArgs(CountString));
        }

        NotifyCollectionChangedEventHandler handler = CollectionChanged;

        if (handler != null)
            handler(this, (NotifyCollectionChangedEventArgs)args);
    }

    protected bool OnChanging(ListChangingEventArgs<T> e)
    {

        OnChanging(e, out bool b);

        return b;
    }

    protected virtual void OnChanging(ListChangingEventArgs<T> e, out bool cancel)
    {
        var changing = Changing;
        var propertyChanging = PropertyChanging;

        if (propertyChanging != null)
        {
            propertyChanging(this, new PropertyChangingEventArgs(IndexerName));
            propertyChanging(this, new PropertyChangingEventArgs(CountString));
        }

        if (changing != null)
            changing(this, e);

        cancel = e.Cancel;
    }

    #region INotifyListChanged<T> Members

    public event ListChangedEventHandler<T> Changed;

    #endregion

    #region INotifyCollectionChanged<KeyValuePair<int,T>,ListChangedEventArgs<T>> Members

    event CollectionChangedEventHandler<T> INotifyCollectionChanged<T>.Changed
    {
        add => Changed += new ListChangedEventHandler<T>(value);
        remove => Changed -= new ListChangedEventHandler<T>(value);
    }

    #endregion

    #region INotifyListChanging<T> Members

    public event ListChangingEventHandler<T> Changing;

    #endregion

    #region INotifyCollectionChanging<KeyValuePair<int,T>> Members

    event CollectionChangingEventHandler<T> INotifyCollectionChanging<T>.Changing
    {
        add => Changing += new ListChangingEventHandler<T>(value);
        remove => Changing -= new ListChangingEventHandler<T>(value);
    }

    #endregion

    #region INotifyCollectionChanged Members

    protected event NotifyCollectionChangedEventHandler CollectionChanged;

    event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
    {
        add => CollectionChanged += value;
        remove => CollectionChanged -= value;
    }

    #endregion

    #region INotifyPropertyChanging Members

    public event PropertyChangingEventHandler PropertyChanging;

    #endregion

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
}
