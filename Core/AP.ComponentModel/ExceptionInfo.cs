using System;

namespace AP.ComponentModel;

/// <summary>
/// Provides more detailed information about an exception for logging
/// </summary>
public record ExceptionInfo
{
    /// <summary>
    /// Gets the context where the error has occured - can be null.
    /// </summary>
    public object? Context { get; init; }

    /// <summary>
    /// Gets the username.
    /// </summary>
    public string? UserName { get; init; }

    /// <summary>
    /// Gets the timestamp.
    /// </summary>
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.Now;

    /// <summary>
    /// Gets the exception.
    /// </summary>
    public required Exception Exception { get; init; }


    public static implicit operator Exception(ExceptionInfo info)
    {
        return info.Exception;
    }
}
