using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Collections;

namespace AP.Observable.Collections
{
    public class QueueChangingEventArgs<T> : CollectionChangingEventArgs<T>
    {
        public new QueueChangeType Type { get { return (QueueChangeType)base.Type; } }
        public new IQueue<T> Source { get { return (IQueue<T>)base.Source; } }

        public QueueChangingEventArgs(IQueue<T> source, AP.Collections.ICollection<T> newItems = null, AP.Collections.ICollection<T> oldItems = null, QueueChangeType type = QueueChangeType.Enqueue)
            : base(source, newItems, oldItems, (ChangeType)type)
        { }

        public static QueueChangingEventArgs<T> Enqueue(IQueue<T> source, AP.Collections.ICollection<T> newItems)
        {
            return new QueueChangingEventArgs<T>(source, newItems, null, QueueChangeType.Enqueue);
        }

        public static QueueChangingEventArgs<T> Dequeue(IQueue<T> source, AP.Collections.ICollection<T> oldItems)
        {
            return new QueueChangingEventArgs<T>(source, null, oldItems, QueueChangeType.Dequeue);
        }

        public static QueueChangingEventArgs<T> Clear(IQueue<T> source, AP.Collections.ICollection<T> clearedItems)
        {
            return new QueueChangingEventArgs<T>(source, null, clearedItems, QueueChangeType.Clear);
        }
    }
}
