using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.ComponentModel.ObjectManagement
{
    public partial class ObjectManager
    {
        public sealed class Scope : IObjectManagementScope
        {
            private ObjectManager _manager;
            private Dictionary<IScopeLifetimeInternal, object> _cache;
            
            private readonly object SyncRoot = new object();

            internal Scope(ObjectManager manager)
            {
                _manager = manager;
                _cache = new Dictionary<IScopeLifetimeInternal, object>();
            }

            #region IObjectManagementScope Members

            public ManagedInstance<TBase> GetInstance<TBase>(object key = null)
            {
                ObjectLifetimeBase<TBase> lifetime = _manager.GetLifetime<TBase>(key);
                ManagedInstance<TBase> instance = lifetime.Instance;

                if (lifetime is IScopeLifetimeInternal)
                    lock (SyncRoot)
                        _cache.Add((IScopeLifetimeInternal)lifetime, instance.Value);                   
                
                return instance;                   
            }

            public IEnumerable<ManagedInstance<TBase>> GetInstances<TBase>()
            {       
                foreach (ObjectLifetimeBase<TBase> lifetime in _manager.GetLifetimes<TBase>())
                {
                    object instance;

                    if (lifetime is IScopeLifetimeInternal)
                    { 
                        IScopeLifetimeInternal scoped = (IScopeLifetimeInternal)lifetime;
                        
                        lock (SyncRoot)
                        {
                            // look into the cache and add it if it's not yet there.
                            if (!_cache.TryGetValue(scoped, out instance))
                                _cache.Add(scoped, instance = lifetime.Instance.Value);
                        }                    
                    }                    
                }
                return _manager.GetInstances<TBase>();
            }
            
            public bool TryGetInstance<TBase>(out ManagedInstance<TBase> instance, object key = null)
            {
                return _manager.TryGetInstance<TBase>(out instance, key);
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                lock (SyncRoot)
                foreach (object instance in _cache.Values)
                    instance.TryDispose();
                
                _manager = null;                
                _cache = null;
            }

            #endregion
        }
    }
}
