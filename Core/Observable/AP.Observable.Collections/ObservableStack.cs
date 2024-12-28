using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using AP.Collections.ObjectModel;

namespace AP.Observable.Collections;

public class ObservableStack<T> : ExtendableStack<T>, INotifyStackChanged<T>, INotifyStackChanging<T>, INotifyCollectionChanged, INotifyPropertyChanging, INotifyPropertyChanged
{
    private const string CountString = "Count";
    private readonly AP.Collections.List<T> _list;

    public ObservableStack()
        : base()
    {
        _list = [];
    }

    public ObservableStack(int capacity)
        : base(capacity)
    {
        _list = new AP.Collections.List<T>(capacity);
    }

    public ObservableStack(int capacity, IEqualityComparer<T> comparer)
        : base(capacity, comparer)
    {
        _list = new AP.Collections.List<T>(capacity, comparer);
    }

    public ObservableStack(IEqualityComparer<T> comparer)
        : base(comparer)
    {
        _list = new AP.Collections.List<T>(comparer);
    }

    public ObservableStack(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        : base(collection, comparer)
    {
        _list = new AP.Collections.List<T>(collection, comparer);
    }

    public ObservableStack(IEnumerable<T> collection)
        : base(collection)
    {
        _list = new (collection);
    }

    protected ObservableStack(AP.Collections.Stack<T> inner)
        : base(inner)
    {
        _list = new AP.Collections.List<T>(inner);
    }

    public new ObservableStack<T> Clone() => (ObservableStack<T>)OnClone();

    protected override CollectionBase<T> OnClone() => new ObservableStack<T>(this, this.Comparer);

    public sealed override T Pop() => Pop(1).First();

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override IEnumerable<T> Pop(int count)
    {
        var list = new AP.Collections.List<T>(_list[_list.Count - count, count]);

        if (OnChanging(StackChangingEventArgs<T>.Pop(this, list)))
            return null;

        var popped = this.Inner.Pop(count);

        _list.Remove(_list.Count - count, count);
        OnChanged(StackChangedEventArgs<T>.Pop(this, list));

        return popped;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override void Push(T item)
    {
        var list = new AP.Collections.List<T> { item };

        if (OnChanging(StackChangingEventArgs<T>.Push(this, list)))
            return;

        base.Inner.Push(item);
        OnChanged(StackChangedEventArgs<T>.Push(this, list));
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override void Push(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        var list = new AP.Collections.List<T>(items);

        if (list.Count < 1)
            return;

        if (OnChanging(StackChangingEventArgs<T>.Push(this, list)))
            return;

        foreach (T item in items)
        {
            list.Add(item);
            base.Inner.Push(item);
        }
        OnChanged(StackChangedEventArgs<T>.Push(this, list));
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override void Clear()
    {
        if (this.Count < 1)
            return;

        var list = new AP.Collections.List<T>(this);

        if (OnChanging(StackChangingEventArgs<T>.Clear(this, list)))
            return;

        base.Clear();
        OnChanged(StackChangedEventArgs<T>.Clear(this, list));
    }

    protected virtual void OnChanged(StackChangedEventArgs<T> e)
    {
        var changed = Changed;

        if (changed != null)
            changed(this, e);

        var propertyChanged = PropertyChanged;

        if (propertyChanged != null)
            propertyChanged(this, new PropertyChangedEventArgs(CountString));

        NotifyCollectionChangedEventHandler handler = CollectionChanged;

        if (handler != null)
            handler(this, (NotifyCollectionChangedEventArgs)e);
    }

    protected bool OnChanging(StackChangingEventArgs<T> e)
    {
        OnChanging(e, out bool cancel);

        return cancel;
    }

    protected virtual void OnChanging(StackChangingEventArgs<T> e, out bool cancel)
    {
        var changing = Changing;
        var propertyChanging = PropertyChanging;

        if (propertyChanging != null)
            propertyChanging(this, new PropertyChangingEventArgs(CountString));

        if (changing != null)
            changing(this, e);

        cancel = e.Cancel;
    }

    #region INotifyStackChanging<T> Members

    public event StackChangingEventHandler<T> Changing;

    #endregion

    #region INotifyCollectionChanging<T> Members

    event CollectionChangingEventHandler<T> INotifyCollectionChanging<T>.Changing
    {
        add => Changing += (StackChangingEventHandler<T>)(object)value;
        remove => Changing -= (StackChangingEventHandler<T>)(object)value;
    }

    #endregion

    #region INotifyStackChanged<T> Members

    public event StackChangedEventHandler<T> Changed;

    #endregion

    #region INotifyCollectionChanged<T> Members

    event CollectionChangedEventHandler<T> INotifyCollectionChanged<T>.Changed
    {
        add => Changed += (StackChangedEventHandler<T>)(object)value;
        remove => Changed -= (StackChangedEventHandler<T>)(object)value;
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

    protected event PropertyChangingEventHandler PropertyChanging;

    event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
    {
        add => PropertyChanging += value;
        remove => PropertyChanging -= value;
    }

    #endregion

    #region INotifyPropertyChanged Members

    protected event PropertyChangedEventHandler PropertyChanged;

    event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
    {
        add => PropertyChanged += value;
        remove => PropertyChanged -= value;
    }

    #endregion
}