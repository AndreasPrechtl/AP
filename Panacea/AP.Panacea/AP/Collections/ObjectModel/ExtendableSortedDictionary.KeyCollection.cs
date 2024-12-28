using AP.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Collections.ObjectModel
{
    public partial class ExtendableSortedDictionary<TKey, TValue>
    {
        [Sorted(true)]
        public new class KeyCollection : DictionaryBase<TKey, TValue>.KeyCollection, IComparerUser<TKey>
        {
            public KeyCollection(DictionaryBase<TKey, TValue> dictionary)
                : base(dictionary)
            { }

            public new KeyCollection Clone()
            {
                return (KeyCollection)this.OnClone();
            }

            #region IComparerUser<TKey> Members

            public IComparer<TKey> Comparer
            {
                get { return ((ExtendableSortedDictionary<TKey, TValue>)base.Dictionary).KeyComparer; }
            }

            #endregion            
        }
    }
}
