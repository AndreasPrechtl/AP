using System;
using AP.Collections.ObjectModel;
using SCG = System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;

namespace AP.Collections
{
    public partial class Dictionary<TKey, TValue>
    {
        public sealed class KeyCollection : DictionaryKeyCollection<Dictionary<TKey, TValue>, TKey, TValue>, IEqualityComparerUser<TKey>
        {
            public KeyCollection(Dictionary<TKey, TValue> dictionary)
                : base(dictionary)
            { }

            public new KeyCollection Clone()
            {
                return (KeyCollection)this.OnClone();
            }

            #region IEqualityComparerUser<TKey> Members

            public IEqualityComparer<TKey> Comparer
            {
                get { return base.Dictionary.KeyComparer; }
            }

            #endregion            
        }
    }
}
