﻿namespace AP.ComponentModel.ObjectManagement;

public abstract class ObjectLifetimeBase<TBase> : DisposableObject, IObjectLifetimeInternal
    where TBase : notnull
{
    private readonly object? _key;

    protected ObjectLifetimeBase(object? key = null)
    {
        _key = key;
    }

    public abstract ManagedInstance<TBase> Instance { get; }

    #region IObjectLifetimeInternal Members

    public object Key => _key!;

    #endregion
}
