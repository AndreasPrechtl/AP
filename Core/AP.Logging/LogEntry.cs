using System;
using System.Security.Principal;

namespace AP.Logging;

public static class LogEntryContextHelper
{

}

public class LogEntryContextBase
{
    public object Context { get; private set; }

    internal LogEntryContextBase(object context)
    {
        ArgumentNullException.ThrowIfNull(context);
        Context = context;
    }
}

public class StaticLogEntryContext : LogEntryContextBase
{
    public Type Type => (Type)Context;

    public StaticLogEntryContext(Type type)
        : base(type)
    { }
}

public class LogEntry
{
    public DateTimeOffset Timestamp { get; private set; }
    public string Message { get; private set; }
    public IPrincipal User { get; private set; }

    /// <summary>
    /// Contains optional information about the object that ordered the log entry
    /// </summary>
    public object Context { get; private set; }

    /// <summary>
    /// probably not needed
    /// </summary>
    public int Code { get; private set; }
}


public class ErrorLogEntry : LogEntry
{
    public Exception Exception { get; private set; }
}
