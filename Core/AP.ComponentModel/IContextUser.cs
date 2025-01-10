namespace AP.ComponentModel;

/// <summary>
/// An interface for types that use disposable objects.
/// </summary>
public interface IContextUser : AP.IContextDependentDisposable
{
    AP.IContextDependentDisposable Context { get; }
    bool OwnsContext { get; }
}

/// <summary>
/// A generic interface for types that use disposable objects.
/// </summary>
/// <typeparam name="TContext"></typeparam>
public interface IContextUser<out TContext> : IContextUser
    where TContext : notnull, AP.IContextDependentDisposable
{
    new TContext Context { get; }
}
