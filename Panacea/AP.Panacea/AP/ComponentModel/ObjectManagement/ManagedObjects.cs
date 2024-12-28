using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.ComponentModel.ObjectManagement
{
    public abstract class ManagedObjects : StaticType
    {
        private static IObjectManager _current;
        public static readonly object SyncRoot = new object();
        
        protected ManagedObjects()
            : base()
        { }

        public static IObjectManager Manager
        {
            get
            {
                return _current;
            }
            set
            {
                lock (SyncRoot)
                {
                    if (HasManager)
                        throw new InvalidOperationException("Manager is already set, use dispose first");

                    _current = value;
                }
            }
        }

        public static bool HasManager
        {
            get
            {
                lock (SyncRoot)
                    return _current != null;
            }
        }

        public static void Dispose(bool disposeManager = true)
        {
            lock (SyncRoot)
            {
                IObjectManager current = _current;
                _current = null;

                if (disposeManager)
                    current.Dispose();
            }
        }


        public static ObjectLifetimeBase<TBase> Register<TBase>(ObjectLifetimeBase<TBase> lifetime, bool disposeOnRelease = true)
        {
            return Manager.Register<TBase>(lifetime, disposeOnRelease);
        }

        public static ManagedInstance<TBase> GetInstance<TBase>(object key = null)
        {
            return Manager.GetInstance<TBase>(key);
        }

        public static bool TryGetInstance<TBase>(out ManagedInstance<TBase> instance, object key = null)
        {
            return Manager.TryGetInstance<TBase>(out instance, key);
        }

        public static IObjectManagementScope GetScope()
        {
            return Manager.GetScope();
        }

        public static ObjectLifetimeBase<TBase> GetObjectLifetime<TBase>(object key = null)
        {
            return Manager.GetLifetime<TBase>(key);
        }
        
        public static bool Contains<TBase>(object key = null)
        {
            return Manager.Contains<TBase>(key);
        }

        public static bool TryGetObjectLifetime<TBase>(out ObjectLifetimeBase<TBase> lifetime, object key = null)
        {
            return Manager.TryGetLifetime<TBase>(out lifetime, key);
        }

        public static void Release<TBase>(ObjectLifetimeBase<TBase> lifetime)
        {
            Manager.Release<TBase>(lifetime);
        }
        
        public static void Release<TBase>(object key = null)
        {
            Manager.Release<TBase>(key);
        }

        public static void Clear()
        {
            Manager.Clear();
        }
    }
}
