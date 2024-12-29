using AP.Collections;
using AP.Collections.ReadOnly;

namespace AP.Observable.Collections;

public class StackChangingEventArgs<T> : CollectionChangingEventArgs<T>
{
    public new StackChangeType Type => (StackChangeType)base.Type;
    public new IStack<T> Source => (IStack<T>)base.Source;

    protected StackChangingEventArgs(IStack<T> source, ICollection<T> newItems, ICollection<T> oldItems, StackChangeType type = StackChangeType.Push)
        : base(source, newItems, oldItems, (ChangeType)type)
    { }

    public static StackChangingEventArgs<T> Push(IStack<T> source, ICollection<T> newItems) => new(source, newItems, ReadOnlyList<T>.Empty, StackChangeType.Push);

    public static StackChangingEventArgs<T> Pop(IStack<T> source, ICollection<T> oldItems) => new(source, ReadOnlyList<T>.Empty, oldItems, StackChangeType.Pop);

    public static StackChangingEventArgs<T> Clear(IStack<T> source, ICollection<T> clearedItems) => new(source, ReadOnlyList<T>.Empty, clearedItems, StackChangeType.Clear);
}
