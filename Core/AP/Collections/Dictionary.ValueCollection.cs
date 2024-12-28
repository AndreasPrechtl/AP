using AP.Collections.ObjectModel;
using System.Collections.Generic;

namespace AP.Collections;

public partial class Dictionary<TKey, TValue>
{
    public sealed class ValueCollection : DictionaryValueCollection<Dictionary<TKey, TValue>, TKey, TValue>, IEqualityComparerUser<TValue>
    {
        public ValueCollection(Dictionary<TKey, TValue> dictionary)
            : base(dictionary)
        { }

        public new ValueCollection Clone() => (ValueCollection)this.OnClone();

        #region IEqualityComparerUser<TValue> Members

        public IEqualityComparer<TValue> Comparer => base.Dictionary.ValueComparer;

        #endregion
    }
}
