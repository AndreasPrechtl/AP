﻿using AP.Collections;
using AP.Collections.ReadOnly;

namespace AP.Observable.Collections;

public class SetChangedEventArgs<T> : CollectionChangedEventArgs<T>
{
    public new SetChangeType Type => (SetChangeType)base.Type;
    public new ISetView<T> Source => (ISetView<T>)base.Source;

    public new ISetView<T> OldItems => (ISetView<T>)base.OldItems;
    public new ISetView<T> NewItems => (ISetView<T>)base.NewItems;

    protected SetChangedEventArgs(ISetView<T> source, ISetView<T> newItems, ISetView<T> oldItems, SetChangeType type = SetChangeType.Add)
        : base(source, newItems, oldItems, (ChangeType)type)
    { }

    public static SetChangedEventArgs<T> Add(AP.Collections.ISet<T> source, ISetView<T> addedItems) => Union(source, addedItems);

    public static SetChangedEventArgs<T> Union(AP.Collections.ISet<T> source, ISetView<T> addedItems) => new(source, addedItems, ReadOnlySet<T>.Empty, SetChangeType.Union);

    public static SetChangedEventArgs<T> Clear(AP.Collections.ISet<T> source, ISetView<T> clearedItems) => new(source, ReadOnlySet<T>.Empty, clearedItems, SetChangeType.Clear);

    public static SetChangedEventArgs<T> Remove(AP.Collections.ISet<T> source, ISetView<T> removedItems) => Except(source, removedItems);

    public static SetChangedEventArgs<T> Except(AP.Collections.ISet<T> source, ISetView<T> removedItems) => new(source, ReadOnlySet<T>.Empty, removedItems, SetChangeType.Except);

    public static SetChangedEventArgs<T> Intersect(AP.Collections.ISet<T> source, ISetView<T> removedItems) => new(source, removedItems, ReadOnlySet<T>.Empty, SetChangeType.Intersect);

    public static SetChangedEventArgs<T> SymmetricExcept(AP.Collections.ISet<T> source, ISetView<T> addedItems, ISetView<T> removedItems) => new(source, addedItems, removedItems, SetChangeType.SymmetricExcept);
}
