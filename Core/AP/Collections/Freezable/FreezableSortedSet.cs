using System.Collections.Generic;
using AP.Collections.ObjectModel;
using System;

namespace AP.Collections;

[Serializable, Sorted(true)]
public class FreezableSortedSet<T> : ExtendableSortedSet<T>, IFreezable
{
    #region IFreezable Members

    private bool _isFrozen;

    public FreezableSortedSet()
        : base()
    {
        _isFrozen = false;
    }

    public FreezableSortedSet(IEnumerable<T> collection, bool isFrozen = false)
        : base(collection)
    {
        _isFrozen = isFrozen;
    }

    public FreezableSortedSet(IComparer<T> comparer)
        : base(comparer)
    {
        _isFrozen = false;
    }

    public FreezableSortedSet(IEnumerable<T> collection, IComparer<T> comparer, bool isFrozen = false)
        : base(collection, comparer)
    {
        _isFrozen = isFrozen;
    }

    protected FreezableSortedSet(Set<T> inner, bool isFrozen = false)
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

    public new FreezableSortedSet<T> Clone() => (FreezableSortedSet<T>)this.OnClone();

    protected override CollectionBase<T> OnClone() => new FreezableSortedSet<T>(this, this.Comparer, false);
}