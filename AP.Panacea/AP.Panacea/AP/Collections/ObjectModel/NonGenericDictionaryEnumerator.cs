using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Collections.ObjectModel
{    
    /// <summary>
    /// Wraps a generic IEnumerator&KeyValuePair&TKey, TValue&& into a non-generic DictionaryEnumerator
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public sealed class NonGenericDictionaryEnumerator<TKey, TValue> : IDictionaryEnumerator, System.IDisposable
    {
        private IEnumerator<KeyValuePair<TKey, TValue>> _inner;

        public NonGenericDictionaryEnumerator(IDictionaryView<TKey, TValue> dictionary)
        {
            _inner = dictionary.GetEnumerator();
        }

        #region IDictionaryEnumerator Members

        public DictionaryEntry Entry
        {
            get { return new DictionaryEntry(_inner.Current.Key, _inner.Current.Value); }
        }

        public object Key
        {
            get { return _inner.Current.Key; }
        }

        public object Value
        {
            get { return _inner.Current.Value; }
        }

        #endregion

        #region IEnumerator Members

        public object Current
        {
            get { return _inner.Current; }
        }

        public bool MoveNext()
        {
            return _inner.MoveNext();
        }

        public void Reset()
        {
            _inner.Reset();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            IEnumerator<KeyValuePair<TKey, TValue>> inner = _inner;

            if (inner != null)
            {
                inner.Dispose();
                _inner = null;
            }
        }

        #endregion
    }
}
