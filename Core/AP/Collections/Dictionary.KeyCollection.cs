using AP.Collections.ObjectModel;
using System.Collections.Generic;

namespace AP.Collections;

public partial class Dictionary<TKey, TValue>
{
    public sealed class KeyCollection : DictionaryKeyCollection<Dictionary<TKey, TValue>, TKey, TValue>, IEqualityComparerUser<TKey>
    {
        public KeyCollection(Dictionary<TKey, TValue> dictionary)
            : base(dictionary)
        { }

        public new KeyCollection Clone() => (KeyCollection)this.OnClone();

        #region IEqualityComparerUser<TKey> Members

        public IEqualityComparer<TKey> Comparer => base.Dictionary.KeyComparer;

        #endregion
    }
}
