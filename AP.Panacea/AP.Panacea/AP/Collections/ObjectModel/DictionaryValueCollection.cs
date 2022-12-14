using System;
using System.Text;
using System.Linq;
using AP.Collections.ReadOnly;
using System.Collections.Generic;
using System.Collections;

namespace AP.Collections.ObjectModel
{
    [System.ComponentModel.ReadOnly(true)]
    public abstract class DictionaryValueCollection<TDictionary, TKey, TValue> : CollectionBase<TValue>, System.Collections.Generic.ICollection<TValue>
        where TDictionary : IDictionaryView<TKey, TValue>
    {
        private readonly TDictionary _dictionary;
        
        protected DictionaryValueCollection(TDictionary dictionary)
        {
            _dictionary = dictionary;
        }
        
        public TDictionary Dictionary { get { return _dictionary; } }      
        
        #region ICollection<TValue> Members

        public sealed override int Count
        {
            get { return _dictionary.Count; }
        }

        public sealed override void CopyTo(TValue[] array, int arrayIndex = 0)
        {
            base.CopyTo(array, arrayIndex);
        }

        public sealed override bool Contains(TValue value)
        {
            return _dictionary.ContainsValue(value);
        }

        #endregion

        #region IEnumerable<TValue> Members

        public sealed override IEnumerator<TValue> GetEnumerator()
        {
            foreach (var item in _dictionary)
                yield return item.Value;
        }

        #endregion

        #region ICloneable Members
        
        protected sealed override CollectionBase<TValue> OnClone()
        {
            return this;
        }

        #endregion
        
        #region ICollection<TValue> Members

        void System.Collections.Generic.ICollection<TValue>.Add(TValue item)
        {
            throw new NotSupportedException();
        }

        void System.Collections.Generic.ICollection<TValue>.Clear()
        {
            throw new NotSupportedException();
        }

        bool System.Collections.Generic.ICollection<TValue>.IsReadOnly
        {
            get { return true; }
        }

        bool System.Collections.Generic.ICollection<TValue>.Remove(TValue item)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
