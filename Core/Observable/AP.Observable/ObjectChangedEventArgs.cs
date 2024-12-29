using System;

namespace AP.Observable;

public class ObjectChangedEventArgs : EventArgs
{
    public static new readonly ObjectChangedEventArgs Empty = new();

    private readonly object _container = null!;
    private readonly object? _oldValue;
    private readonly object? _newValue;

    public object Container => _container;
    public object? OldValue => _oldValue;
    public object? NewValue => _newValue;

    private ObjectChangedEventArgs()
    { }

    public ObjectChangedEventArgs(object container, object? oldValue, object? newValue)
        : this()
    {
        ArgumentNullException.ThrowIfNull(container);

        _container = container;
        _oldValue = oldValue;
        _newValue = newValue;
    }

    public bool HasChanges => Equals(_oldValue, _newValue);
}

public class ObjectChangedEventArgs<T> : ObjectChangedEventArgs
{
    public new T Container => (T)base.Container!;

    public ObjectChangedEventArgs(T container, object? oldValue, object? newValue)
        : base(container!, oldValue, newValue)
    { }
}

public class ObjectChangedEventArgs<T, TValue> : ObjectChangedEventArgs<T>
{
    public new TValue? OldValue => (TValue?)base.OldValue;
    public new TValue? NewValue => (TValue?)base.NewValue;

    public ObjectChangedEventArgs(T container, TValue oldValue, TValue newValue)
        : base(container!, oldValue, newValue)
    { }
}