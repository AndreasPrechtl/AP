using System;
using AP.Linq;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using AP.Reflection;
using AP.Collections;

using System.Collections.Generic;

namespace AP.Collections
{
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false)]
    public partial class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly System.Collections.Generic.Dictionary<TKey, TValue> _inner;

        private readonly IEqualityComparer<TKey> _keyComparer;
        private readonly IEqualityComparer<TValue> _valueComparer;

        private readonly KeyCollection _keys;
        private readonly ValueCollection _values;

        /// <summary>
        /// Wraps an existing <cref="System.Collections.Generic.Dictionary<TKey, TValue>" /> into a new <cref="AP.Collections.Dictionary<TKey, TValue>" />
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="valueComparer"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> Wrap(System.Collections.Generic.Dictionary<TKey, TValue> dictionary, IEqualityComparer<TValue> valueComparer = null)
        {
            return new Dictionary<TKey, TValue>(dictionary, valueComparer ?? EqualityComparer<TValue>.Default);
        }

        private static System.Collections.Generic.Dictionary<TKey, TValue> CreateInnerDictionary(int capacity, IEqualityComparer<TKey> keyComparer)
        {
            keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;

            return new System.Collections.Generic.Dictionary<TKey, TValue>(capacity, keyComparer);
        }

        private static System.Collections.Generic.Dictionary<TKey, TValue> CreateInnerDictionary(IEnumerable<KeyValuePair<TKey, TValue>> items, IEqualityComparer<TKey> keyComparer)
        {
            System.Collections.Generic.Dictionary<TKey, TValue> dict = new System.Collections.Generic.Dictionary<TKey, TValue>(keyComparer ?? EqualityComparer<TKey>.Default);

            if (items != null)
                foreach (var item in items)
                    dict.Add(item.Key, item.Value);

            return dict;
        }

        public Dictionary()
            : this(0, null, null)
        { }

        public Dictionary(int capacity)
            : this(capacity, null, null)
        { }

        public Dictionary(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : this(0, keyComparer, valueComparer)
        { }

        public Dictionary(int capacity, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : this(CreateInnerDictionary(capacity, keyComparer), valueComparer ?? EqualityComparer<TValue>.Default)
        { }

        public Dictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            : this(dictionary, null, null)
        { }

        public Dictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : this(CreateInnerDictionary(dictionary, keyComparer), valueComparer ?? EqualityComparer<TValue>.Default)
        { }

        protected Dictionary(System.Collections.Generic.Dictionary<TKey, TValue> inner, IEqualityComparer<TValue> valueComparer)
        {
            if (inner == null)
                throw new ArgumentNullException("inner");

            if (valueComparer == null)
                throw new ArgumentNullException("valueComparer");

            _keyComparer = inner.Comparer;
            _valueComparer = valueComparer;
            _inner = inner;
            _keys = new KeyCollection(this);
            _values = new ValueCollection(this);
        }

        public KeyCollection Keys { get { return _keys; } }
        public ValueCollection Values { get { return _values; } }

        public IEqualityComparer<TKey> KeyComparer { get { return _keyComparer; } }
        public IEqualityComparer<TValue> ValueComparer { get { return _valueComparer; } }

        public override string ToString()
        {
            return _inner.ToString();
        }

        public static implicit operator Dictionary<TKey, TValue>(System.Collections.Generic.Dictionary<TKey, TValue> dictionary)
        {
            return Wrap(dictionary);
        }

        public static implicit operator System.Collections.Generic.Dictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            return dictionary._inner;
        }

        #region IDictionary<TKey,TValue> Members

        public bool Add(TKey key, TValue value)
        {
            bool b;
            if (!(b = _inner.ContainsKey(key)))
                _inner.Add(key, value);

            return b;
        }

        public bool Add(KeyValuePair<TKey, TValue> item)
        {
            return this.Add(item.Key, item.Value);
        }

        public void Add(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (KeyValuePair<TKey, TValue> item in items)
                _inner.Add(item.Key, item.Value);
        }

        public bool Update(TKey key, TValue value)
        {
            bool b;
            if (b = _inner.ContainsKey(key))
                _inner[key] = value;

            return b;
        }

        public bool Update(KeyValuePair<TKey, TValue> item)
        {
            return this.Update(item.Key, item.Value);
        }

        public void Update(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (KeyValuePair<TKey, TValue> kvp in items)
                this.Update(kvp);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item, bool compareValues = false)
        {
            if (compareValues)
            {
                if (this.Contains(item, compareValues))
                    return this.Remove(item.Key);

                return false;
            }

            return this.Remove(item.Key);
        }

        public void Remove(IEnumerable<TKey> keys)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            foreach (TKey key in keys)
                _inner.Remove(key);
        }

        public void Remove(IEnumerable<KeyValuePair<TKey, TValue>> items, bool compareValues = false)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            List<TKey> keys = new List<TKey>();

            foreach (var current in this)
            {
                TKey key = current.Key;

                foreach (var item in items)
                {
                    if (_keyComparer.Equals(key, item.Key) && (!compareValues || _valueComparer.Equals(current.Value, item.Value)))
                        keys.Add(key);
                }
            }

            foreach (TKey key in keys)
                _inner.Remove(key);
        }

        #endregion

        #region System.Collections.Generic.IDictionary<TKey,TValue> Members

        void System.Collections.Generic.IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            _inner.Add(key, value);
        }

        bool System.Collections.Generic.IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
        {
            return _inner.TryGetValue(key, out value);
        }

        public bool ContainsKey(TKey key)
        {
            return _inner.ContainsKey(key);
        }

        System.Collections.Generic.ICollection<TKey> System.Collections.Generic.IDictionary<TKey, TValue>.Keys
        {
            get { return _inner.Keys; }
        }

        System.Collections.Generic.ICollection<TValue> System.Collections.Generic.IDictionary<TKey, TValue>.Values
        {
            get { return _inner.Values; }
        }

        public bool Remove(TKey key)
        {
            return _inner.Remove(key);
        }

        public bool Contains(TKey key, out TValue value)
        {
            return _inner.TryGetValue(key, out value);
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

        public TValue this[TKey key]
        {
            get
            {
                return _inner[key];
            }
            set
            {
                _inner[key] = value;
            }
        }

        public IEnumerable<TValue> this[IEnumerable<TKey> keys]
        {
            get
            {
                if (keys == null)
                    throw new ArgumentNullException("keys");

                foreach (TKey key in keys)
                    yield return _inner[key];
            }
        }

        #endregion

        #region System.Collections.Generic.ICollection<KeyValuePair<TKey,TValue>> Members

        void System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            ((System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>)_inner).Add(item);
        }

        bool System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        bool System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.Remove(item, false);
        }

        public void Clear()
        {
            _inner.Clear();
        }

        bool System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.Contains(item, false);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>)_inner).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _inner.Count; }
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        #endregion

        #region ICollection<KeyValuePair<TKey, TValue>> Members

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.Contains(item, false);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        #endregion

        #region IDictionaryView<TKey,TValue> Members

        ICollection<TKey> IDictionaryView<TKey, TValue>.Keys
        {
            get { return this.Keys; }
        }

        ICollection<TValue> IDictionaryView<TKey, TValue>.Values
        {
            get { return this.Values; }
        }

        public bool Contains(KeyValuePair<TKey, TValue> keyValuePair, bool compareValues = false)
        {
            if (compareValues)
            {
                TValue value;
                return _inner.TryGetValue(keyValuePair.Key, out value) && _valueComparer.Equals(keyValuePair.Value, value);
            }

            return _inner.ContainsKey(keyValuePair.Key);
        }

        public bool ContainsValue(TValue value)
        {
            foreach (KeyValuePair<TKey, TValue> kvp in _inner)
                if (_valueComparer.Equals(value, kvp.Value))
                    return true;

            return false;
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

        #region ICloneable Members

        public Dictionary<TKey, TValue> Clone()
        {
            return this.OnClone();
        }

        protected virtual Dictionary<TKey, TValue> OnClone()
        {
            return new Dictionary<TKey, TValue>(this, _keyComparer, _valueComparer);
        }

        object ICloneable.Clone()
        {
            return this.OnClone();
        }

        #endregion    
    }
}
