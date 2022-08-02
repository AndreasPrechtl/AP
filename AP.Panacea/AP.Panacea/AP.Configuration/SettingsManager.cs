using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace AP.Configuration
{
    public class SettingsManager : ProviderManager<ISettingsProvider>
    {
        public ISettings GetSettings(string providerName = null)
        {
            return this.GetProvider(providerName).Settings;
        }

        public TSettings GetSettings<TSettings>(string providerName = null)
            where TSettings : ISettings
        {
            return (TSettings)this.GetSettings(providerName);
        }
    }
}
