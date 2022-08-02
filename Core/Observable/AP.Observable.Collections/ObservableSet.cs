using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using AP.Collections;
using AP.Collections.ObjectModel;

namespace AP.Observable.Collections
{
    public class ObservableSet<T> : ExtendableSet<T>, INotifySetChanged<T>, INotifySetChanging<T>, INotifyCollectionChanged, INotifyPropertyChanging, INotifyPropertyChanged
    {
        private const string CountString = "Count";

        public ObservableSet()
            : base()
        { }

        public ObservableSet(IEnumerable<T> collection)
            : base(collection)
        { }

        public ObservableSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : base(collection, comparer)
        { }

        public ObservableSet(IEqualityComparer<T> comparer)
            : base(comparer)
        { }

        protected ObservableSet(Set<T> inner)
            : base(inner)
        { }


        public new ObservableSet<T> Clone()
        {
            return (ObservableSet<T>)OnClone();
        }

        protected override CollectionBase<T> OnClone()
        {
            return new ObservableSet<T>(this, this.Comparer);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override bool Add(T item)
        {
            if (!this.Contains(item))
                return false;

            var tmp = new Set<T>(this.Comparer) { item };

            if (OnChanging(SetChangingEventArgs<T>.Union(this, tmp)))
                return false;

            this.Inner.Add(item);
            OnChanged(SetChangedEventArgs<T>.Union(this, tmp));

            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override bool Remove(T item)
        {
            if (!this.Contains(item))
                return false;

            var tmp = new Set<T>(this.Comparer) { item };

            if (OnChanging(SetChangingEventArgs<T>.Except(this, tmp)))
                return false;

            this.Inner.Remove(item);
            OnChanged(SetChangedEventArgs<T>.Except(this, tmp));

            return true;
        }

        // remove 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void ExceptWith(IEnumerable<T> other)
        {
            Set<T> removedItems = new Set<T>(this.Comparer);

            foreach (T item in other)
            {
                if (this.Contains(item))
                    removedItems.Add(item);
            }

            if (removedItems.Count == 0)
                return;

            if (OnChanging(SetChangingEventArgs<T>.Except(this, removedItems)))
                return;

            this.Inner.ExceptWith(removedItems);

            OnChanged(SetChangedEventArgs<T>.Except(this, removedItems));
        }

        // removes all elements that are not in both sets
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void IntersectWith(IEnumerable<T> other)
        {
            Set<T> removedItems = new Set<T>(this.Comparer);
            Set<T> tmp = new Set<T>(this.Comparer);

            foreach (T item in other)
            {
                if (!this.Contains(item))
                    removedItems.Add(item);
                else
                    tmp.Add(item);
            }

            if (removedItems.Count == 0)
                return;

            if (OnChanging(SetChangingEventArgs<T>.Intersect(this, removedItems)))
                return;

            // clearing and adding should be faster than removing each
            this.Inner.Clear();
            this.Inner.Add(tmp);

            OnChanged(SetChangedEventArgs<T>.Intersect(this, removedItems));
        }

        // adds all items that are in either set, but not shared between the sets
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void SymmetricExceptWith(IEnumerable<T> other)
        {
            Set<T> removedItems = new Set<T>(this.Comparer);
            Set<T> addedItems = new Set<T>(this.Comparer);

            foreach (T item in other)
            {
                if (this.Contains(item))
                    removedItems.Add(item);
                else
                    addedItems.Add(item);
            }

            if (addedItems.Count == 0 || removedItems.Count == 0)
                return;

            if (OnChanging(SetChangingEventArgs<T>.SymmetricExcept(this, addedItems, removedItems)))
                return;

            this.Inner.Remove(removedItems);
            this.Inner.Add(addedItems);

            OnChanged(SetChangedEventArgs<T>.SymmetricExcept(this, addedItems, removedItems));
        }

        // add
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void UnionWith(IEnumerable<T> other)
        {
            var addedItems = new Set<T>(this.Comparer);
            foreach (T item in other)
            {
                if (this.Contains(item))
                    addedItems.Add(item);
            }

            if (addedItems.Count == 0)
                return;

            if (OnChanging(SetChangingEventArgs<T>.Union(this, addedItems)))
                return;

            this.Inner.UnionWith(addedItems);

            OnChanged(SetChangedEventArgs<T>.Union(this, addedItems));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Clear()
        {
            if (this.Count == 0)
                return;

            Set<T> cleared = this.Inner.Clone();

            if (OnChanging(SetChangingEventArgs<T>.Clear(this, cleared)))
                return;

            this.Inner.Clear();
            OnChanged(SetChangedEventArgs<T>.Clear(this, cleared));
        }

        protected virtual void OnChanged(SetChangedEventArgs<T> e)
        {
            var changed = Changed;

            if (changed != null)
                changed(this, e);

            var propertyChanged = PropertyChanged;

            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(CountString));

            NotifyCollectionChangedEventHandler handler = CollectionChanged;

            if (handler != null)
                handler(this, (NotifyCollectionChangedEventArgs)e);
        }

        protected bool OnChanging(SetChangingEventArgs<T> e)
        {
            bool cancel;
            OnChanging(e, out cancel);

            return cancel;
        }

        protected virtual void OnChanging(SetChangingEventArgs<T> e, out bool cancel)
        {
            var changing = Changing;
            var propertyChanging = PropertyChanging;

            if (propertyChanging != null)
                propertyChanging(this, new PropertyChangingEventArgs(CountString));

            if (changing != null)
                changing(this, e);

            cancel = e.Cancel;
        }

        #region INotifySetChanging<T> Members

        public event SetChangingEventHandler<T> Changing;

        #endregion

        #region INotifyCollectionChanging<T> Members

        event CollectionChangingEventHandler<T> INotifyCollectionChanging<T>.Changing
        {
            add { Changing += new SetChangingEventHandler<T>(value); }
            remove { Changing -= new SetChangingEventHandler<T>(value); }
        }

        #endregion

        #region INotifySetChanged<T> Members

        public event SetChangedEventHandler<T> Changed;

        #endregion

        #region INotifyCollectionChanged<T> Members

        event CollectionChangedEventHandler<T> INotifyCollectionChanged<T>.Changed
        {
            add { Changed += (SetChangedEventHandler<T>)(object)value; }
            remove { Changed -= (SetChangedEventHandler<T>)(object)value; }
        }

        #endregion

        #region INotifyCollectionChanged Members

        protected event NotifyCollectionChangedEventHandler CollectionChanged;

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add { CollectionChanged += value; }
            remove { CollectionChanged -= value; }
        }

        #endregion

        #region INotifyPropertyChanging Members

        protected event PropertyChangingEventHandler PropertyChanging;

        event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
        {
            add { PropertyChanging += value; }
            remove { PropertyChanging -= value; }
        }

        #endregion

        #region INotifyPropertyChanged Members

        protected event PropertyChangedEventHandler PropertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { PropertyChanged += value; }
            remove { PropertyChanged -= value; }
        }

        #endregion
    }
}
