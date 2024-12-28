using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Configuration
{
    /// <summary>
    /// Interface for settings providers.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    public interface ISettingsProvider<out TSettings> : IContentProvider<TSettings>
        where TSettings : ISettings
    { }
}
