using AP.Collections;
using AP.Collections.ReadOnly;

namespace AP.Observable.Collections;

public class SetChangingEventArgs<T> : CollectionChangingEventArgs<T>
{
    public new SetChangeType Type => (SetChangeType)base.Type;
    public new ISetView<T> Source => (ISetView<T>)base.Source;

    public SetChangingEventArgs(ISet<T> source, ISetView<T> newItems, ISetView<T> oldItems, SetChangeType type = SetChangeType.Add)
        : base(source, newItems, oldItems, (ChangeType)type)
    { }

    public static SetChangingEventArgs<T> Add(ISet<T> source, ISetView<T> addedItems) => Union(source, addedItems);

    public static SetChangingEventArgs<T> Union(ISet<T> source, ISetView<T> addedItems) => new(source, addedItems, ReadOnlySet<T>.Empty, SetChangeType.Union);

    public static SetChangingEventArgs<T> Clear(ISet<T> source, ISetView<T> clearedItems) => new(source, ReadOnlySet<T>.Empty, clearedItems, SetChangeType.Clear);

    public static SetChangingEventArgs<T> Remove(ISet<T> source, ISetView<T> removedItems) => Except(source, removedItems);

    public static SetChangingEventArgs<T> Except(ISet<T> source, ISetView<T> removedItems) => new(source, ReadOnlySet<T>.Empty, removedItems, SetChangeType.Except);

    public static SetChangingEventArgs<T> Intersect(ISet<T> source, ISetView<T> removedItems) => new(source, removedItems, ReadOnlySet<T>.Empty, SetChangeType.Intersect);

    public static SetChangingEventArgs<T> SymmetricExcept(ISet<T> source, ISetView<T> addedItems, ISetView<T> removedItems) => new(source, addedItems, removedItems, SetChangeType.SymmetricExcept);
}
