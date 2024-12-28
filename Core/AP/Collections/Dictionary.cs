using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;

using System.Collections.Generic;

namespace AP.Collections;

[Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false)]
public partial class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>
    where TKey : notnull
{
    private readonly System.Collections.Generic.Dictionary<TKey, TValue> _inner;

    private readonly IEqualityComparer<TKey> _keyComparer;
    private readonly IEqualityComparer<TValue> _valueComparer;

    private readonly KeyCollection _keys;
    private readonly ValueCollection _values;

    /// <summary>
    /// Wraps an existing <cref="System.Collections.Generic.Dictionary<TKey, TValue>" /> into a new <cref="AP.Collections.Dictionary<TKey, TValue>" />
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="valueComparer"></param>
    /// <returns></returns>
    public static Dictionary<TKey, TValue> Wrap(System.Collections.Generic.Dictionary<TKey, TValue> dictionary, IEqualityComparer<TValue>? valueComparer = null) => new(dictionary, valueComparer ?? EqualityComparer<TValue>.Default);

    private static System.Collections.Generic.Dictionary<TKey, TValue> CreateInnerDictionary(int capacity, IEqualityComparer<TKey> keyComparer)
    {
        keyComparer ??= EqualityComparer<TKey>.Default;

        return new System.Collections.Generic.Dictionary<TKey, TValue>(capacity, keyComparer);
    }

    private static System.Collections.Generic.Dictionary<TKey, TValue> CreateInnerDictionary(IEnumerable<KeyValuePair<TKey, TValue>> items, IEqualityComparer<TKey> keyComparer)
    {
        System.Collections.Generic.Dictionary<TKey, TValue> dict = new(keyComparer ?? EqualityComparer<TKey>.Default);

        if (items != null)
            foreach (var item in items)
                dict.Add(item.Key, item.Value);

        return dict;
    }

    public Dictionary()
        : this(0, null!, null!)
    { }

    public Dictionary(int capacity)
        : this(capacity, null!, null!)
    { }

    public Dictionary(IEqualityComparer<TKey> keyComparer)
        : this(0, keyComparer, null!) 
    { }

    public Dictionary(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        : this(0, keyComparer, valueComparer)
    { }

    public Dictionary(int capacity, IEqualityComparer<TKey> keyComparer)
        : this(capacity, keyComparer, null!)
    { }

    public Dictionary(int capacity, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        : this(CreateInnerDictionary(capacity, keyComparer), valueComparer ?? EqualityComparer<TValue>.Default)
    { }

    public Dictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
        : this(dictionary, null!, null!)
    { }

    public Dictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEqualityComparer<TKey> keyComparer)
        : this(dictionary, keyComparer, null!)
    { }

    public Dictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        : this(CreateInnerDictionary(dictionary, keyComparer), valueComparer ?? EqualityComparer<TValue>.Default)
    { }

    protected Dictionary(System.Collections.Generic.Dictionary<TKey, TValue> inner, IEqualityComparer<TValue> valueComparer)
    {
        ArgumentNullException.ThrowIfNull(inner);
        ArgumentNullException.ThrowIfNull(valueComparer);

        _keyComparer = inner.Comparer;
        _valueComparer = valueComparer;
        _inner = inner;
        _keys = new KeyCollection(this);
        _values = new ValueCollection(this);
    }

    public KeyCollection Keys => _keys;
    public ValueCollection Values => _values;

    public IEqualityComparer<TKey> KeyComparer => _keyComparer;
    public IEqualityComparer<TValue> ValueComparer => _valueComparer;

    public override string ToString() => _inner.ToString()!;

    public static implicit operator Dictionary<TKey, TValue>(System.Collections.Generic.Dictionary<TKey, TValue> dictionary)
        => Wrap(dictionary);

    public static implicit operator System.Collections.Generic.Dictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        => dictionary._inner;

    #region IDictionary<TKey,TValue> Members

    public bool Add(TKey key, TValue value)
    {
        bool b;
        if (!(b = _inner.ContainsKey(key)))
            _inner.Add(key, value);

        return b;
    }

    public bool Add(KeyValuePair<TKey, TValue> item) => this.Add(item.Key, item.Value);

    public void Add(IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        foreach (KeyValuePair<TKey, TValue> item in items)
            _inner.Add(item.Key, item.Value);
    }

    public bool Update(TKey key, TValue value)
    {
        bool b;
        if (b = _inner.ContainsKey(key))
            _inner[key] = value;

        return b;
    }

    public bool Update(KeyValuePair<TKey, TValue> item) => this.Update(item.Key, item.Value);

    public void Update(IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        foreach (KeyValuePair<TKey, TValue> kvp in items)
            this.Update(kvp);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item, bool compareValues = false)
    {
        if (compareValues)
        {
            if (this.Contains(item, compareValues))
                return this.Remove(item.Key);

            return false;
        }

        return this.Remove(item.Key);
    }

    public void Remove(IEnumerable<TKey> keys)
    {
        ArgumentNullException.ThrowIfNull(keys);

        foreach (TKey key in keys)
            _inner.Remove(key);
    }

    public void Remove(IEnumerable<KeyValuePair<TKey, TValue>> items, bool compareValues = false)
    {
        ArgumentNullException.ThrowIfNull(items);

        List<TKey> keys = [];

        foreach (var current in this)
        {
            TKey key = current.Key;

            foreach (var item in items)
            {
                if (_keyComparer.Equals(key, item.Key) && (!compareValues || _valueComparer.Equals(current.Value, item.Value)))
                    keys.Add(key);
            }
        }

        foreach (TKey key in keys)
            _inner.Remove(key);
    }

    #endregion

    #region System.Collections.Generic.IDictionary<TKey,TValue> Members

    void System.Collections.Generic.IDictionary<TKey, TValue>.Add(TKey key, TValue value) => _inner.Add(key, value);

    bool System.Collections.Generic.IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) => _inner.TryGetValue(key, out value!);

    public bool ContainsKey(TKey key) => _inner.ContainsKey(key);

    System.Collections.Generic.ICollection<TKey> System.Collections.Generic.IDictionary<TKey, TValue>.Keys => _inner.Keys;

    System.Collections.Generic.ICollection<TValue> System.Collections.Generic.IDictionary<TKey, TValue>.Values => _inner.Values;

    public bool Remove(TKey key) => _inner.Remove(key);

    public bool Contains(TKey key, out TValue value) => _inner.TryGetValue(key, out value);

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (key == null)
        {
            value = default!;
            return false;
        }

        return this.Contains(key, out value);
    }

    public TValue this[TKey key]
    {
        get => _inner[key];
        set => _inner[key] = value;
    }

    public IEnumerable<TValue> this[IEnumerable<TKey> keys]
    {
        get
        {
            ArgumentNullException.ThrowIfNull(keys);

            foreach (TKey key in keys)
                yield return _inner[key];
        }
    }

    #endregion

    #region System.Collections.Generic.ICollection<KeyValuePair<TKey,TValue>> Members

    void System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => ((System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>)_inner).Add(item);

    bool System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

    bool System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => this.Remove(item, false);

    public void Clear() => _inner.Clear();

    bool System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => this.Contains(item, false);

    void System.Collections.Generic.ICollection<KeyValuePair<TKey,TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => CollectionsHelper.CopyTo(this, array, arrayIndex);

    public int Count => _inner.Count;

    #endregion

    #region IEnumerable<KeyValuePair<TKey,TValue>> Members

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _inner.GetEnumerator();

    #endregion

    #region ICollection<KeyValuePair<TKey, TValue>> Members

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => this.Contains(item, false);

    #endregion

    #region IEnumerable Members

    IEnumerator IEnumerable.GetEnumerator() => _inner.GetEnumerator();

    #endregion

    #region IDictionaryView<TKey,TValue> Members

    ICollection<TKey> IDictionaryView<TKey, TValue>.Keys => this.Keys;

    ICollection<TValue> IDictionaryView<TKey, TValue>.Values => this.Values;

    public bool Contains(KeyValuePair<TKey, TValue> keyValuePair, bool compareValues = false)
    {
        if (compareValues)
        {
            return _inner.TryGetValue(keyValuePair.Key, out TValue? value) && _valueComparer.Equals(keyValuePair.Value, value);
        }

        return _inner.ContainsKey(keyValuePair.Key);
    }

    public bool ContainsValue(TValue value)
    {
        foreach (KeyValuePair<TKey, TValue> kvp in _inner)
            if (_valueComparer.Equals(value, kvp.Value))
                return true;

        return false;
    }

    #endregion

    #region IReadOnlyDictionary<TKey,TValue> Members

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => _keys;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => _values;

    #endregion

    #region ICloneable Members

    public Dictionary<TKey, TValue> Clone() => this.OnClone();

    protected virtual Dictionary<TKey, TValue> OnClone() => new(this, _keyComparer, _valueComparer);

    object ICloneable.Clone() => this.OnClone();

    #endregion    
}
