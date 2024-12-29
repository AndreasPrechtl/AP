using System;
using System.Collections.Specialized;
using System.Linq;
using AP.Collections;
using AP.Collections.ReadOnly;

namespace AP.Observable.Collections;

public class ListChangedEventArgs<T> : CollectionChangedEventArgs<T>
{
    private readonly IDictionaryView<int, T> _newItems;
    private readonly IDictionaryView<int, T> _oldItems;

    public new ListChangeType Type => (ListChangeType)base.Type;
    public new IListView<T> Source => (IListView<T>)base.Source;

    public new IDictionaryView<int, T> NewItems => _newItems;
    public new IDictionaryView<int, T> OldItems => _oldItems;

    protected ListChangedEventArgs(IListView<T> source, IDictionaryView<int, T> newItems, IDictionaryView<int, T> oldItems, ListChangeType type = ListChangeType.Add)
        : base(source, newItems != null ? newItems.Values : ReadOnlyList<T>.Empty, oldItems != null ? oldItems.Values : ReadOnlyList<T>.Empty, (ChangeType)type)
    {
        _newItems = newItems ?? ReadOnlyDictionary<int, T>.Empty;
        _oldItems = oldItems ?? ReadOnlyDictionary<int, T>.Empty;
    }

    public static ListChangedEventArgs<T> Add(IListView<T> source, IDictionaryView<int, T> addedItems) => new(source, addedItems, ReadOnlyDictionary<int, T>.Empty, ListChangeType.Add);

    public static ListChangedEventArgs<T> Insert(IListView<T> source, IDictionaryView<int, T> insertedItems) => new(source, insertedItems, ReadOnlyDictionary<int, T>.Empty, ListChangeType.Insert);

    public static ListChangedEventArgs<T> Remove(IListView<T> source, IDictionaryView<int, T> removedItems) => new(source, ReadOnlyDictionary<int, T>.Empty, removedItems, ListChangeType.Remove);

    public static ListChangedEventArgs<T> Move(IListView<T> source, IDictionaryView<int, T> newItems, IDictionaryView<int, T> oldItems) => new(source, newItems, oldItems, ListChangeType.Move);

    public static ListChangedEventArgs<T> Replace(IListView<T> source, IDictionaryView<int, T> newItems, IDictionaryView<int, T> oldItems) => new(source, newItems, oldItems, ListChangeType.Replace);

    public static ListChangedEventArgs<T> Clear(IListView<T> source, IDictionaryView<int, T> clearedItems) => new(source, ReadOnlyDictionary<int, T>.Empty, clearedItems, ListChangeType.Clear);

    public override NotifyCollectionChangedEventArgs ToNotifyCollectionChangedEventArgs()
    {
        NotifyCollectionChangedEventArgs a;

        switch (Type)
        {
            case ListChangeType.Add:
            case ListChangeType.Insert:
                a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T>(NewItems.Values));
                break;
            case ListChangeType.Remove:
                a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T>(OldItems.Values));
                break;
            case ListChangeType.Replace:
                a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new List<T>(NewItems.Values), new List<T>(OldItems.Values));
                break;
            case ListChangeType.Move:
                a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, new List<T>(NewItems.Values), NewItems.Keys.First(), OldItems.Keys.First());
                break;
            case ListChangeType.Clear:
                a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(Type));
        }

        return a;
    }
}
