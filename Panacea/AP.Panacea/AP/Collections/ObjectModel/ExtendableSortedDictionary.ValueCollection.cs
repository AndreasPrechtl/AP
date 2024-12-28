using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Collections.ObjectModel
{
    public partial class ExtendableSortedDictionary<TKey, TValue>
    {
        public new class ValueCollection : DictionaryBase<TKey, TValue>.ValueCollection, IEqualityComparerUser<TValue>
        {
            public ValueCollection(DictionaryBase<TKey, TValue> dictionary)
                : base(dictionary)
            { }

            public new ValueCollection Clone()
            {
                return (ValueCollection)this.OnClone();
            }

            #region IEqualityComparerUser<TValue> Members

            public IEqualityComparer<TValue> Comparer
            {
                get { return ((ExtendableSortedDictionary<TKey, TValue>)base.Dictionary).ValueComparer; }
            }

            #endregion
        }
    }
}
