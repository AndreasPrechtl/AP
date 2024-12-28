using System;
using System.Threading;

namespace AP.ComponentModel;

/// <summary>
/// Provides more detailed information about an exception for logging
/// </summary>
[Serializable]
public class ExceptionInfo
{
    /// <summary>
    /// Gets the context where the error has occured - can be null.
    /// </summary>
    public object Context { get; private set; }

    /// <summary>
    /// Gets the username.
    /// </summary>
    public string UserName { get; private set; }

    /// <summary>
    /// Gets the timestamp.
    /// </summary>
    public DateTimeOffset Timestamp { get; private set; }

    /// <summary>
    /// Gets the exception.
    /// </summary>
    public Exception Exception { get; private set; }

    /// <summary>
    /// Creates a new ExceptionInfo.
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="timestamp"></param>
    /// <param name="userName"></param>
    public ExceptionInfo(Exception exception, DateTimeOffset? timestamp = null, string? userName = null)
        : this(null, exception, timestamp, userName)
    { }

    /// <summary>
    /// Creates a new ExceptionInfo.
    /// </summary>
    /// <param name="context">The context where the exception occured - can be null.</param>
    /// <param name="exception">The exception.</param>
    /// <param name="timestamp">The timestamp.</param>
    /// <param name="userName">The username.</param>
    public ExceptionInfo(object context, Exception exception, DateTimeOffset? timestamp = null, string? userName = null)            
    {
        ArgumentNullException.ThrowIfNull(exception);

        this.Exception = exception;
        this.Context = context;
        this.Timestamp = timestamp ?? DateTimeOffset.UtcNow;
        this.UserName = userName ?? Thread.CurrentPrincipal.Identity.Name;
    }

    public static implicit operator Exception(ExceptionInfo info)
    {
        return info.Exception;
    }
}

/// <summary>
/// Provides additional details to an exception.
/// </summary>
/// <typeparam name="TException"></typeparam>
[Serializable]
public class ExceptionInfo<TException> : ExceptionInfo
    where TException : Exception
{
    /// <summary>
    /// Gets the exception.
    /// </summary>
    public new TException Exception => (TException)base.Exception;

    /// <summary>
    /// Creates a new ExceptionInfo.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="timestamp">The timestamp.</param>
    /// <param name="userName">The username.</param>
    public ExceptionInfo(TException exception, DateTimeOffset? timestamp = null, string? userName = null)
        : base(exception, timestamp, userName)
    { }
    
    /// <summary>
    /// Creates a new ExceptionInfo.
    /// </summary>
    /// <param name="context">The context where the exception occured.</param>
    /// <param name="exception">The exception.</param>
    /// <param name="timestamp">The timestamp.</param>
    /// <param name="userName">The username.</param>
    public ExceptionInfo(object context, TException exception, DateTimeOffset? timestamp = null, string? userName = null)
        : base(context, exception, timestamp, userName)
    { }

    public static implicit operator TException(ExceptionInfo<TException> info)
    {
        return info.Exception;
    }
}
