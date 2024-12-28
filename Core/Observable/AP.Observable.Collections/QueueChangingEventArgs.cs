using AP.Collections;

namespace AP.Observable.Collections;

public class QueueChangingEventArgs<T> : CollectionChangingEventArgs<T>
{
    public new QueueChangeType Type => (QueueChangeType)base.Type;
    public new IQueue<T> Source => (IQueue<T>)base.Source;

    public QueueChangingEventArgs(IQueue<T> source, AP.Collections.ICollection<T>? newItems = null, AP.Collections.ICollection<T>? oldItems = null, QueueChangeType type = QueueChangeType.Enqueue)
        : base(source, newItems, oldItems, (ChangeType)type)
    { }

    public static QueueChangingEventArgs<T> Enqueue(IQueue<T> source, AP.Collections.ICollection<T> newItems) => new(source, newItems, null, QueueChangeType.Enqueue);

    public static QueueChangingEventArgs<T> Dequeue(IQueue<T> source, AP.Collections.ICollection<T> oldItems) => new(source, null, oldItems, QueueChangeType.Dequeue);

    public static QueueChangingEventArgs<T> Clear(IQueue<T> source, AP.Collections.ICollection<T> clearedItems) => new(source, null, clearedItems, QueueChangeType.Clear);
}
