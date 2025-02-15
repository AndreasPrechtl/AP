﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using AP.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace AP.Observable.Collections;

public class ObservableQueue<T> : ExtendableQueue<T>, INotifyQueueChanged<T>, INotifyQueueChanging<T>, INotifyCollectionChanged, INotifyPropertyChanging, INotifyPropertyChanged
{
    private const string CountString = "Count";

    public ObservableQueue()
        : base()
    { }

    public ObservableQueue(int capacity)
        : base(capacity)
    { }

    public ObservableQueue(int capacity, IEqualityComparer<T> comparer)
        : base(capacity, comparer)
    { }

    public ObservableQueue(IEqualityComparer<T> comparer)
        : base(comparer)
    { }

    public ObservableQueue(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        : base(collection, comparer)
    { }

    public ObservableQueue(IEnumerable<T> collection)
        : base(collection)
    { }

    protected ObservableQueue(Queue<T> inner)
        : base(inner)
    { }

    public new ObservableQueue<T> Clone() => (ObservableQueue<T>)OnClone();

    protected override CollectionBase<T> OnClone() => new ObservableQueue<T>(this, this.Comparer);

    public sealed override T Dequeue() => Dequeue(1).First();

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override IEnumerable<T> Dequeue(int count)
    {
        var list = new AP.Collections.List<T>(this.Take(count));

        if (OnChanging(QueueChangingEventArgs<T>.Dequeue(this, list)))
            return [];

        var dequeued = base.Dequeue(count);
        OnChanged(QueueChangedEventArgs<T>.Dequeue(this, list));

        return dequeued;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override void Enqueue(T item)
    {
        var list = new AP.Collections.List<T> { item };

        if (OnChanging(QueueChangingEventArgs<T>.Enqueue(this, list)))
            return;

        base.Inner.Enqueue(item);
        OnChanged(QueueChangedEventArgs<T>.Enqueue(this, list));
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override void Enqueue(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        var list = new AP.Collections.List<T>(items);

        if (list.Count < 1)
            return;

        if (OnChanging(QueueChangingEventArgs<T>.Enqueue(this, list)))
            return;

        foreach (T item in items)
        {
            list.Add(item);
            base.Inner.Enqueue(item);
        }

        OnChanged(QueueChangedEventArgs<T>.Enqueue(this, list));
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public override void Clear()
    {
        if (this.Count < 1)
            return;

        var list = new AP.Collections.List<T>(this);

        var a = QueueChangingEventArgs<T>.Clear(this, list);
        OnChanging(a);
        if (a.Cancel)
            return;

        base.Clear();
        OnChanged(QueueChangedEventArgs<T>.Clear(this, list));
    }

    protected virtual void OnChanged(QueueChangedEventArgs<T> e)
    {
        Changed?.Invoke(this, e);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(CountString));
        CollectionChanged?.Invoke(this, (NotifyCollectionChangedEventArgs)e);
    }

    protected bool OnChanging(QueueChangingEventArgs<T> e)
    {
        OnChanging(e, out bool cancel);

        return cancel;
    }

    protected virtual void OnChanging(QueueChangingEventArgs<T> e, out bool cancel)
    {   
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(CountString));
        Changing?.Invoke(this, e);

        cancel = e.Cancel;
    }

    #region INotifyQueueChanging<T> Members

    public event QueueChangingEventHandler<T>? Changing;

    #endregion

    #region INotifyCollectionChanging<T> Members

    event CollectionChangingEventHandler<T>? INotifyCollectionChanging<T>.Changing
    {
        add => Changing += new QueueChangingEventHandler<T>(value!);
        remove => Changing -= new QueueChangingEventHandler<T>(value!);
    }

    #endregion

    #region INotifyQueueChanged<T> Members

    public event QueueChangedEventHandler<T>? Changed;

    #endregion

    #region INotifyCollectionChanged<T> Members

    event CollectionChangedEventHandler<T>? INotifyCollectionChanged<T>.Changed
    {
        add => Changed += new QueueChangedEventHandler<T>(value!);
        remove => Changed -= new QueueChangedEventHandler<T>(value!);
    }

    #endregion

    #region INotifyCollectionChanged Members

    protected event NotifyCollectionChangedEventHandler? CollectionChanged;

    event NotifyCollectionChangedEventHandler? INotifyCollectionChanged.CollectionChanged
    {
        add => CollectionChanged += value;
        remove => CollectionChanged -= value;
    }

    #endregion

    #region INotifyPropertyChanging Members

    protected event PropertyChangingEventHandler? PropertyChanging;

    event PropertyChangingEventHandler? INotifyPropertyChanging.PropertyChanging
    {
        add => PropertyChanging += value;
        remove => PropertyChanging -= value;
    }

    #endregion

    #region INotifyPropertyChanged Members

    protected event PropertyChangedEventHandler? PropertyChanged;

    event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
    {
        add => PropertyChanged += value;
        remove => PropertyChanged -= value;
    }

    #endregion
}
