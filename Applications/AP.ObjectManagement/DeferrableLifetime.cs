using System;

namespace AP.ComponentModel.ObjectManagement;

public sealed class DeferrableLifetime<TBase> : ObjectLifetimeBase<TBase>
    where TBase : notnull
{
    private Lazy<TBase> _inner;
    
    public DeferrableLifetime(TBase instance, object? key = null)
        : this(new Lazy<TBase>(instance), key)
    {
        if (instance == null)
            throw new ArgumentNullException(nameof(instance));
    }

    public DeferrableLifetime(Activator<TBase> activator, object? key = null)
        : this(new Lazy<TBase>((Func<TBase>)(object)activator), key)
    {
        ArgumentNullException.ThrowIfNull(activator);
    }
    
    public DeferrableLifetime(Lazy<TBase> deferrable, object? key = null)
    {
        ArgumentNullException.ThrowIfNull(deferrable);

        _inner = deferrable;
    }

    public bool IsInstanceActive => _inner.IsValueCreated;

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
