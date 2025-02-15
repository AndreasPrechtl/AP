﻿using System.Collections.Generic;

namespace AP.Observable.Collections;

public delegate void CollectionChangingEventHandler<T>(object sender, CollectionChangingEventArgs<T> e);
public delegate void ListChangingEventHandler<T>(object sender, ListChangingEventArgs<T> e);
public delegate void SetChangingEventHandler<T>(object sender, SetChangingEventArgs<T> e);
public delegate void QueueChangingEventHandler<T>(object sender, QueueChangingEventArgs<T> e);
public delegate void StackChangingEventHandler<T>(object sender, StackChangingEventArgs<T> e);
public delegate void DictionaryChangingEventHandler<TKey, TValue>(object sender, DictionaryChangingEventArgs<TKey, TValue> e) where TKey : notnull;

public interface INotifyCollectionChanging<T>
{
    event CollectionChangingEventHandler<T>? Changing;
}

public interface INotifyListChanging<T> : INotifyCollectionChanging<T>
{
    new event ListChangingEventHandler<T>? Changing;
}

public interface INotifySetChanging<T> : INotifyCollectionChanging<T>
{
    new event SetChangingEventHandler<T>? Changing;
}

public interface INotifyQueueChanging<T> : INotifyCollectionChanging<T>
{
    new event QueueChangingEventHandler<T>? Changing;
}

public interface INotifyStackChanging<T> : INotifyCollectionChanging<T>
{
    new event StackChangingEventHandler<T>? Changing;
}

public interface INotifyDictionaryChanging<TKey, TValue> : INotifyCollectionChanging<KeyValuePair<TKey, TValue>>
    where TKey : notnull
{
    new event DictionaryChangingEventHandler<TKey, TValue>? Changing;
}
