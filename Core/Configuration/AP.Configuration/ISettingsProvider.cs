namespace AP.Configuration;

/// <summary>
/// Interface for settings providers.
/// </summary>
/// <typeparam name="TSettings">The settings type.</typeparam>
public interface ISettingsProvider<out TSettings> : IContentProvider<TSettings>
    where TSettings : ISettings
{ }
