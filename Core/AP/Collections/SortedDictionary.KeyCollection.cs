using System;
using AP.Collections.ObjectModel;
using SCG = System.Collections.Generic;
using System.Linq;
using SC = System.Collections;
using AP.Collections.ReadOnly;
using System.Collections.Generic;
using AP.ComponentModel;

namespace AP.Collections
{
    public partial class SortedDictionary<TKey, TValue>
    {
        [Sorted(true)]
        public sealed class KeyCollection : DictionaryKeyCollection<SortedDictionary<TKey, TValue>, TKey, TValue>, IComparerUser<TKey>
        {
            public KeyCollection(SortedDictionary<TKey, TValue> dictionary)
                : base(dictionary)
            { }

            public new KeyCollection Clone()
            {
                return (KeyCollection)this.OnClone();
            }

            #region IComparerUser<TValue> Members

            public IComparer<TKey> Comparer
            {
                get { return base.Dictionary.KeyComparer; }
            }

            #endregion
        }
    }
}
