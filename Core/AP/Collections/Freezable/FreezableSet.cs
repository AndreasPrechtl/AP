using System.Collections.Generic;
using AP.Collections.ObjectModel;
using System;

namespace AP.Collections;

[Serializable]
public class FreezableSet<T> : ExtendableSet<T>, IFreezable
{
    #region IFreezable Members

    private bool _isFrozen;

    public FreezableSet()
        : base()
    {
        _isFrozen = false;
    }
    
    public FreezableSet(IEnumerable<T> collection, bool isFrozen = false)
        : base(collection)
    {
        _isFrozen = isFrozen;
    }
    
    public FreezableSet(IEqualityComparer<T> comparer)
        : base(comparer)
    {
        _isFrozen = false;
    }
    
    public FreezableSet(IEnumerable<T> collection, IEqualityComparer<T> comparer, bool isFrozen = false)
        : base(collection, comparer)
    { 
        _isFrozen = isFrozen; 
    }

    protected FreezableSet(Set<T> inner, bool isFrozen = false)
        : base(inner)
    {
        _isFrozen = isFrozen;
    }

    public virtual bool IsFrozen
    {
        get => _isFrozen;
        set
        {
            this.AssertCanWrite();
            _isFrozen = value;
        }
    }

    protected virtual void AssertCanWrite() => FreezableHelper.AssertCanWrite(this);

    #endregion

    public override bool Add(T item)
    {
        AssertCanWrite();
        return base.Add(item);
    }

    public override void Clear()
    {
        AssertCanWrite();
        base.Clear();
    }
    
    public override bool Remove(T item)
    {
        AssertCanWrite();
        return base.Remove(item);
    }

    public new FreezableSet<T> Clone() => (FreezableSet<T>)this.OnClone();

    protected override CollectionBase<T> OnClone() => new FreezableSet<T>(this, this.Comparer, false);
}