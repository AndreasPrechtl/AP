using System;
using System.ComponentModel;

namespace AP.Observable.Collections;

public class CollectionChangingEventArgs<T> : CancelEventArgs
{
    private readonly int _count;
    private readonly ChangeType _type;
    private readonly AP.Collections.ICollection<T> _oldItems;
    private readonly AP.Collections.ICollection<T> _newItems;
    private readonly AP.Collections.ICollection<T> _source;

    public AP.Collections.ICollection<T> NewItems => _newItems;
    public AP.Collections.ICollection<T> OldItems => _oldItems;
    public ChangeType Type => _type;
    public int Count => _count;
    public AP.Collections.ICollection<T> Source => _source;

    public CollectionChangingEventArgs(AP.Collections.ICollection<T> source, AP.Collections.ICollection<T> newItems, AP.Collections.ICollection<T> oldItems, ChangeType action = ChangeType.Add)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(newItems);
        ArgumentNullException.ThrowIfNull(oldItems);

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
                count = oldItems.Count;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action));
        }

        if (count < 1)
            ArgumentOutOfRangeException.ThrowIfLessThan(1, count);

        _source = source;
        _oldItems = oldItems;
        _newItems = newItems;
        _type = action;
        _count = count;
    }
}
