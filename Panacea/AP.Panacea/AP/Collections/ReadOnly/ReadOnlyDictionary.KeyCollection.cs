using System;
using AP.Collections.ObjectModel;
using SCG = System.Collections.Generic;
using System.Linq;
using SC = System.Collections;
using AP.Collections.ReadOnly;
using System.Collections.Generic;

namespace AP.Collections.ReadOnly
{    
    public partial class ReadOnlyDictionary<TKey, TValue>
    {
        [System.ComponentModel.ReadOnly(true)]
        public sealed class KeyCollection : DictionaryKeyCollection<ReadOnlyDictionary<TKey, TValue>, TKey, TValue>, IEqualityComparerUser<TKey>
        {
            public KeyCollection(ReadOnlyDictionary<TKey, TValue> dictionary)
                : base(dictionary)
            { }
            
            #region IEqualityComparerUser<TKey> Members

            public IEqualityComparer<TKey> Comparer
            {
                get { return base.Dictionary.KeyComparer; }
            }

            #endregion
                       
            public new KeyCollection Clone()
            {
                return (KeyCollection)this.OnClone();
            }
        }
    }
}
