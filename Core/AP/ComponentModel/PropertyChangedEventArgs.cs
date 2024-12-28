namespace AP.ComponentModel;

public class PropertyChangedEventArgs : System.ComponentModel.PropertyChangedEventArgs
{
    public object? NewValue { get; private set; }
    public object? OldValue { get; private set; }

    public PropertyChangedEventArgs(string propertyName, object newValue, object oldValue)
        : base(propertyName)
    {
        this.NewValue = newValue;
        this.OldValue = oldValue;
    }
}

public class PropertyChangingEventArgs : System.ComponentModel.PropertyChangingEventArgs
{
    public object? NewValue { get; private set; }
    public object? CurrentValue { get; private set; }

    public PropertyChangingEventArgs(string propertyName, object currentValue, object newValue)
        : base(propertyName)
    {
        this.NewValue = newValue;
        this.CurrentValue = currentValue;
    }
}

public class PropertyChangingEventArgs<T> : PropertyChangingEventArgs
{
    public new T NewValue => (T)base.NewValue;
    public new T CurrentValue => (T)base.CurrentValue;

    public PropertyChangingEventArgs(string propertyName, T currentValue, T newValue)
        : base(propertyName, currentValue, newValue)
    { }
}

public class PropertyChangedEventArgs<T> : PropertyChangedEventArgs
{
    public new T NewValue => (T)base.NewValue;
    public new T OldValue => (T)base.OldValue;

    public PropertyChangedEventArgs(string propertyName, T newValue, T oldValue)
        : base(propertyName, newValue, oldValue)
    { }
}
