using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.ComponentModel;

namespace AP.Configuration
{
    // do I need those? for unit testing? nah - it's not something that needs mocking
    
    public interface IProviderManager : IDisposable
    {
        IProvider Register(IProvider provider);
        IProvider GetProvider(string name = null);
        
        void Release(IProvider provider, bool dispose = true);
        void Release(string name, bool dispose = true);
        IEnumerable<IProvider> Providers { get; }
        void Clear(bool disposeProviders = true);        
    }

    public interface IProviderManager<TProviderBase> : IDisposable //, IProviderMapper
        where TProviderBase : class, IProvider
    {
        TProviderBase Register(TProviderBase provider);
        TProviderBase GetProvider(string name = null);
        void Release(TProviderBase provider, bool dispose = true);
        void Release(string name = null, bool dispose = true);
        IEnumerable<TProviderBase> Providers { get; }
    }
}
