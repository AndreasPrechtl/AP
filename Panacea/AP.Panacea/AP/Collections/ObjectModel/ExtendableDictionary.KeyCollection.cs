using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Collections.ObjectModel
{
    public partial class ExtendableDictionary<TKey, TValue>
    {
        public new class KeyCollection : DictionaryBase<TKey, TValue>.KeyCollection, IEqualityComparerUser<TKey>
        {
            public KeyCollection(DictionaryBase<TKey, TValue> dictionary)
                : base(dictionary)
            { }

            public new KeyCollection Clone()
            {
                return (KeyCollection)this.OnClone();
            }

            #region IEqualityComparerUser<TKey> Members

            public IEqualityComparer<TKey> Comparer
            {
                get { return ((ExtendableDictionary<TKey, TValue>)base.Dictionary).KeyComparer; }
            }

            #endregion
        }
    }
}
