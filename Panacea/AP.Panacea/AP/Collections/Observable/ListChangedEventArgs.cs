using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace AP.Collections.Observable
{
    public class ListChangedEventArgs<T> : CollectionChangedEventArgs<T>
    {
        private readonly IDictionaryView<int, T> _newItems;
        private readonly IDictionaryView<int, T> _oldItems;

        public new ListChangeType Type { get { return (ListChangeType)base.Type; } }
        public new IListView<T> Source { get { return (IListView<T>)base.Source; } }

        public new IDictionaryView<int, T> NewItems { get { return _newItems; } }
        public new IDictionaryView<int, T> OldItems { get { return _oldItems; } }
        
        protected ListChangedEventArgs(IListView<T> source, IDictionaryView<int, T> newItems = null, IDictionaryView<int, T> oldItems = null, ListChangeType type = ListChangeType.Add)
            : base(source, newItems != null ? newItems.Values : null, oldItems != null ? oldItems.Values : null, (ChangeType)type)
        {
            _newItems = newItems;
            _oldItems = oldItems;
        }

        public static ListChangedEventArgs<T> Add(IListView<T> source, IDictionaryView<int, T> addedItems)
        {
            return new ListChangedEventArgs<T>(source, addedItems, null, ListChangeType.Add);
        }

        public static ListChangedEventArgs<T> Insert(IListView<T> source, IDictionaryView<int, T> insertedItems)
        {
            return new ListChangedEventArgs<T>(source, insertedItems, null, ListChangeType.Insert);
        }

        public static ListChangedEventArgs<T> Remove(IListView<T> source, IDictionaryView<int, T> removedItems)
        {
            return new ListChangedEventArgs<T>(source, null, removedItems, ListChangeType.Remove);
        }

        public static ListChangedEventArgs<T> Move(IListView<T> source, IDictionaryView<int, T> newItems, IDictionaryView<int, T> oldItems)
        {
            return new ListChangedEventArgs<T>(source, newItems, oldItems, ListChangeType.Move);
        }

        public static ListChangedEventArgs<T> Replace(IListView<T> source, IDictionaryView<int, T> newItems, IDictionaryView<int, T> oldItems)
        {
            return new ListChangedEventArgs<T>(source, newItems, oldItems, ListChangeType.Replace);
        }

        public static ListChangedEventArgs<T> Clear(IListView<T> source, IDictionaryView<int, T> clearedItems)
        {
            return new ListChangedEventArgs<T>(source, null, clearedItems, ListChangeType.Clear);
        }

        public override System.Collections.Specialized.NotifyCollectionChangedEventArgs ToNotifyCollectionChangedEventArgs()
        {
            NotifyCollectionChangedEventArgs a;

            switch (this.Type)
            {
                case ListChangeType.Add:
                case ListChangeType.Insert:
                    a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new System.Collections.Generic.List<T>(this.NewItems.Values));
                    break;
                case ListChangeType.Remove:
                    a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,  new System.Collections.Generic.List<T>(this.OldItems.Values));
                    break;
                case ListChangeType.Replace:
                    a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new System.Collections.Generic.List<T>(this.NewItems.Values), new System.Collections.Generic.List<T>(this.OldItems.Values));
                    break;
                case ListChangeType.Move:
                    a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, new System.Collections.Generic.List<T>(this.NewItems.Values), this.NewItems.Keys.First(), this.OldItems.Keys.First());
                    break;
                case ListChangeType.Clear:
                    a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                    break;
                default:
                    a = null;
                    break;
            }

            return a;
        }
    }
}
