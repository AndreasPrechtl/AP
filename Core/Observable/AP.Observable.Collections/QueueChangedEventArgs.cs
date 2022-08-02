using System;
using System.Linq;
using System.Text;
using AP.Collections;

namespace AP.Observable.Collections
{
    public class QueueChangedEventArgs<T> : CollectionChangedEventArgs<T>
    {
        public new QueueChangeType Type { get { return (QueueChangeType)base.Type; } }
        public new IQueue<T> Source { get { return (IQueue<T>)base.Source; } }

        public QueueChangedEventArgs(IQueue<T> source, IListView<T> newItems = null, IListView<T> oldItems = null, QueueChangeType type = QueueChangeType.Enqueue)
            : base(source, newItems, oldItems, (ChangeType)type)
        { }

        public static QueueChangedEventArgs<T> Enqueue(IQueue<T> source, IListView<T> newItems)
        {
            return new QueueChangedEventArgs<T>(source, newItems, null, QueueChangeType.Enqueue);
        }

        public static QueueChangedEventArgs<T> Dequeue(IQueue<T> source, IListView<T> oldItems)
        {
            return new QueueChangedEventArgs<T>(source, null, oldItems, QueueChangeType.Dequeue);
        }

        public static QueueChangedEventArgs<T> Clear(IQueue<T> source, IListView<T> clearedItems)
        {
            return new QueueChangedEventArgs<T>(source, null, clearedItems, QueueChangeType.Clear);
        }
    }
}
