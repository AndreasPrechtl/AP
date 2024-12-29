using System.Collections.Generic;
using AP.Collections.ObjectModel;

namespace AP.Collections.Freezable;

[Sorted(true)]
public class FreezableSortedDictionary<TKey, TValue> : ExtendableSortedDictionary<TKey, TValue>, IFreezable
    where TKey : notnull
{ 
    protected FreezableSortedDictionary(bool isFrozen = false)
        : base()
    {
        _isFrozen = isFrozen;
    }

    protected FreezableSortedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, bool isFrozen = false)
        : base(dictionary)
    {
        _isFrozen = isFrozen;
    }

    protected FreezableSortedDictionary(IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer, bool isFrozen = false)
        : base(keyComparer, valueComparer)
    {
        _isFrozen = isFrozen;
    }

    protected FreezableSortedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer, bool isFrozen = false)
        : base(dictionary, keyComparer, valueComparer)
    {
        _isFrozen = isFrozen;
    }

    protected FreezableSortedDictionary(SortedDictionary<TKey, TValue> inner, bool isFrozen = false)
        : base(inner)
    {
        _isFrozen = isFrozen;
    }

    public new FreezableSortedDictionary<TKey, TValue> Clone() => (FreezableSortedDictionary<TKey, TValue>)this.OnClone();

    protected override CollectionBase<KeyValuePair<TKey, TValue>> OnClone() => new FreezableSortedDictionary<TKey, TValue>(this, this.KeyComparer, this.ValueComparer, this.IsFrozen);

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
