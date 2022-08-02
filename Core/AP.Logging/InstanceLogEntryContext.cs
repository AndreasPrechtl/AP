namespace AP.Logging
{
    public class InstanceLogEntryContext<T> : LogEntryContextBase
    {
        public T Instance { get { return (T)Context; } }

        public InstanceLogEntryContext(T instance)
            : base(instance)
        { }
    }
}
