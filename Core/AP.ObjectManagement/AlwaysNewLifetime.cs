using System;

namespace AP.ComponentModel.ObjectManagement;

public sealed class AlwaysNewLifetime<TBase> : ObjectLifetimeBase<TBase>
    where TBase : notnull
{
    private Activator<TBase> _activator;
    
    public AlwaysNewLifetime(Activator<TBase> activator, object? key = null)
        : base(key)
    {
        ArgumentNullException.ThrowIfNull(activator);

        _activator = activator;
    }

    public override ManagedInstance<TBase> Instance => new(_activator(), true);

    protected override void CleanUpResources()
    {
        _activator = null!;
        base.CleanUpResources();
    }
}
