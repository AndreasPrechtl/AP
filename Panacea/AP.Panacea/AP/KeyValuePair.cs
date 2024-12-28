using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCG = System.Collections.Generic;

namespace AP
{
    public struct KeyValuePair<TKey, TValue> : IComparable<KeyValuePair<TKey, TValue>>, IComparable<TKey>
    {
        private readonly TKey _key;
        private readonly TValue _value;

        private volatile static IComparer<TKey> _comparer;
        private static IComparer<TKey> Comparer
        {
            get
            {
                IComparer<TKey> comparer = _comparer;

                if (comparer == null)
                    _comparer = comparer = Comparer<TKey>.Default;
                
                return comparer;
            }
        }

        public KeyValuePair(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }

        public KeyValuePair(SCG.KeyValuePair<TKey, TValue> inner)
            : this(inner.Key, inner.Value)
        { }

        #region IComparable<KeyValuePair<TKey,TValue>> Members

        public int CompareTo(KeyValuePair<TKey, TValue> other)
        {
            return this.CompareTo(other._key);
        }

        #endregion

        #region IEquatable<KeyValuePair<TKey,TValue>> Members

        public bool Equals(KeyValuePair<TKey, TValue> other)
        {
            return this.Equals(other._key);
        }

        #endregion

        #region IComparable Members

        int System.IComparable.CompareTo(object obj)
        {
            if (obj is KeyValuePair<TKey, TValue>)
                return this.CompareTo(((KeyValuePair<TKey, TValue>)obj)._key);

            if (obj is SCG.KeyValuePair<TKey, TValue>)
                return this.CompareTo(((SCG.KeyValuePair<TKey, TValue>)obj).Key);
            
            return Comparer.Compare(_key, obj);
        }

        #endregion

        #region IComparable<TKey> Members

        public int CompareTo(TKey other)
        {
            return Comparer.Compare(_key, other);
        }

        #endregion

        #region IEquatable<TKey> Members

        public bool Equals(TKey other)
        {
            return Comparer.Equals(_key, other);
        }

        #endregion
    }
}
