using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using AP.Collections;

namespace AP.Observable.Collections
{
    public class CollectionChangingEventArgs<T> : CancelEventArgs
    {
        private readonly int _count;
        private readonly ChangeType _type;
        private readonly AP.Collections.ICollection<T> _oldItems;
        private readonly AP.Collections.ICollection<T> _newItems;
        private readonly AP.Collections.ICollection<T> _source;

        public AP.Collections.ICollection<T> NewItems { get { return _newItems; } }
        public AP.Collections.ICollection<T> OldItems { get { return _oldItems; } }
        public ChangeType Type { get { return _type; } }
        public int Count { get { return _count; } }
        public AP.Collections.ICollection<T> Source { get { return _source; } }

        public CollectionChangingEventArgs(AP.Collections.ICollection<T> source, AP.Collections.ICollection<T> newItems, AP.Collections.ICollection<T> oldItems = null, ChangeType action = ChangeType.Add)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (oldItems == null || newItems == null)
                throw new ArgumentNullException("items");

            int count = 0;

            // quick and dirty solution to the problem - the enums are equals except for the missing values.
            // and since the ListAction enum provides more possible scenarios, it's ok to use that.
            ListChangeType a = (ListChangeType)action;

            switch (a)
            {
                case ListChangeType.Add:
                case ListChangeType.Insert:
                case ListChangeType.Replace:
                    count = newItems.Count;
                    break;
                case ListChangeType.Remove:
                case ListChangeType.Move:
                    count = oldItems.Count;
                    break;
            }

            if (count < 1)
                throw new ArgumentException("count");

            _source = source;
            _oldItems = oldItems;
            _newItems = newItems;
            _type = action;
            _count = count;
        }
    }

}
