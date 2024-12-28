using System.Collections.Generic;

namespace AP.Collections.ObjectModel;

public partial class ExtendableDictionary<TKey, TValue>
{
    public new class KeyCollection : DictionaryBase<TKey, TValue>.KeyCollection, IEqualityComparerUser<TKey>
    {
        public KeyCollection(DictionaryBase<TKey, TValue> dictionary)
            : base(dictionary)
        { }

        public new KeyCollection Clone() => (KeyCollection)this.OnClone();

        #region IEqualityComparerUser<TKey> Members

        public IEqualityComparer<TKey> Comparer => ((ExtendableDictionary<TKey, TValue>)base.Dictionary).KeyComparer;

        #endregion
    }
}
