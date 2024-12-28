using AP.Collections;

namespace AP.Observable.Collections;

public class SetChangingEventArgs<T> : CollectionChangingEventArgs<T>
{
    public new SetChangeType Type => (SetChangeType)base.Type;
    public new ISetView<T> Source => (ISetView<T>)base.Source;

    public SetChangingEventArgs(AP.Collections.ISet<T> source, ISetView<T>? newItems = null, ISetView<T>? oldItems = null, SetChangeType type = SetChangeType.Add)
        : base(source, newItems, oldItems, (ChangeType)type)
    { }

    public static SetChangingEventArgs<T> Add(AP.Collections.ISet<T> source, ISetView<T> addedItems) => Union(source, addedItems);

    public static SetChangingEventArgs<T> Union(AP.Collections.ISet<T> source, ISetView<T> addedItems) => new(source, addedItems, null, SetChangeType.Union);

    public static SetChangingEventArgs<T> Clear(AP.Collections.ISet<T> source, ISetView<T> clearedItems) => new(source, null, clearedItems, SetChangeType.Clear);

    public static SetChangingEventArgs<T> Remove(AP.Collections.ISet<T> source, ISetView<T> removedItems) => Except(source, removedItems);

    public static SetChangingEventArgs<T> Except(AP.Collections.ISet<T> source, ISetView<T> removedItems) => new(source, null, removedItems, SetChangeType.Except);

    public static SetChangingEventArgs<T> Intersect(AP.Collections.ISet<T> source, ISetView<T> removedItems) => new(source, removedItems, null, SetChangeType.Intersect);

    public static SetChangingEventArgs<T> SymmetricExcept(AP.Collections.ISet<T> source, ISetView<T> addedItems, ISetView<T> removedItems) => new(source, addedItems, removedItems, SetChangeType.SymmetricExcept);
}
