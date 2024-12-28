using AP.Collections.ObjectModel;
using System.Collections.Generic;

namespace AP.Collections.ReadOnly;

public partial class ReadOnlySortedDictionary<TKey, TValue>
{
    [System.ComponentModel.ReadOnly(true)]
    public sealed class ValueCollection : DictionaryValueCollection<ReadOnlySortedDictionary<TKey, TValue>, TKey, TValue>, IEqualityComparerUser<TValue>
    {
        public ValueCollection(ReadOnlySortedDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        { }

        #region IEqualityComparerUser<TValue> Members

        public IEqualityComparer<TValue> Comparer => base.Dictionary.ValueComparer;

        #endregion

        public new ValueCollection Clone() => (ValueCollection)this.OnClone();
    }
}
