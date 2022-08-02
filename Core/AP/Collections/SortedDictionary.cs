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
using AP.ComponentModel;

namespace AP.Collections
{
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false), Sorted(true)]
    public partial class SortedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly System.Collections.Generic.SortedDictionary<TKey, TValue> _inner;

        private readonly IComparer<TKey> _keyComparer;
        private readonly IEqualityComparer<TValue> _valueComparer;

        private readonly KeyCollection _keys;
        private readonly ValueCollection _values;

        private static System.Collections.Generic.SortedDictionary<TKey, TValue> CreateInnerSortedDictionary(int capacity, IComparer<TKey> keyComparer)
        {
            return new System.Collections.Generic.SortedDictionary<TKey, TValue>(new System.Collections.Generic.Dictionary<TKey, TValue>(capacity), keyComparer ?? Comparer<TKey>.Default);
        }

        private static System.Collections.Generic.SortedDictionary<TKey, TValue> CreateInnerSortedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> items, IComparer<TKey> keyComparer)
        {
            System.Collections.Generic.SortedDictionary<TKey, TValue> dict = new System.Collections.Generic.SortedDictionary<TKey, TValue>(keyComparer ?? Comparer<TKey>.Default);

            if (items != null)
                foreach (var item in items)
                    dict.Add(item.Key, item.Value);

            return dict;
        }

        public SortedDictionary()
            : this(0, null, null)
        { }

        public SortedDictionary(int capacity)
            : this(capacity, null, null)
        { }

        public SortedDictionary(IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : this(0, keyComparer, valueComparer)
        { }

        public SortedDictionary(int capacity, IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : this(CreateInnerSortedDictionary(capacity, keyComparer), valueComparer ?? EqualityComparer<TValue>.Default)
        { }

        public SortedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            : this(dictionary, null, null)
        { }

        public SortedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : this(CreateInnerSortedDictionary(dictionary, keyComparer), valueComparer ?? EqualityComparer<TValue>.Default)
        { }

        protected SortedDictionary(System.Collections.Generic.SortedDictionary<TKey, TValue> inner, IEqualityComparer<TValue> valueComparer)
        {
            if (inner == null)
                throw new ArgumentNullException("inner");

            if (valueComparer == null)
                throw new ArgumentNullException("valueComparer");

            _keyComparer = inner.Comparer;
            _valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;
            _inner = inner;
            _keys = new KeyCollection(this);
            _values = new ValueCollection(this);
        }

        public KeyCollection Keys { get { return _keys; } }
        public ValueCollection Values { get { return _values; } }

        public IComparer<TKey> KeyComparer { get { return _keyComparer; } }
        public IEqualityComparer<TValue> ValueComparer { get { return _valueComparer; } }

        public static SortedDictionary<TKey, TValue> Wrap(System.Collections.Generic.SortedDictionary<TKey, TValue> dictionary, IEqualityComparer<TValue> valueComparer = null)
        {
            return new SortedDictionary<TKey, TValue>(dictionary, valueComparer ?? EqualityComparer<TValue>.Default);
        }

        public static implicit operator SortedDictionary<TKey, TValue>(System.Collections.Generic.SortedDictionary<TKey, TValue> dictionary)
        {
            return Wrap(dictionary);
        }

        public static implicit operator System.Collections.Generic.SortedDictionary<TKey, TValue>(SortedDictionary<TKey, TValue> dictionary)
        {
            return dictionary._inner;
        }

        public override string ToString()
        {
            return _inner.ToString();
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

        public void AddOrUpdate(KeyValuePair<TKey, TValue> item)
        {
            this.AddOrUpdate(item.Key, item.Value);
        }

        public void AddOrUpdate(TKey key, TValue value)
        {
            if (_inner.ContainsKey(key))
                _inner[key] = value;
            else
                _inner.Add(key, value);
        }

        public void AddOrUpdate(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            foreach (var item in items)
                this.AddOrUpdate(item);
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

        public void Add(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (KeyValuePair<TKey, TValue> item in items)
                _inner.Add(item.Key, item.Value);
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
                throw new ArgumentNullException("items");

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
                    if (_keyComparer.Compare(key, item.Key) == 0 && (!compareValues || _valueComparer.Equals(current.Value, item.Value)))
                        keys.Add(key);
                }
            }

            foreach (TKey key in keys)
                _inner.Remove(key);
        }

        public void Update(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (KeyValuePair<TKey, TValue> kvp in items)
                this.Update(kvp);
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

            return _inner.TryGetValue(key, out value);
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

        bool System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.Contains(item, false);
        }

        public void Clear()
        {
            _inner.Clear();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _inner.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _inner.Count; }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey, TValue>> Members

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.Contains(item, false);
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

        public SortedDictionary<TKey, TValue> Clone()
        {
            return this.OnClone();
        }

        protected virtual SortedDictionary<TKey, TValue> OnClone()
        {
            return new SortedDictionary<TKey, TValue>(this, _keyComparer, _valueComparer);
        }

        object ICloneable.Clone()
        {
            return this.OnClone();
        }

        #endregion
    }
}
