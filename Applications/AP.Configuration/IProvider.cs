namespace AP.Configuration;

/// <summary>
/// Interface for providers.
/// </summary>
public interface IProvider : AP.IContextDependentDisposable
{
    string Name { get; }
}
