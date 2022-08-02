using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Collections.Observable
{
    public class QueueChangingEventArgs<T> : CollectionChangingEventArgs<T>
    {
        public new QueueChangeType Type { get { return (QueueChangeType)base.Type; } }
        public new IQueue<T> Source { get { return (IQueue<T>)base.Source; } }

        public QueueChangingEventArgs(IQueue<T> source, ICollection<T> newItems = null, ICollection<T> oldItems = null, QueueChangeType type = QueueChangeType.Enqueue)
            : base(source, newItems, oldItems, (ChangeType)type)
        { }

        public static QueueChangingEventArgs<T> Enqueue(IQueue<T> source, ICollection<T> newItems)
        {
            return new QueueChangingEventArgs<T>(source, newItems, null, QueueChangeType.Enqueue);
        }

        public static QueueChangingEventArgs<T> Dequeue(IQueue<T> source, ICollection<T> oldItems)
        {
            return new QueueChangingEventArgs<T>(source, null, oldItems, QueueChangeType.Dequeue);
        }

        public static QueueChangingEventArgs<T> Clear(IQueue<T> source, ICollection<T> clearedItems)
        {
            return new QueueChangingEventArgs<T>(source, null, clearedItems, QueueChangeType.Clear);
        }
    }
}
