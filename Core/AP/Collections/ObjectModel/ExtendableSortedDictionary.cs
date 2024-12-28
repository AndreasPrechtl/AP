using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

using SCG = System.Collections.Generic;
using AP.ComponentModel;

namespace AP.Collections.ObjectModel;

[Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false), Sorted(true)]
public abstract partial class ExtendableSortedDictionary<TKey, TValue> : DictionaryBase<TKey, TValue>
    where TKey : notnull
{
    private readonly AP.Collections.SortedDictionary<TKey, TValue> _inner;

    protected SortedDictionary<TKey, TValue> Inner => _inner;

    protected ExtendableSortedDictionary()
        : this(null!, null!, null!)
    { }

    protected ExtendableSortedDictionary(params IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
        : this(dictionary, null!, null!)
    { }

    protected ExtendableSortedDictionary(IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        : this(null, keyComparer, valueComparer)
    { }

    protected ExtendableSortedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        : this(new SortedDictionary<TKey, TValue>(dictionary, keyComparer, valueComparer))
    { }
    
    protected ExtendableSortedDictionary(SortedDictionary<TKey, TValue> inner)
    {
        ArgumentNullException.ThrowIfNull(inner);

        _inner = inner;
    }

    public new KeyCollection Keys => (KeyCollection)base.Keys;
    public new ValueCollection Values => (ValueCollection)base.Values;

    #region IDictionary<TKey,TValue> Members

    public override bool ContainsValue(TValue value) => _inner.ContainsValue(value);

    #endregion

    #region IDictionary<TKey,TValue> Members

    public override bool Add(TKey key, TValue value) => _inner.Add(key, value);

    public override bool ContainsKey(TKey key) => _inner.ContainsKey(key);

    public override bool Remove(KeyValuePair<TKey, TValue> item, bool compareValues = false) => _inner.Remove(item, compareValues);

    public override bool Remove(TKey key) => _inner.Remove(key);

    public override bool Contains(TKey key, out TValue value) => _inner.Contains(key, out value);

    public override TValue this[TKey key]
    {
        get => _inner[key];
        set => _inner[key] = value;
    }

    #endregion

    #region ICollection<KeyValuePair<TKey,TValue>> Members

    public override bool Add(KeyValuePair<TKey, TValue> item) => _inner.Add(item.Key, item.Value);

    public override void Clear() => _inner.Clear();

    public override int Count => _inner.Count;

    #endregion

    #region IEnumerable<KeyValuePair<TKey,TValue>> Members

    public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _inner.GetEnumerator();

    #endregion

    #region IEnumerable Members

    #endregion

    public new ExtendableSortedDictionary<TKey, TValue> Clone() => (ExtendableSortedDictionary<TKey, TValue>)this.OnClone();

    public override string ToString() => _inner.ToString();

    public IComparer<TKey> KeyComparer => _inner.KeyComparer;

    public IEqualityComparer<TValue> ValueComparer => _inner.ValueComparer;
}
