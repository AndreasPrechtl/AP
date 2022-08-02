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
    public abstract partial class DictionaryBase<TKey, TValue> : CollectionBase<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>
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
    }
}
