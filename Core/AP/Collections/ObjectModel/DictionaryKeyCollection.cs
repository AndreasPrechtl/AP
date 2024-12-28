using System;
using System.Collections.Generic;

namespace AP.Collections.ObjectModel;

// ICollection<TKey> is only implemented because of the old SCG.IDictionary<TKey, TValue> interface definition for the Keys collection
[System.ComponentModel.ReadOnly(true)]
public class DictionaryKeyCollection<TDictionary, TKey, TValue> : CollectionBase<TKey>, System.Collections.Generic.ICollection<TKey>
    where TDictionary : IDictionaryView<TKey, TValue>
    where TKey : notnull
{
    private readonly TDictionary _dictionary;

    protected DictionaryKeyCollection(TDictionary dictionary)
    {
        _dictionary = dictionary;
    }

    public TDictionary Dictionary => _dictionary;

    #region ICollection<TKey> Members

    public sealed override int Count => _dictionary.Count;

    public sealed override void CopyTo(TKey[] array, int arrayIndex = 0) => base.CopyTo(array, arrayIndex);

    public sealed override bool Contains(TKey key) => _dictionary.ContainsKey(key);

    #endregion

    #region IEnumerable<TKey> Members

    public sealed override IEnumerator<TKey> GetEnumerator()
    {
        foreach (var item in _dictionary)
            yield return item.Key;
    }

    #endregion

    #region ICloneable Members

    protected sealed override CollectionBase<TKey> OnClone() => this;

    #endregion

    #region ICollection<TKey> Members

    void System.Collections.Generic.ICollection<TKey>.Add(TKey item) => throw new NotSupportedException();

    void System.Collections.Generic.ICollection<TKey>.Clear() => throw new NotSupportedException();

    bool System.Collections.Generic.ICollection<TKey>.IsReadOnly => true;

    bool System.Collections.Generic.ICollection<TKey>.Remove(TKey item) => throw new NotSupportedException();

    #endregion
}
