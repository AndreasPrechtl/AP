using AP.Collections.ObjectModel;
using System.Collections.Generic;

namespace AP.Collections.ReadOnly;

public partial class ReadOnlySortedDictionary<TKey, TValue>
{
    [System.ComponentModel.ReadOnly(true), Sorted(true)]
    public sealed class KeyCollection : DictionaryKeyCollection<ReadOnlySortedDictionary<TKey, TValue>, TKey, TValue>, IComparerUser<TKey>
    {
        public KeyCollection(ReadOnlySortedDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        { }

        #region IComparerUser<TKey> Members

        public IComparer<TKey> Comparer => base.Dictionary.KeyComparer;

        #endregion

        public new KeyCollection Clone() => (KeyCollection)base.Clone();
    }
}
