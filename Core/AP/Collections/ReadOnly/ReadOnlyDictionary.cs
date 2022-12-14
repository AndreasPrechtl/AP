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
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false), System.ComponentModel.ReadOnly(true)]
    public partial class ReadOnlyDictionary<TKey, TValue> : IDictionaryView<TKey, TValue>
    {
        private readonly AP.Collections.Dictionary<TKey, TValue> _inner;
        private readonly KeyCollection _keys;
        private readonly ValueCollection _values;
        private volatile static ReadOnlyDictionary<TKey, TValue> _empty;

        public static ReadOnlyDictionary<TKey, TValue> Empty
        {
            get
            {
                ReadOnlyDictionary<TKey, TValue> empty = _empty;

                if (empty == null)
                {
                    empty = new ReadOnlyDictionary<TKey, TValue>(new Dictionary<TKey, TValue>(0));
                    _empty = empty;
                }

                return empty;
            }
        }

        private static AP.Collections.Dictionary<TKey, TValue> CreateInner(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            return new Dictionary<TKey, TValue>(collection, keyComparer, valueComparer);
        }

        public ReadOnlyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            : this(dictionary, null, null)
        { }

        public ReadOnlyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : this(CreateInner(dictionary, keyComparer, valueComparer))
        { }

        protected ReadOnlyDictionary(AP.Collections.Dictionary<TKey, TValue> inner)
        {
            if (inner == null)
                throw new ArgumentNullException("inner");

            _inner = inner;
            _keys = new KeyCollection(this);
            _values = new ValueCollection(this);
        }

        public IEqualityComparer<TKey> KeyComparer { get { return _inner.KeyComparer; } }
        public IEqualityComparer<TValue> ValueComparer { get { return _inner.ValueComparer; } }

        public KeyCollection Keys { get { return _keys; } }
        public ValueCollection Values { get { return _values; } }

        public override string ToString()
        {
            return _inner.ToString();
        }
        
        #region IDictionaryView<TKey,TValue> Members

        ICollection<TKey> IDictionaryView<TKey, TValue>.Keys
        {
            get { return _keys; }
        }

        ICollection<TValue> IDictionaryView<TKey, TValue>.Values
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

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)_inner).Contains(item);
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

        public ReadOnlyDictionary<TKey, TValue> Clone()
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
    }
}