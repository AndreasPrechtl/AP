using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AP.Collections.ObjectModel;

namespace AP.Collections;

[Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false)]
public class FreezableList<T> : ExtendableList<T>, IFreezable
{
    public FreezableList()
        : base()
    {
        _isFrozen = false;
    }

    public FreezableList(IEqualityComparer<T> comparer)
        : base(comparer)
    {
        _isFrozen = false;
    }

    public FreezableList(int capacity)
        : base(capacity)
    {
        _isFrozen = false;
    }

    public FreezableList(int capacity, IEqualityComparer<T> comparer)
        : base(capacity, comparer)
    {
        _isFrozen = false;
    }

    public FreezableList(IEnumerable<T> collection, bool isFrozen = false)
        : base(collection)
    {
        _isFrozen = isFrozen;
    }

    public FreezableList(IEnumerable<T> collection, IEqualityComparer<T> comparer, bool isFrozen = false)
        : base(collection, comparer)
    {
        _isFrozen = isFrozen;
    }

    protected FreezableList(AP.Collections.List<T> inner, bool isFrozen = false)
        : base(inner)
    {
        _isFrozen = isFrozen;
    }

    public new FreezableList<T> Clone() => (FreezableList<T>)this.OnClone();

    protected override CollectionBase<T> OnClone() => new FreezableList<T>(this, this.Comparer, this.IsFrozen);

    #region IFreezable Members

    private bool _isFrozen;

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

    public override int Add(T item)
    {
        AssertCanWrite();
        return base.Add(item);
    }

    public override void Clear()
    {
        AssertCanWrite();
        base.Clear();
    }

    public override void Insert(int index, T item)
    {
        AssertCanWrite();
        base.Insert(index, item);
    }
    
    protected override void SetItem(int index, T item)
    {
        AssertCanWrite();
        base.SetItem(index, item);
    }

    public override void Move(int index, int newIndex, int count = 1)
    {
        AssertCanWrite();
        base.Move(index, newIndex, count);
    }

    public override void Remove(int index, int count = 1)
    {
        AssertCanWrite();
        base.Remove(index, count);
    }

    public override void Insert(int index, IEnumerable<T> items)
    {
        AssertCanWrite();
        base.Insert(index, items);
    }
    
    public override void Remove(T item, SelectionMode mode = SelectionMode.First)
    {
        AssertCanWrite();
        base.Remove(item, mode);
    }

    public override void Add(IEnumerable<T> collection)
    {
        AssertCanWrite();
        base.Add(collection);
    }
}
