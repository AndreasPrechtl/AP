using System;
using AP.Collections.ObjectModel;
using SCG = System.Collections.Generic;
using System.Linq;
using SC = System.Collections;
using System.Collections.Generic;

namespace AP.Collections
{
    public partial class Dictionary<TKey, TValue>
    {
        public sealed class ValueCollection : DictionaryValueCollection<Dictionary<TKey, TValue>, TKey, TValue>, IEqualityComparerUser<TValue>
        {
            public ValueCollection(Dictionary<TKey, TValue> dictionary)
                : base(dictionary)
            { }

            public new ValueCollection Clone()
            {
                return (ValueCollection)this.OnClone();
            }

            #region IEqualityComparerUser<TValue> Members

            public IEqualityComparer<TValue> Comparer
            {
                get { return base.Dictionary.ValueComparer; }
            }

            #endregion
        }
    }
}
