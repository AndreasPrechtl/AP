using System.Collections.Generic;
using System.Collections.Specialized;

namespace AP.Collections.Observable
{
    public delegate void CollectionChangedEventHandler<T>(object sender, CollectionChangedEventArgs<T> e);
    public delegate void ListChangedEventHandler<T>(object sender, ListChangedEventArgs<T> e);
    public delegate void SetChangedEventHandler<T>(object sender, SetChangedEventArgs<T> e);
    public delegate void QueueChangedEventHandler<T>(object sender, QueueChangedEventArgs<T> e);
    public delegate void StackChangedEventHandler<T>(object sender, StackChangedEventArgs<T> e);
    public delegate void DictionaryChangedEventHandler<TKey, TValue>(object sender, DictionaryChangedEventArgs<TKey, TValue> e);

    public interface INotifyCollectionChanged<T>
    {
        event CollectionChangedEventHandler<T> Changed;
    }

    public interface INotifyListChanged<T> : INotifyCollectionChanged<T>
    {
        new event ListChangedEventHandler<T> Changed;
    }

    public interface INotifySetChanged<T> : INotifyCollectionChanged<T>
    {
        new event SetChangedEventHandler<T> Changed;
    }

    public interface INotifyQueueChanged<T> : INotifyCollectionChanged<T>
    {
        new event QueueChangedEventHandler<T> Changed;
    }

    public interface INotifyStackChanged<T> : INotifyCollectionChanged<T>
    {
        new event StackChangedEventHandler<T> Changed;
    }

    public interface INotifyDictionaryChanged<TKey, TValue> : INotifyCollectionChanged<KeyValuePair<TKey, TValue>>
    {
        new event DictionaryChangedEventHandler<TKey, TValue> Changed;
    }
}
