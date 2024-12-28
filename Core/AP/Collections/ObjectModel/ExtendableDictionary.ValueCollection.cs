using System.Collections.Generic;

namespace AP.Collections.ObjectModel;

public partial class ExtendableDictionary<TKey, TValue>
{
    public new class ValueCollection : DictionaryBase<TKey, TValue>.ValueCollection, IEqualityComparerUser<TValue>
    {
        public ValueCollection(DictionaryBase<TKey, TValue> dictionary)
            : base(dictionary)
        { }

        public new ValueCollection Clone() => (ValueCollection)this.OnClone();

        #region IEqualityComparerUser<TValue> Members

        public IEqualityComparer<TValue> Comparer => ((ExtendableDictionary<TKey, TValue>)base.Dictionary).ValueComparer;

        #endregion
    }
}
