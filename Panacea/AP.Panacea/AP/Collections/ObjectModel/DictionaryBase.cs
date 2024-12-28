using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections.ObjectModel
{
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false)]
    public abstract partial class DictionaryBase<TKey, TValue> : CollectionBase<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>, IDictionary
    {
        private readonly KeyCollection _keys;
        private readonly ValueCollection _values;
        
        protected DictionaryBase()
            : this(null, null)
        { }

        protected DictionaryBase(KeyCollection keyCollection, ValueCollection valueCollection)
        {            
            _keys = keyCollection ?? new KeyCollection(this);
            _values = valueCollection ?? new ValueCollection(this);
        }

        public KeyCollection Keys { get { return _keys; } }
        public ValueCollection Values { get { return _values; } }

        public override string ToString()
        {
            return CollectionsHelper.ToString(this);
        }
        
        #region IDictionary<TKey,TValue> Members

        public abstract bool Add(TKey key, TValue value);
        public virtual bool Add(KeyValuePair<TKey, TValue> item)
        {
            return this.Add(item.Key, item.Value);
        }

        public virtual void Add(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            foreach (var item in items)
                this.Add(item);
        }

        public abstract bool Remove(KeyValuePair<TKey, TValue> item, bool compareValues = false);

        public virtual bool Update(TKey key, TValue value)
        {
            bool b = this.ContainsKey(key);
            if (!b)
                this[key] = value;

            return b;
        }

        public virtual bool Update(KeyValuePair<TKey, TValue> item)
        {
            return this.Update(item.Key, item.Value);
        }
        
        public virtual void Update(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            foreach (var item in items)
                this.Update(item);
        }

        public virtual void Remove(IEnumerable<TKey> keys)
        {
            foreach (TKey key in keys)
                this.Remove(key);
        }

        public virtual void Remove(IEnumerable<KeyValuePair<TKey, TValue>> items, bool compareValues = false)
        {
            foreach (var item in items)
                this.Remove(item, compareValues);
        }

        public abstract void Clear();

        #endregion

        #region IDictionaryView<TKey,TValue> Members

        ICollection<TKey> IDictionaryView<TKey, TValue>.Keys
        {
            get { return _keys; }
        }

        ICollection<TValue> IDictionaryView<TKey, TValue>.Values
        {
            get { return _values; }
        }
                
        public virtual bool Contains(KeyValuePair<TKey, TValue> item, bool compareValues = false)
        {
            if (compareValues)
            {
                TValue value;                
                return this.Contains(item.Key, out value) && value.Equals(item.Value);
            }

            return this.ContainsKey(item.Key);
        }

        public virtual bool Contains(TKey key, out TValue value)
        {
            foreach (var item in this)
            {
                if (key.Equals(item.Key))
                {
                    value = item.Value;
                    return true;
                }
            }

            value = default(TValue);
            return false;
        }

        public virtual bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null)
            {
                value = default(TValue);
                return false;
            }

            return this.Contains(key, out value);
        }

        public virtual bool ContainsKey(TKey key)
        {
            foreach (var item in this)
                if (key.Equals(item.Key))
                    return true;

            return false;
        }

        public virtual bool ContainsValue(TValue value)
        {
            foreach (var item in this)
                if (value.Equals(item.Value))
                    return true;
            
            return false;
        }
        
        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.Contains(item, true);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region ICloneable Members

        public new DictionaryBase<TKey, TValue> Clone()
        {
            return (DictionaryBase<TKey, TValue>)this.OnClone();
        }

        #endregion
        
        #region IReadOnlyDictionary<TKey,TValue> Members

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get { return _keys; }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get { return _values; }
        }

        #endregion

        #region IDictionary<TKey,TValue> Members

        void System.Collections.Generic.IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            this.Add(key, value);
        }

        System.Collections.Generic.ICollection<TKey> System.Collections.Generic.IDictionary<TKey, TValue>.Keys
        {
            get { return _keys; }
        }

        System.Collections.Generic.ICollection<TValue> System.Collections.Generic.IDictionary<TKey, TValue>.Values
        {
            get { return _values; }
        }

        public abstract bool Remove(TKey key);

        bool System.Collections.Generic.IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
        {
            return this.Contains(key, out value);
        }

        public abstract TValue this[TKey key]
        {
            get;
            set;
        }

        public IEnumerable<TValue> this[IEnumerable<TKey> keys]
        {
            get
            {
                if (keys == null)
                    throw new ArgumentNullException("keys");

                foreach (TKey key in keys)
                    yield return this[key];
            }
        }

        protected virtual bool IsFixedSize
        {
            get { return false; }
        }

        protected virtual bool IsReadOnly
        { 
            get { return false; } 
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        void System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item);
        }

        bool System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.Remove(item, false);
        }

        bool System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.Contains(item, false);
        }
        
        bool System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return this.IsReadOnly; }
        }

        #endregion  

        #region IDictionary Members

        bool IDictionary.Add(object key, object value)
        {
            return CollectionsHelper.IsCompatible<TKey>(key) && CollectionsHelper.IsCompatible<TValue>(value) && this.Add((TKey)key, (TValue)value);
        }

        bool IDictionary.Add(DictionaryEntry item)
        {
            return ((IDictionary)this).Add(item.Key, item.Value);
        }

        void IDictionary.Add(IEnumerable<DictionaryEntry> items)
        {
            this.Add(items.Select(p => new KeyValuePair<TKey, TValue>((TKey)p.Key, (TValue)p.Value)));
        }

        bool IDictionary.Update(object key, object value)
        {
            return CollectionsHelper.IsCompatible<TKey>(key) && CollectionsHelper.IsCompatible<TValue>(value) && this.Update((TKey)key, (TValue)value);
        }

        bool IDictionary.Update(DictionaryEntry item)
        {
            return ((IDictionary)this).Update(item.Key, item.Value);
        }

        void IDictionary.Update(IEnumerable<DictionaryEntry> items)
        {
            this.Update(items.Select(p => new KeyValuePair<TKey, TValue>((TKey)p.Key, (TValue)p.Value)));
        }

        bool IDictionary.Remove(DictionaryEntry item, bool compareValues)
        {
            return CollectionsHelper.IsCompatible<TKey>(item)
                && CollectionsHelper.IsCompatible<TValue>(item.Value)
                && this.Remove(new KeyValuePair<TKey, TValue>((TKey)item.Key, (TValue)item.Value), compareValues);
        }

        void IDictionary.Remove(IEnumerable keys)
        {
            this.Remove((IEnumerable<TKey>)keys);
        }

        void IDictionary.Remove(IEnumerable<DictionaryEntry> items, bool compareValues)
        {
            this.Remove(items.Select(p => new KeyValuePair<TKey, TValue>((TKey)p.Key, (TValue)p.Value)), compareValues);
        }

        #endregion

        #region IDictionaryView Members

        ICollection IDictionaryView.Keys
        {
            get { return this.Keys; }
        }

        ICollection IDictionaryView.Values
        {
            get { return this.Values; }
        }

        bool IDictionaryView.Contains(DictionaryEntry item, bool compareValues)
        {
            return CollectionsHelper.IsCompatible<TKey>(item.Key)
                && CollectionsHelper.IsCompatible<TValue>(item.Value)
                && this.Contains(new KeyValuePair<TKey, TValue>((TKey)item.Key, (TValue)item.Value), compareValues);
        }

        bool IDictionaryView.Contains(object key, out object value)
        {
            if (CollectionsHelper.IsCompatible<TKey>(key))
            {
                TValue tmp;
                if (this.Contains((TKey)key, out tmp))
                {
                    value = tmp;
                    return true;
                }
            }

            value = null;
            return false;
        }

        bool IDictionaryView.ContainsKey(object key)
        {
            return CollectionsHelper.IsCompatible<TKey>(key) && this.ContainsKey((TKey)key);
        }

        bool IDictionaryView.ContainsValue(object value)
        {
            return CollectionsHelper.IsCompatible<TValue>(value) && this.ContainsValue((TValue)value);
        }

        IEnumerable IDictionaryView.this[IEnumerable keys]
        {
            get { return this[(IEnumerable<TKey>)keys]; }
        }

        #endregion

        #region IDictionary Members

        void System.Collections.IDictionary.Add(object key, object value)
        {
            this.Add((TKey)key, (TValue)value);
        }

        bool System.Collections.IDictionary.Contains(object key)
        {
            return CollectionsHelper.IsCompatible<TKey>(key) && this.ContainsKey((TKey)key);
        }

        IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
        {
            return new NonGenericDictionaryEnumerator<TKey, TValue>(this);
        }

        bool System.Collections.IDictionary.IsFixedSize
        {
            get { return this.IsFixedSize; }
        }

        bool System.Collections.IDictionary.IsReadOnly
        {
            get { return this.IsReadOnly; }
        }

        System.Collections.ICollection System.Collections.IDictionary.Keys
        {
            get { return this.Keys; }
        }

        void System.Collections.IDictionary.Remove(object key)
        {
            this.Remove((TKey)key);
        }

        System.Collections.ICollection System.Collections.IDictionary.Values
        {
            get { return this.Values; }
        }

        object System.Collections.IDictionary.this[object key]
        {
            get
            {
                return this[(TKey)key];
            }
            set
            {
                this[(TKey)key] = (TValue)value;
            }
        }

        #endregion
    }
}
