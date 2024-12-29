namespace AP.Configuration;

/// <summary>
/// Interface for ContentProviders.
/// </summary>
/// <typeparam name="TContent">The content type.</typeparam>
public interface IContentProvider<out TContent> : IProvider
{
    TContent Content { get; }
}
