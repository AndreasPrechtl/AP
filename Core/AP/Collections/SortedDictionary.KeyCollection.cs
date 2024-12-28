using AP.Collections.ObjectModel;
using System.Collections.Generic;
using AP.ComponentModel;

namespace AP.Collections;

public partial class SortedDictionary<TKey, TValue>
{
    [Sorted(true)]
    public sealed class KeyCollection : DictionaryKeyCollection<SortedDictionary<TKey, TValue>, TKey, TValue>, IComparerUser<TKey>
    {
        public KeyCollection(SortedDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        { }

        public new KeyCollection Clone() => (KeyCollection)this.OnClone();

        #region IComparerUser<TValue> Members

        public IComparer<TKey> Comparer => base.Dictionary.KeyComparer;

        #endregion
    }
}
