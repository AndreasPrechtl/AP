using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections;
using AP;
using AP.Collections.Specialized;
using AP.Linq;

namespace AP.Configuration
{
    public abstract partial class ProviderManager<TProviderBase> : DisposableObject
        where TProviderBase : class, IProvider
    {
        private readonly NameValueDictionary<TProviderBase> _map = new NameValueDictionary<TProviderBase>();

        public readonly object SyncRoot = new object();

        public IEnumerable<TProviderBase> Providers
        {
            get
            {
                foreach (TProviderBase provider in _map.Values)
                    yield return provider;
            }
        }

        public void Register(TProviderBase provider)
        {
            _map.Add(this.CreateKey(provider.Name), provider);
        }

        private string CreateKey(string name = null)
        {
            return name ?? string.Empty;
        }

        /// <summary>
        /// Gets the provider using the name, if the name is null it gets the first one
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TProviderBase GetProvider(string name = null)
        {
            TProviderBase p = (TProviderBase)_map[this.CreateKey(name)];

            if (p != null)
                return p;

            // just get the first one
            if (name.IsNullOrWhiteSpace())
            {
                IEnumerator e = _map.Values.GetEnumerator();
                if (e.MoveNext())
                    p = (TProviderBase)e.Current;                
            }

            return p;
        }

        public void Release(string name = null, bool dispose = true)
        {
            this.Release(this.GetProvider(name), dispose);
        }

        public void Release(TProviderBase provider, bool dispose = true)
        {
            if (!provider.IsRegistered)
                throw new InvalidOperationException("Provider is not registered");

            if (provider.Manager != this)
                throw new InvalidOperationException("Provider is not registered in a different Mapper");

            _map.Remove(this.CreateKey(provider.Name));

            if (dispose)
            {
                provider.SetManager(null);
                provider.Dispose();
            }
        }

        public void Clear(bool disposeItems = true)
        {
            if (disposeItems)
            {
                foreach (IProvider value in _map.Values)
                {
                    value.SetManager(null);
                    value.Dispose();
                }                
            }
            _map.Clear();
        }

        bool _disposeProviders = true;
        public void Dispose(bool disposeProviders = true)
        {
            _disposeProviders = disposeProviders;
            base.Dispose();
        }

        protected override void CleanUpResources()
        {
            base.CleanUpResources();
            this.Clear(_disposeProviders);
        }
    }
}

