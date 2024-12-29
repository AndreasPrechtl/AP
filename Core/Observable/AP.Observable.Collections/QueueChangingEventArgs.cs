using AP.Collections;
using AP.Collections.ReadOnly;

namespace AP.Observable.Collections;

public class QueueChangingEventArgs<T> : CollectionChangingEventArgs<T>
{
    public new QueueChangeType Type => (QueueChangeType)base.Type;
    public new IQueue<T> Source => (IQueue<T>)base.Source;

    public QueueChangingEventArgs(IQueue<T> source, AP.Collections.ICollection<T> newItems, AP.Collections.ICollection<T> oldItems, QueueChangeType type = QueueChangeType.Enqueue)
        : base(source, newItems, oldItems, (ChangeType)type)
    { }

    public static QueueChangingEventArgs<T> Enqueue(IQueue<T> source, AP.Collections.ICollection<T> newItems) => new(source, newItems, ReadOnlyList<T>.Empty, QueueChangeType.Enqueue);

    public static QueueChangingEventArgs<T> Dequeue(IQueue<T> source, AP.Collections.ICollection<T> oldItems) => new(source, ReadOnlyList<T>.Empty, oldItems, QueueChangeType.Dequeue);

    public static QueueChangingEventArgs<T> Clear(IQueue<T> source, AP.Collections.ICollection<T> clearedItems) => new(source, ReadOnlyList<T>.Empty, clearedItems, QueueChangeType.Clear);
}
