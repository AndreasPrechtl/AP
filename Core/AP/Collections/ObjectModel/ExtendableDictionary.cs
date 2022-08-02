using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace AP.Collections.ObjectModel
{
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false)]
    public abstract partial class ExtendableDictionary<TKey, TValue> : DictionaryBase<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _inner;

        protected Dictionary<TKey, TValue> Inner { get { return _inner; } }
        
        protected ExtendableDictionary()
            : this(0, null, null)
        { }

        protected ExtendableDictionary(int capacity)
            : this(capacity, null, null)
        { }

        protected ExtendableDictionary(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : this(0, keyComparer, valueComparer)
        { }

        protected ExtendableDictionary(int capacity, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : this(new Dictionary<TKey, TValue>(capacity, keyComparer, valueComparer))
        { }
        
        protected ExtendableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            : this(dictionary, null, null)
        { }
        
        protected ExtendableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : this(new Dictionary<TKey, TValue>(dictionary, keyComparer, valueComparer))
        { }
        
        protected ExtendableDictionary(Dictionary<TKey, TValue> inner)
        {
            if (inner == null)
                throw new ArgumentNullException("inner");

            _inner = inner;
        }
        
        public new KeyCollection Keys { get { return (KeyCollection)base.Keys; } }
        public new ValueCollection Values { get { return (ValueCollection)base.Values; } }

        public IEqualityComparer<TKey> KeyComparer { get { return _inner.KeyComparer; } }
        public IEqualityComparer<TValue> ValueComparer { get { return _inner.ValueComparer; } }
        
        public new ExtendableDictionary<TKey, TValue> Clone()
        {
            return (ExtendableDictionary<TKey, TValue>)this.OnClone();
        }

        public override string ToString()
        {
            return _inner.ToString();
        }

        public override bool Add(TKey key, TValue value)
        {
            return _inner.Add(key, value);
        }

        public override void Add(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            _inner.Add(items);
        }

        public override bool Add(KeyValuePair<TKey, TValue> item)
        {
            return _inner.Add(item);
        }

        public override bool Update(KeyValuePair<TKey, TValue> item)
        {
            return _inner.Update(item);
        }

        public override void Update(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            _inner.Update(items);
        }

        public override void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex = 0)
        {
            _inner.CopyTo(array, arrayIndex);
        }

        public override void Remove(IEnumerable<KeyValuePair<TKey, TValue>> items, bool compareValues = false)
        {
            _inner.Remove(items, compareValues);
        }

        public override void Remove(IEnumerable<TKey> keys)
        {
            _inner.Remove(keys);
        }

        public override bool Remove(KeyValuePair<TKey, TValue> item, bool compareValues = false)
        {
            return _inner.Remove(item, compareValues);
        }

        public override void Clear()
        {
            _inner.Clear();
        }

        public override bool Contains(KeyValuePair<TKey, TValue> item, bool compareValues = false)
        {
            return _inner.Contains(item, compareValues);
        }

        public override bool Contains(TKey key, out TValue value)
        {
            return _inner.Contains(key, out value);
        }
        
        public override bool ContainsKey(TKey key)
        {
            return _inner.ContainsKey(key);
        }

        public override bool ContainsValue(TValue value)
        {
            return _inner.ContainsValue(value);
        }

        public override int Count
        {
            get { return _inner.Count; }
        }

        public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }
               
        public override bool Remove(TKey key)
        {
            return _inner.Remove(key);
        }

        public override TValue this[TKey key]
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
    }
}
