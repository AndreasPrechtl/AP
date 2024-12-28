using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace AP.ComponentModel.Logging
{
    public static class LogEntryContextHelper
    {
        
    }

    public class LogEntryContextBase
    {
        public object Context { get; private set; }

        internal LogEntryContextBase(object context)
        {
            ExceptionHelper.AssertNotNull(() => context);
            this.Context = context;
        }
    }

    public class StaticLogEntryContext : LogEntryContextBase
    {
        public Type Type { get { return (Type)base.Context; } }

        public StaticLogEntryContext(Type type)
            : base(type)
        { }
    }

    public class InstanceLogEntryContext<T> : LogEntryContextBase
    {
        public T Instance { get { return (T)base.Context; } }

        public InstanceLogEntryContext(T instance)
            : base(instance)
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
}
