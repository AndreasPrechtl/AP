using System;

namespace AP.Observable;

public class ObjectChangingEventArgs : System.ComponentModel.CancelEventArgs
{
    public static new readonly ObjectChangingEventArgs Empty = new();

    private readonly object _container = null!;
    private readonly object? _oldValue;
    private readonly object? _newValue;

    public object Container => _container;
    public object? OldValue => _oldValue;
    public object? NewValue => _newValue;

    private ObjectChangingEventArgs()
    { }

    public ObjectChangingEventArgs(object container, object? oldValue, object? newValue)
        : this()
    {
        ArgumentNullException.ThrowIfNull(container);

        _container = container;
        _oldValue = oldValue;
        _newValue = newValue;
    }

    public virtual bool HasChanges => Equals(_oldValue, _newValue);
}

public class ObjectChangingEventArgs<T> : ObjectChangingEventArgs
{
    public new T Container => (T)base.Container!;

    public ObjectChangingEventArgs(T container, object? oldValue, object? newValue)
        : base(container!, oldValue, newValue)
    { }
}

public class ObjectChangingEventArgs<T, TValue> : ObjectChangingEventArgs<T>
{
    public new TValue? OldValue => (TValue?)base.OldValue;
    public new TValue? NewValue => (TValue?)base.NewValue;

    public ObjectChangingEventArgs(T container, TValue? oldValue, TValue? newValue)
        : base(container!, oldValue, newValue)
    { }
}