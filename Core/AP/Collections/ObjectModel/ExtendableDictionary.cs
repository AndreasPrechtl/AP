using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace AP.Collections.ObjectModel;

[Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false)]
public abstract partial class ExtendableDictionary<TKey, TValue> : DictionaryBase<TKey, TValue>
    where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _inner;

    protected Dictionary<TKey, TValue> Inner => _inner;

    protected ExtendableDictionary()
        : this(0, null!, null!)
    { }

    protected ExtendableDictionary(int capacity)
        : this(capacity, null!, null!)
    { }

    protected ExtendableDictionary(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        : this(0, keyComparer, valueComparer)
    { }

    protected ExtendableDictionary(int capacity, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        : this(new Dictionary<TKey, TValue>(capacity, keyComparer, valueComparer))
    { }
    
    protected ExtendableDictionary(params IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
        : this(dictionary, null!, null!)
    { }
    
    protected ExtendableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        : this(new Dictionary<TKey, TValue>(dictionary, keyComparer, valueComparer))
    { }
    
    protected ExtendableDictionary(Dictionary<TKey, TValue> inner)
    {
        ArgumentNullException.ThrowIfNull(inner);

        _inner = inner;
    }

    public new KeyCollection Keys => (KeyCollection)base.Keys;
    public new ValueCollection Values => (ValueCollection)base.Values;

    public IEqualityComparer<TKey> KeyComparer => _inner.KeyComparer;
    public IEqualityComparer<TValue> ValueComparer => _inner.ValueComparer;

    public new ExtendableDictionary<TKey, TValue> Clone() => (ExtendableDictionary<TKey, TValue>)this.OnClone();

    public override string ToString() => _inner.ToString();

    public override bool Add(TKey key, TValue value) => _inner.Add(key, value);

    public override void Add(IEnumerable<KeyValuePair<TKey, TValue>> items) => _inner.Add(items);

    public override bool Add(KeyValuePair<TKey, TValue> item) => _inner.Add(item);

    public override bool Update(KeyValuePair<TKey, TValue> item) => _inner.Update(item);

    public override void Update(IEnumerable<KeyValuePair<TKey, TValue>> items) => _inner.Update(items);

    public override void Remove(IEnumerable<KeyValuePair<TKey, TValue>> items, bool compareValues = false) => _inner.Remove(items, compareValues);

    public override void Remove(IEnumerable<TKey> keys) => _inner.Remove(keys);

    public override bool Remove(KeyValuePair<TKey, TValue> item, bool compareValues = false) => _inner.Remove(item, compareValues);

    public override void Clear() => _inner.Clear();

    public override bool Contains(KeyValuePair<TKey, TValue> item, bool compareValues = false) => _inner.Contains(item, compareValues);

    public override bool Contains(TKey key, out TValue value) => _inner.Contains(key, out value);

    public override bool ContainsKey(TKey key) => _inner.ContainsKey(key);

    public override bool ContainsValue(TValue value) => _inner.ContainsValue(value);

    public override int Count => _inner.Count;

    public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _inner.GetEnumerator();

    public override bool Remove(TKey key) => _inner.Remove(key);

    public override TValue this[TKey key]
    {
        get => _inner[key];
        set => _inner[key] = value;
    }
}
