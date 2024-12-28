using AP.Collections;

namespace AP.Observable.Collections;

public class StackChangingEventArgs<T> : CollectionChangingEventArgs<T>
{
    public new StackChangeType Type => (StackChangeType)base.Type;
    public new IStack<T> Source => (IStack<T>)base.Source;

    protected StackChangingEventArgs(IStack<T> source, ICollection<T>? newItems = null, ICollection<T>? oldItems = null, StackChangeType type = StackChangeType.Push)
        : base(source, newItems, oldItems, (ChangeType)type)
    { }

    public static StackChangingEventArgs<T> Push(IStack<T> source, ICollection<T> newItems) => new(source, newItems, null, StackChangeType.Push);

    public static StackChangingEventArgs<T> Pop(IStack<T> source, ICollection<T> oldItems) => new(source, null, oldItems, StackChangeType.Pop);

    public static StackChangingEventArgs<T> Clear(IStack<T> source, ICollection<T> clearedItems) => new(source, null, clearedItems, StackChangeType.Clear);
}
