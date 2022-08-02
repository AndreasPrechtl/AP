using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AP.Collections.Observable
{
    public class CollectionChangingEventArgs<T> : CancelEventArgs
    {
        private readonly int _count;
        private readonly ChangeType _type;
        private readonly ICollection<T> _oldItems;
        private readonly ICollection<T> _newItems;
        private readonly ICollection<T> _source;

        public ICollection<T> NewItems { get { return _newItems; } }
        public ICollection<T> OldItems { get { return _oldItems; } }
        public ChangeType Type { get { return _type; } }
        public int Count { get { return _count; } }
        public ICollection<T> Source { get { return _source; } }

        public CollectionChangingEventArgs(ICollection<T> source, ICollection<T> newItems, ICollection<T> oldItems = null, ChangeType action = ChangeType.Add)
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
