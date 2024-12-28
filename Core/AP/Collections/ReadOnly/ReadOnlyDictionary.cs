using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

namespace AP.Collections.ReadOnly;

[Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false), System.ComponentModel.ReadOnly(true)]
public partial class ReadOnlyDictionary<TKey, TValue> : IDictionaryView<TKey, TValue>
    where TKey : notnull
{
    private readonly AP.Collections.Dictionary<TKey, TValue> _inner;
    private readonly KeyCollection _keys;
    private readonly ValueCollection _values;

    private static readonly ReadOnlyDictionary<TKey, TValue> s_empty = new([]);
    
    public static ReadOnlyDictionary<TKey, TValue> Empty => s_empty;

    private static AP.Collections.Dictionary<TKey, TValue> CreateInner(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
    {
        ArgumentNullException.ThrowIfNull(collection);

        return new Dictionary<TKey, TValue>(collection, keyComparer, valueComparer);
    }

    public ReadOnlyDictionary(params IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
        : this(dictionary, null!, null!)
    { }

    public ReadOnlyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        : this(CreateInner(dictionary, keyComparer, valueComparer))
    { }

    protected ReadOnlyDictionary(AP.Collections.Dictionary<TKey, TValue> inner)
    {
        ArgumentNullException.ThrowIfNull(inner);

        _inner = inner;
        _keys = new KeyCollection(this);
        _values = new ValueCollection(this);
    }

    public IEqualityComparer<TKey> KeyComparer => _inner.KeyComparer;
    public IEqualityComparer<TValue> ValueComparer => _inner.ValueComparer;

    public KeyCollection Keys => _keys;
    public ValueCollection Values => _values;

    public override string ToString() => _inner.ToString();

    #region IDictionaryView<TKey,TValue> Members

    ICollection<TKey> IDictionaryView<TKey, TValue>.Keys => _keys;

    ICollection<TValue> IDictionaryView<TKey, TValue>.Values => _values;

    public bool Contains(KeyValuePair<TKey, TValue> item, bool compareValues = false) => _inner.Contains(item, compareValues);

    public bool Contains(TKey key, out TValue value) => _inner.Contains(key, out value);

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (key == null)
        {
            value = default;
            return false;
        }

        return this.Contains(key, out value);
    }

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

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex = 0) => _inner.CopyTo(array, arrayIndex);

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)_inner).Contains(item);

    #endregion

    #region IEnumerable<KeyValuePair<TKey,TValue>> Members

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _inner.GetEnumerator();

    #endregion

    #region IEnumerable Members

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    #endregion

    #region ICloneable Members

    public ReadOnlyDictionary<TKey, TValue> Clone() => this;

    object ICloneable.Clone() => this.Clone();

    #endregion

    #region IReadOnlyDictionary<TKey,TValue> Members

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => _keys;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => _values;

    #endregion
}