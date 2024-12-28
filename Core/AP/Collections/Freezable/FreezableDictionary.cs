using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AP.Collections.ObjectModel;

namespace AP.Collections;

[Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false)]
public class FreezableDictionary<TKey, TValue> : ExtendableDictionary<TKey, TValue>, IFreezable
    where TKey : notnull
{
    protected FreezableDictionary(bool isFrozen = false)
        : base()
    {
        _isFrozen = isFrozen;
    }

    protected FreezableDictionary(int capacity, bool isFrozen = false)
        : base(capacity)
    {
        _isFrozen = isFrozen;
    }

    protected FreezableDictionary(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer, bool isFrozen = false)
        : base(keyComparer, valueComparer)
    {
        _isFrozen = isFrozen;
    }

    protected FreezableDictionary(int capacity, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer, bool isFrozen = false)
        : base(capacity, keyComparer, valueComparer)
    {
        _isFrozen = isFrozen;
    }
    
    protected FreezableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, bool isFrozen = false)
        : base(dictionary)
    {
        _isFrozen = isFrozen;
    }
    
    protected FreezableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer, bool isFrozen = false)
        : base(dictionary, keyComparer, valueComparer)
    {
        _isFrozen = isFrozen;
    }
    
    protected FreezableDictionary(Dictionary<TKey, TValue> inner, bool isFrozen = false)
        : base(inner)
    {
        _isFrozen = isFrozen;
    }

    public new FreezableDictionary<TKey, TValue> Clone() => (FreezableDictionary<TKey, TValue>)this.OnClone();

    protected override CollectionBase<KeyValuePair<TKey, TValue>> OnClone() => new FreezableDictionary<TKey, TValue>(this, this.KeyComparer, this.ValueComparer, this.IsFrozen);

    public override bool Add(TKey key, TValue value)
    {
        this.AssertCanWrite();
        return base.Add(key, value);
    }
    public override TValue this[TKey key]
    {
        get => base[key];
        set
        {
            this.AssertCanWrite();
            base[key] = value;
        }
    }
    public override void Clear()
    {
        this.AssertCanWrite();
        base.Clear();
    }
    public override bool Remove(TKey key)
    {
        this.AssertCanWrite();
        return base.Remove(key);
    }

    #region IFreezable Members

    private bool _isFrozen;

    public virtual bool IsFrozen
    {
        get => _isFrozen;
        set
        {
            AssertCanWrite();
            _isFrozen = value;
        }
    }

    protected virtual void AssertCanWrite() => FreezableHelper.AssertCanWrite(this);

    #endregion
}