namespace AP.Configuration;

/// <summary>
/// Xaml file based settings provider.
/// </summary>
/// <typeparam name="TSettings">The settings type.</typeparam>
public class XamlSettingsProvider<TSettings> : XamlContentProvider<TSettings>
    where TSettings : ISettings
{
    /// <summary>
    /// cctor.
    /// </summary>
    /// <param name="name">The provider name.</param>
    protected XamlSettingsProvider(string? name = null)
        : base(name: name)
    { }

    /// <summary>
    /// Creates a new instance of a XamlSettingsProvider given the filename and provider name.
    /// </summary>
    /// <param name="fileName">The filename.</param>
    /// <param name="name">The provider name.</param>
    public XamlSettingsProvider(string fileName, string? name = null)
        : base(fileName, name)
    { }
}
