using System;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AP.Collections;
using System.Collections;
using System.Collections.Generic;
using AP.ComponentModel;

namespace AP.Collections.ReadOnly
{
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false), System.ComponentModel.ReadOnly(true), Sorted(true)]
    public partial class ReadOnlySortedDictionary<TKey, TValue> : IDictionaryView<TKey, TValue>, IDictionaryView, System.Collections.Generic.IDictionary<TKey, TValue>, System.Collections.IDictionary
    {
        private readonly AP.Collections.SortedDictionary<TKey, TValue> _inner;
        private readonly KeyCollection _keys;
        private readonly ValueCollection _values;
        private volatile static ReadOnlySortedDictionary<TKey, TValue> _empty;

        public static ReadOnlySortedDictionary<TKey, TValue> Empty
        {
            get
            {
                ReadOnlySortedDictionary<TKey, TValue> empty = _empty;

                if (empty == null)
                {
                    empty = new ReadOnlySortedDictionary<TKey, TValue>(new Dictionary<TKey, TValue>(0));
                    _empty = empty;
                }

                return empty;
            }
        }

        private static AP.Collections.SortedDictionary<TKey, TValue> CreateInner(IEnumerable<KeyValuePair<TKey, TValue>> collection, IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            return new SortedDictionary<TKey, TValue>(collection, keyComparer, valueComparer);
        }        

        public ReadOnlySortedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            : this(dictionary, null, null)
        { }

        public ReadOnlySortedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : this(CreateInner(dictionary, keyComparer, valueComparer))
        { }

        protected ReadOnlySortedDictionary(AP.Collections.SortedDictionary<TKey, TValue> inner)
        {
            if (inner == null)
                throw new ArgumentNullException("inner");
            
            _inner = inner;
            _keys = new KeyCollection(this);
            _values = new ValueCollection(this);
        }

        public IComparer<TKey> KeyComparer { get { return _inner.KeyComparer; } }
        public IEqualityComparer<TValue> ValueComparer { get { return _inner.ValueComparer; } }

        public KeyCollection Keys { get { return _keys; } }
        public ValueCollection Values { get { return _values; } }

        public override string ToString()
        {
            return _inner.ToString();
        }
        
        #region IDictionaryView<TKey,TValue> Members

        ICollection<TKey> IDictionaryView<TKey,TValue>.Keys
        {
            get { return _keys; }
        }

        ICollection<TValue> IDictionaryView<TKey,TValue>.Values
        {
            get { return _values; }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item, bool compareValues = false)
        {
            return _inner.Contains(item, compareValues);
        }

        public bool Contains(TKey key, out TValue value)
        {
            return _inner.Contains(key, out value);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null)
            {
                value = default(TValue);
                return false;
            }

            return this.Contains(key, out value);
        }

        public bool ContainsKey(TKey key)
        {
            return _inner.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return _inner.ContainsValue(value);
        }

        public TValue this[TKey key]
        {
            get { return _inner[key]; }
        }

        public IEnumerable<TValue> this[IEnumerable<TKey> keys]
        {
            get { return _inner[keys]; }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public int Count
        {
            get { return _inner.Count; }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex = 0)
        {
            _inner.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey,TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey,TValue>>)_inner).Contains(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region ICloneable Members

        public virtual ReadOnlySortedDictionary<TKey, TValue> Clone()
        {
            return this;
        }
        
        object ICloneable.Clone()
        {
            return this.Clone();
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
            throw new NotSupportedException();
        }

        System.Collections.Generic.ICollection<TKey> System.Collections.Generic.IDictionary<TKey, TValue>.Keys
        {
            get { return this.Keys; }
        }

        bool System.Collections.Generic.IDictionary<TKey, TValue>.Remove(TKey key)
        {
            throw new NotSupportedException();
        }

        System.Collections.Generic.ICollection<TValue> System.Collections.Generic.IDictionary<TKey, TValue>.Values
        {
            get { return this.Values; }
        }

        TValue System.Collections.Generic.IDictionary<TKey, TValue>.this[TKey key]
        {
            get
            {
                return this[key];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        void System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        void System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            throw new NotSupportedException();
        }

        bool System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        void System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex);
        }

        bool System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return true; }
        }

        bool System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IDictionary Members

        void System.Collections.IDictionary.Add(object key, object value)
        {
            throw new NotSupportedException();
        }

        void System.Collections.IDictionary.Clear()
        {
            throw new NotSupportedException();
        }

        bool System.Collections.IDictionary.Contains(object key)
        {
            return CollectionsHelper.IsCompatible<TKey>(key) && this.ContainsKey((TKey)key);
        }

        IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
        {
            return new AP.Collections.ObjectModel.NonGenericDictionaryEnumerator<TKey, TValue>(this);
        }

        bool System.Collections.IDictionary.IsFixedSize
        {
            get { return true; }
        }

        bool System.Collections.IDictionary.IsReadOnly
        {
            get { return true; }
        }

        System.Collections.ICollection System.Collections.IDictionary.Keys
        {
            get { return this.Keys; }
        }

        void System.Collections.IDictionary.Remove(object key)
        {
            throw new NotSupportedException();
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
                throw new NotSupportedException();
            }
        }

        #endregion

        #region ICollection Members

        void System.Collections.ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        bool System.Collections.ICollection.IsSynchronized
        {
            get { return true; }
        }

        object System.Collections.ICollection.SyncRoot
        {
            get { return ((System.Collections.ICollection)_inner).SyncRoot; }
        }

        #endregion

        #region IDictionaryView Members

        ICollection IDictionaryView.Keys
        {
            get { throw new NotImplementedException(); }
        }

        ICollection IDictionaryView.Values
        {
            get { throw new NotImplementedException(); }
        }

        bool IDictionaryView.Contains(DictionaryEntry item, bool compareValues)
        {
            throw new NotImplementedException();
        }

        bool IDictionaryView.Contains(object key, out object value)
        {
            throw new NotImplementedException();
        }

        bool IDictionaryView.ContainsKey(object key)
        {
            throw new NotImplementedException();
        }

        bool IDictionaryView.ContainsValue(object value)
        {
            throw new NotImplementedException();
        }

        IEnumerable IDictionaryView.this[IEnumerable keys]
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
