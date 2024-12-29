﻿using System;

namespace AP.ComponentModel.ObjectManagement;

public sealed class DeferrableLifetime<TBase> : ObjectLifetimeBase<TBase>
    where TBase : notnull
{
    private Deferrable<TBase> _inner;
    
    public DeferrableLifetime(TBase instance, object? key = null)
        : this(new Deferrable<TBase>(instance), key)
    {
        if (instance == null)
            throw new ArgumentNullException(nameof(instance));
    }

    public DeferrableLifetime(Activator<TBase> activator, object? key = null)
        : this(new Deferrable<TBase>(activator), key)
    {
        ArgumentNullException.ThrowIfNull(activator);

        _inner = activator;
    }
    
    public DeferrableLifetime(Deferrable<TBase> deferrable, object? key = null)
    {
        ArgumentNullException.ThrowIfNull(deferrable);

        _inner = deferrable;
    }

    public bool IsInstanceActive => _inner.IsValueActive;

    public override ManagedInstance<TBase> Instance => new(_inner.Value, false);

    protected override void CleanUpResources()
    {
        base.CleanUpResources();

        if (this.IsInstanceActive)
        {
            _inner?.Value?.TryDispose();
            _inner = null!;
        }
    }
}
