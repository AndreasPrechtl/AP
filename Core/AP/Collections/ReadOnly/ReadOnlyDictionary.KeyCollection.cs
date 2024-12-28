using AP.Collections.ObjectModel;
using System.Collections.Generic;

namespace AP.Collections.ReadOnly;

public partial class ReadOnlyDictionary<TKey, TValue>
{
    [System.ComponentModel.ReadOnly(true)]
    public sealed class KeyCollection : DictionaryKeyCollection<ReadOnlyDictionary<TKey, TValue>, TKey, TValue>, IEqualityComparerUser<TKey>
    {
        public KeyCollection(ReadOnlyDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        { }

        #region IEqualityComparerUser<TKey> Members

        public IEqualityComparer<TKey> Comparer => base.Dictionary.KeyComparer;

        #endregion

        public new KeyCollection Clone() => (KeyCollection)this.OnClone();
    }
}
