using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Collections.Observable
{
    public class ListChangingEventArgs<T> : CollectionChangingEventArgs<T>
    {
        private readonly IDictionaryView<int, T> _newItems;
        private readonly IDictionaryView<int, T> _oldItems;

        public new ListChangeType Type { get { return (ListChangeType)base.Type; } }
        public new IListView<T> Source { get { return (IListView<T>)base.Source; } }

        public new IDictionaryView<int, T> NewItems { get { return _newItems; } }
        public new IDictionaryView<int, T> OldItems { get { return _oldItems; } }
        
        protected ListChangingEventArgs(IListView<T> source, IDictionaryView<int, T> newItems = null, IDictionaryView<int, T> oldItems = null, ListChangeType type = ListChangeType.Add)
            : base(source, newItems != null ? newItems.Values : null, oldItems != null ? oldItems.Values : null, (ChangeType)type)
        {
            _newItems = newItems;
            _oldItems = oldItems;
        }

        public static ListChangingEventArgs<T> Add(IListView<T> source, IDictionaryView<int, T> addedItems)
        {
            return new ListChangingEventArgs<T>(source, addedItems, null, ListChangeType.Add);
        }

        public static ListChangingEventArgs<T> Insert(IListView<T> source, IDictionaryView<int, T> insertedItems)
        {
            return new ListChangingEventArgs<T>(source, insertedItems, null, ListChangeType.Insert);
        }

        public static ListChangingEventArgs<T> Remove(IListView<T> source, IDictionaryView<int, T> removedItems)
        {
            return new ListChangingEventArgs<T>(source, null, removedItems, ListChangeType.Remove);
        }

        public static ListChangingEventArgs<T> Move(IListView<T> source, IDictionaryView<int, T> newItems, IDictionaryView<int, T> oldItems)
        {
            return new ListChangingEventArgs<T>(source, newItems, oldItems, ListChangeType.Move);
        }

        public static ListChangingEventArgs<T> Replace(IListView<T> source, IDictionaryView<int, T> newItems, IDictionaryView<int, T> oldItems)
        {
            return new ListChangingEventArgs<T>(source, newItems, oldItems, ListChangeType.Replace);
        }

        public static ListChangingEventArgs<T> Clear(IListView<T> source, IDictionaryView<int, T> clearedItems)
        {
            return new ListChangingEventArgs<T>(source, null, clearedItems, ListChangeType.Clear);
        }
    }
}
