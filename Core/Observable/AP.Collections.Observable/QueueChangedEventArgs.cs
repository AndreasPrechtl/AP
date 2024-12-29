using AP.Collections;
using AP.Collections.ReadOnly;

namespace AP.Observable.Collections;

public class QueueChangedEventArgs<T> : CollectionChangedEventArgs<T>
{
    public new QueueChangeType Type => (QueueChangeType)base.Type;
    public new IQueue<T> Source => (IQueue<T>)base.Source;

    public QueueChangedEventArgs(IQueue<T> source, IListView<T> newItems, IListView<T> oldItems, QueueChangeType type = QueueChangeType.Enqueue)
        : base(source, newItems, oldItems, (ChangeType)type)
    { }

    public static QueueChangedEventArgs<T> Enqueue(IQueue<T> source, IListView<T> newItems) => new(source, newItems, ReadOnlyList<T>.Empty, QueueChangeType.Enqueue);

    public static QueueChangedEventArgs<T> Dequeue(IQueue<T> source, IListView<T> oldItems) => new(source, ReadOnlyList<T>.Empty, oldItems, QueueChangeType.Dequeue);

    public static QueueChangedEventArgs<T> Clear(IQueue<T> source, IListView<T> clearedItems) => new(source, ReadOnlyList<T>.Empty, clearedItems, QueueChangeType.Clear);
}
