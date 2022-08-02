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
        public sealed class ValueCollection : DictionaryValueCollection<ReadOnlyDictionary<TKey, TValue>, TKey, TValue>, IEqualityComparerUser<TValue>
        {
            public ValueCollection(ReadOnlyDictionary<TKey, TValue> dictionary)
                : base(dictionary)
            { }

            #region IEqualityComparerUser<TValue> Members

            public IEqualityComparer<TValue> Comparer
            {
                get { return base.Dictionary.ValueComparer; }
            }

            #endregion

            public new ValueCollection Clone()
            {
                return (ValueCollection)base.Clone();
            }
        }
    }
}
