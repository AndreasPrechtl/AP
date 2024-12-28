using System;
using AP.Collections;
using System.Collections.Specialized;

namespace AP.Observable.Collections;

public class CollectionChangedEventArgs<T> : EventArgs
{
    private readonly int _count;
    private readonly ChangeType _type;
    private readonly ICollection<T> _oldItems;
    private readonly ICollection<T> _newItems;
    private readonly ICollection<T> _source;

    public ICollection<T> NewItems => _newItems;
    public ICollection<T> OldItems => _oldItems;
    public ChangeType Type => _type;
    public int Count => _count;
    public ICollection<T> Source => _source;

    public CollectionChangedEventArgs(ICollection<T> source, ICollection<T> newItems, ICollection<T>? oldItems = null, ChangeType action = ChangeType.Add)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (oldItems == null || newItems == null)
            throw new ArgumentNullException("items");

        int count = 0;

        // quick and dirty solution to the problem - the enums are equals except for the missing values.
        // and since the ListAction enum provides more possible scenarios, it's ok to use that.
        ListChangeType a = (ListChangeType)action;

        switch (a)
        {
            case ListChangeType.Add:
            case ListChangeType.Insert:
            case ListChangeType.Replace:
                count = newItems.Count;
                break;
            case ListChangeType.Remove:
            case ListChangeType.Move:
            case ListChangeType.Clear:
                count = oldItems.Count;
                break;
            default:
                break;
        }

        if (count < 1)
            throw new ArgumentException("count");

        _source = source;
        _oldItems = oldItems;
        _newItems = newItems;
        _type = action;
        _count = count;
    }

    public static explicit operator NotifyCollectionChangedEventArgs(CollectionChangedEventArgs<T> args)
    {
        return args.ToNotifyCollectionChangedEventArgs();
    }

    public virtual NotifyCollectionChangedEventArgs ToNotifyCollectionChangedEventArgs()
    {
        NotifyCollectionChangedEventArgs a;

        switch ((ListChangeType)Type)
        {
            case ListChangeType.Add:
            case ListChangeType.Insert:
                a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, NewItems);
                break;
            case ListChangeType.Remove:
                a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, OldItems);
                break;
            case ListChangeType.Replace:
                a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, NewItems, OldItems, -1);
                break;
            case ListChangeType.Move:
                a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, NewItems, NewItems, -1);
                break;
            case ListChangeType.Clear:
                a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                break;
            default:
                a = null;
                break;
        }

        return a;
    }
}
