using AP.Collections;

namespace AP.Observable.Collections;

public class StackChangedEventArgs<T> : CollectionChangedEventArgs<T>
{
    public new StackChangeType Type => (StackChangeType)base.Type;
    public new IStack<T> Source => (IStack<T>)base.Source;

    protected StackChangedEventArgs(IStack<T> source, IListView<T>? newItems = null, IListView<T>? oldItems = null, StackChangeType type = StackChangeType.Push)
        : base(source, newItems, oldItems, (ChangeType)type)
    { }

    public static StackChangedEventArgs<T> Push(IStack<T> source, IListView<T> newItems) => new(source, newItems, null, StackChangeType.Push);

    public static StackChangedEventArgs<T> Pop(IStack<T> source, IListView<T> oldItems) => new(source, null, oldItems, StackChangeType.Pop);

    public static StackChangedEventArgs<T> Clear(IStack<T> source, IListView<T> clearedItems) => new(source, null, clearedItems, StackChangeType.Clear);
}
