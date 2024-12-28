namespace AP.Logging;

public class InstanceLogEntryContext<T> : LogEntryContextBase
{
    public T Instance => (T)Context;

    public InstanceLogEntryContext(T instance)
        : base(instance)
    { }
}
