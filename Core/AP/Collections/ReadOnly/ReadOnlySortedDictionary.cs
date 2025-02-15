﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

namespace AP.Collections.ReadOnly;

[Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false), System.ComponentModel.ReadOnly(true), Sorted(true)]
public partial class ReadOnlySortedDictionary<TKey, TValue> : IDictionaryView<TKey, TValue>, System.Collections.Generic.IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    public static readonly ReadOnlySortedDictionary<TKey, TValue> Empty = new(new Dictionary<TKey, TValue>(0));

    private readonly AP.Collections.SortedDictionary<TKey, TValue> _inner;
    private readonly KeyCollection _keys;
    private readonly ValueCollection _values;
    
    private static AP.Collections.SortedDictionary<TKey, TValue> CreateInner(IEnumerable<KeyValuePair<TKey, TValue>> collection, IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
    {
        ArgumentNullException.ThrowIfNull(collection);

        return new SortedDictionary<TKey, TValue>(collection, keyComparer, valueComparer);
    }        

    public ReadOnlySortedDictionary(params IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
        : this(dictionary, null!, null!)
    { }

    public ReadOnlySortedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        : this(CreateInner(dictionary, keyComparer, valueComparer))
    { }

    protected ReadOnlySortedDictionary(AP.Collections.SortedDictionary<TKey, TValue> inner)
    {
        ArgumentNullException.ThrowIfNull(inner);

        _inner = inner;
        _keys = new KeyCollection(this);
        _values = new ValueCollection(this);
    }

    public IComparer<TKey> KeyComparer => _inner.KeyComparer;
    public IEqualityComparer<TValue> ValueComparer => _inner.ValueComparer;

    public KeyCollection Keys => _keys;
    public ValueCollection Values => _values;

    public override string ToString() => _inner.ToString();

    #region IDictionaryView<TKey,TValue> Members

    ICollection<TKey> IDictionaryView<TKey, TValue>.Keys => _keys;

    ICollection<TValue> IDictionaryView<TKey, TValue>.Values => _values;

    public bool Contains(KeyValuePair<TKey, TValue> item, bool compareValues = false) => _inner.Contains(item, compareValues);

    public bool TryGetValue(TKey key, out TValue? value) => _inner.TryGetValue(key, out value!);

    public bool ContainsKey(TKey key) => _inner.ContainsKey(key);

    public bool ContainsValue(TValue value) => _inner.ContainsValue(value);

    public TValue this[TKey key]
    {
        get { return _inner[key]; }
    }

    public IEnumerable<TValue> this[IEnumerable<TKey> keys]
    {
        get { return _inner[keys]; }
    }

    #endregion

    #region ICollection<KeyValuePair<TKey,TValue>> Members

    public int Count => _inner.Count;

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)_inner).Contains(item);

    #endregion

    #region IEnumerable<KeyValuePair<TKey,TValue>> Members

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _inner.GetEnumerator();

    #endregion

    #region IEnumerable Members

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    #endregion

    #region ICloneable Members

    public virtual ReadOnlySortedDictionary<TKey, TValue> Clone() => this;

    object ICloneable.Clone() => this.Clone();

    #endregion

    #region IReadOnlyDictionary<TKey,TValue> Members

    bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) => this.TryGetValue(key, out value!);

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => _keys;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => _values;

    #endregion
}
