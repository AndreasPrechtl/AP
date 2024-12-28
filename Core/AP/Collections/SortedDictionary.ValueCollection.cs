using AP.Collections.ObjectModel;
using System.Collections.Generic;

namespace AP.Collections;

public partial class SortedDictionary<TKey, TValue>
{
    public sealed class ValueCollection : DictionaryValueCollection<SortedDictionary<TKey, TValue>, TKey, TValue>, IEqualityComparerUser<TValue>
    {
        public ValueCollection(SortedDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        { }

        public new ValueCollection Clone() => (ValueCollection)this.OnClone();

        #region IEqualityComparerUser<TValue> Members

        public IEqualityComparer<TValue> Comparer => base.Dictionary.ValueComparer;

        #endregion
    }
}
