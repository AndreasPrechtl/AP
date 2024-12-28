using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using AP.Reflection;
using AP.Collections;

using SCG = System.Collections.Generic;
using SC = System.Collections;
using AP.ComponentModel;

namespace AP.Collections.ObjectModel
{
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false), Sorted(true)]
    public abstract partial class ExtendableSortedDictionary<TKey, TValue> : DictionaryBase<TKey, TValue>
    {
        private readonly AP.Collections.SortedDictionary<TKey, TValue> _inner;

        protected SortedDictionary<TKey, TValue> Inner { get { return _inner; } }

        protected ExtendableSortedDictionary()
            : this(null, null, null)
        { }

        protected ExtendableSortedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            : this(dictionary, null, null)
        { }

        protected ExtendableSortedDictionary(IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : this(null, keyComparer, valueComparer)
        { }

        protected ExtendableSortedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : this(new SortedDictionary<TKey, TValue>(dictionary, keyComparer, valueComparer))
        { }
        
        protected ExtendableSortedDictionary(SortedDictionary<TKey, TValue> inner)
        {
            ExceptionHelper.AssertNotNull(() => inner);

            _inner = inner;
        }

        public new KeyCollection Keys { get { return (KeyCollection)base.Keys; } }
        public new ValueCollection Values { get { return (ValueCollection)base.Values; } }

        #region IDictionary<TKey,TValue> Members

        public override bool ContainsValue(TValue value)
        {
            return _inner.ContainsValue(value);
        }

        #endregion

        #region IDictionary<TKey,TValue> Members

        public override bool Add(TKey key, TValue value)
        {
            return _inner.Add(key, value);
        }

        public override bool ContainsKey(TKey key)
        {
            return _inner.ContainsKey(key);
        }

        public override bool Remove(KeyValuePair<TKey, TValue> item, bool compareValues = false)
        {
            return _inner.Remove(item, compareValues);
        }

        public override bool Remove(TKey key)
        {
            return _inner.Remove(key);
        }

        public override bool Contains(TKey key, out TValue value)
        {
            return _inner.Contains(key, out value);
        }

        public override TValue this[TKey key]
        {
            get { return _inner[key]; }
            set { _inner[key] = value; }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public override bool Add(KeyValuePair<TKey, TValue> item)
        {
            return _inner.Add(item.Key, item.Value);
        }

        public override void Clear()
        {
            _inner.Clear();
        }

        public override void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((SCG.ICollection<KeyValuePair<TKey, TValue>>)_inner).CopyTo(array, arrayIndex);
        }

        public override int Count
        {
            get { return _inner.Count; }
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        #endregion

        public new ExtendableSortedDictionary<TKey, TValue> Clone()
        {
            return (ExtendableSortedDictionary<TKey, TValue>)this.OnClone();
        }

        public override string ToString()
        {
            return _inner.ToString();
        }
        
        public IComparer<TKey> KeyComparer
        {
            get { return _inner.KeyComparer; }
        }

        public IEqualityComparer<TValue> ValueComparer
        {
            get { return _inner.ValueComparer; }
        }        
    }
}
