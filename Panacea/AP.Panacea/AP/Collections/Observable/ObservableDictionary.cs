using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP.Collections.ObjectModel;
using AP.ComponentModel.Observable;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace AP.Collections.Observable
{
    public class ObservableDictionary<TKey, TValue> : 
        ExtendableDictionary<TKey, TValue>, INotifyDictionaryChanged<TKey, TValue>, INotifyDictionaryChanging<TKey, TValue>, System.Collections.Specialized.INotifyCollectionChanged, INotifyPropertyChanging, INotifyPropertyChanged
    {
        private const string CountString = "Count";
        private const string IndexerName = "Item[]";

        public ObservableDictionary()
            : base()
        { }

        public ObservableDictionary(int capacity)
            : base(capacity)
        { }

        public ObservableDictionary(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : base(keyComparer, valueComparer)
        { }

        public ObservableDictionary(int capacity, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : base(capacity, keyComparer, valueComparer)
        { }
        
        public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            : base(dictionary)
        { }
        
        public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : base(dictionary, keyComparer, valueComparer)
        { }
        
        protected ObservableDictionary(Dictionary<TKey, TValue> inner)
            : base(inner)
        { }

        public new ObservableDictionary<TKey, TValue> Clone()
        {
            return (ObservableDictionary<TKey, TValue>)this.OnClone();
        }

        protected override CollectionBase<KeyValuePair<TKey, TValue>> OnClone()
        {
            return new ObservableDictionary<TKey, TValue>(this, this.KeyComparer, this.ValueComparer);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override bool Add(TKey key, TValue value)
        {
            return this.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override bool Add(KeyValuePair<TKey, TValue> item)
        {
            var newItems = New.Dictionary(item);

            if (this.ContainsKey(item.Key))
                return false;
                        
            if (this.OnChanging(DictionaryChangingEventArgs<TKey, TValue>.Add(this, newItems)))
                return false;

            base.Add(item);

            this.OnChanged(DictionaryChangedEventArgs<TKey, TValue>.Add(this, newItems));
            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Add(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            var addedItems = new Dictionary<TKey, TValue>();

            foreach (var item in items)
                if (!this.ContainsKey(item.Key))
                    addedItems.Add(item);

            if (addedItems.Count == 0)
                return;

            if (this.OnChanging(DictionaryChangingEventArgs<TKey, TValue>.Add(this, addedItems)))
                return;
            
            this.Inner.Add(addedItems);
            this.OnChanged(DictionaryChangedEventArgs<TKey, TValue>.Add(this, addedItems));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override bool Remove(TKey key)
        {
            TValue v;
            if (!this.Contains(key, out v))
                return false;

            var removedItems = new Dictionary<TKey, TValue> { { key, v } };

            if (this.OnChanging(DictionaryChangingEventArgs<TKey, TValue>.Remove(this, removedItems)))
                return false;

            this.Inner.Remove(key);

            this.OnChanged(DictionaryChangedEventArgs<TKey, TValue>.Remove(this, removedItems));

            return true;
            
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override bool Remove(KeyValuePair<TKey, TValue> item, bool compareValues = false)
        {
            TValue v;
            TKey key = item.Key;

            if (!this.Contains(key, out v))
                return false;
                        
            if (!compareValues || this.ValueComparer.Equals(v, item.Value))
            {
                var removedItems = New.Dictionary(item);

                if (this.OnChanging(DictionaryChangingEventArgs<TKey, TValue>.Remove(this, removedItems)))
                    return false;

                // key was already certified -> return true
                this.Inner.Remove(key);
                this.OnChanged(DictionaryChangedEventArgs<TKey, TValue>.Remove(this, removedItems));

                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Remove(IEnumerable<KeyValuePair<TKey, TValue>> items, bool compareValues = false)
        {
            var removedItems = new Dictionary<TKey, TValue>();

            foreach (var current in this)
            {
                TKey key = current.Key;

                foreach (var item in items)
                {
                    if (this.KeyComparer.Equals(key, item.Key) && (!compareValues || this.ValueComparer.Equals(current.Value, item.Value)))
                        removedItems.Add(current);
                }
            }

            if (removedItems.Count == 0)
                return;

            if (this.OnChanging(DictionaryChangingEventArgs<TKey, TValue>.Remove(this, removedItems)))
                return;

            foreach (TKey key in removedItems.Keys)
                this.Inner.Remove(key);

            this.OnChanged(DictionaryChangedEventArgs<TKey, TValue>.Remove(this, removedItems));            
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Remove(IEnumerable<TKey> keys)
        {
            var removedItems = new Dictionary<TKey, TValue>();

            foreach (TKey key in keys)
            {
                if (this.ContainsKey(key))
                    removedItems.Add(key, this[key]);
            }

            if (removedItems.Count == 0)
                return;

            if (this.OnChanging(DictionaryChangingEventArgs<TKey, TValue>.Remove(this, removedItems)))
                return;

            this.Inner.Remove(keys);

            this.OnChanged(DictionaryChangedEventArgs<TKey, TValue>.Remove(this, removedItems));            
        }
        
        public override TValue this[TKey key]
        {
            get
            {
                return base[key];
            }
            set
            {
                this.Update(key, value);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override bool Update(TKey key, TValue value)
        {
            return this.Update(new KeyValuePair<TKey, TValue>(key, value));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override bool Update(KeyValuePair<TKey, TValue> item)
        {
            TKey key = item.Key;
            TValue value;            

            if (!this.Contains(item.Key, out value))
                return false;

            // if there's no change, there's no need to update it
            if (this.ValueComparer.Equals(value, item.Value))
                return false;

            var original = new Dictionary<TKey, TValue> { { key, value } };
            var updated = New.Dictionary(item);

            if (this.OnChanging(DictionaryChangingEventArgs<TKey, TValue>.Update(this, original, updated)))
                return false;

            this.Inner[key] = item.Value;

            this.OnChanged(DictionaryChangedEventArgs<TKey, TValue>.Update(this, original, updated));

            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Update(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            var originals = new Dictionary<TKey, TValue>();
            var updated = new Dictionary<TKey, TValue>();
            
            foreach (var current in this)
            {
                TKey key = current.Key;

                foreach (var item in items)
                {
                    if (this.KeyComparer.Equals(key, item.Key) && !this.ValueComparer.Equals(current.Value, item.Value))
                    {
                        originals.Add(current);
                        updated.Add(item);
                    }
                }
            }

            if (updated.Count == 0)
                return;

            if (this.OnChanging(DictionaryChangingEventArgs<TKey, TValue>.Update(this, originals, updated)))
                return;

            foreach (var item in updated)
                this.Inner[item.Key] = item.Value;

            this.OnChanged(DictionaryChangedEventArgs<TKey, TValue>.Update(this, originals, updated));            
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Clear()
        {
            var cleared = this.Inner.Clone();

            if (this.OnChanging(DictionaryChangingEventArgs<TKey, TValue>.Clear(this, cleared)))
                return;

            this.Inner.Clear();
            this.OnChanged(DictionaryChangedEventArgs<TKey, TValue>.Clear(this, cleared));
        }

        protected virtual void OnChanged(DictionaryChangedEventArgs<TKey, TValue> args)
        {
            var changed = this.Changed;

            if (changed != null)
                changed(this, args);

            var propertyChanged = this.PropertyChanged;

            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(IndexerName));
                propertyChanged(this, new PropertyChangedEventArgs(CountString));
            }            

            NotifyCollectionChangedEventHandler handler = this.CollectionChanged;

            if (handler != null)
                handler(this, (NotifyCollectionChangedEventArgs)args);
        }

        protected bool OnChanging(DictionaryChangingEventArgs<TKey, TValue> e)
        {
            bool cancel;

            this.OnChanging(e, out cancel);

            return cancel;
        }

        protected virtual void OnChanging(DictionaryChangingEventArgs<TKey, TValue> e, out bool cancel)
        {
            var changing = this.Changing;
            var propertyChanging = this.PropertyChanging;

            if (propertyChanging != null)
            {
                propertyChanging(this, new PropertyChangingEventArgs(IndexerName));
                propertyChanging(this, new PropertyChangingEventArgs(CountString));
            }

            if (changing != null)
                changing(this, e);

            cancel = e.Cancel;
        }

        #region INotifyDictionaryChanged<TKey,TValue> Members

        public event DictionaryChangedEventHandler<TKey, TValue> Changed;

        #endregion

        #region INotifyCollectionChanged<KeyValuePair<TKey,TValue>> Members

        event CollectionChangedEventHandler<KeyValuePair<TKey, TValue>> INotifyCollectionChanged<KeyValuePair<TKey, TValue>>.Changed
        {
            add { this.Changed += new DictionaryChangedEventHandler<TKey, TValue>(value); }
            remove { this.Changed -= new DictionaryChangedEventHandler<TKey, TValue>(value); }
        }

        #endregion

        #region INotifyDictionaryChanging<TKey,TValue> Members

        public event DictionaryChangingEventHandler<TKey, TValue> Changing;

        #endregion

        #region INotifyCollectionChanging<KeyValuePair<TKey,TValue>> Members

        event CollectionChangingEventHandler<KeyValuePair<TKey, TValue>> INotifyCollectionChanging<KeyValuePair<TKey, TValue>>.Changing
        {
            add { this.Changing += new DictionaryChangingEventHandler<TKey, TValue>(value); }
            remove { this.Changing -= new DictionaryChangingEventHandler<TKey, TValue>(value); }
        }

        #endregion

        #region INotifyCollectionChanged Members

        protected event NotifyCollectionChangedEventHandler CollectionChanged;

        event System.Collections.Specialized.NotifyCollectionChangedEventHandler System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged
        {
            add { CollectionChanged += value; }
            remove { CollectionChanged -= value; }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
