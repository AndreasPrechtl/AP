using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Collections.Observable
{
    public class QueueChangedEventArgs<T> : CollectionChangedEventArgs<T>
    {
        public new QueueChangeType Type { get { return (QueueChangeType)base.Type; } }
        public new IQueue<T> Source { get { return (IQueue<T>)base.Source; } }

        public QueueChangedEventArgs(IQueue<T> source, ICollection<T> newItems = null, ICollection<T> oldItems = null, QueueChangeType type = QueueChangeType.Enqueue)
            : base(source, newItems, oldItems, (ChangeType)type)
        { }

        public static QueueChangedEventArgs<T> Enqueue(IQueue<T> source, ICollection<T> newItems)
        {
            return new QueueChangedEventArgs<T>(source, newItems, null, QueueChangeType.Enqueue);
        }

        public static QueueChangedEventArgs<T> Dequeue(IQueue<T> source, ICollection<T> oldItems)
        {
            return new QueueChangedEventArgs<T>(source, null, oldItems, QueueChangeType.Dequeue);
        }

        public static QueueChangedEventArgs<T> Clear(IQueue<T> source, ICollection<T> clearedItems)
        {
            return new QueueChangedEventArgs<T>(source, null, clearedItems, QueueChangeType.Clear);
        }
    }
}
