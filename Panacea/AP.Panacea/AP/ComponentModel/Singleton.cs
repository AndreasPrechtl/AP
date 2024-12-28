using System.Reflection;
using System.Threading;
using System;
using System.Linq;

namespace AP.ComponentModel
{
    public abstract class Singleton<TSingleton> : FinalizableObject
        where TSingleton : Singleton<TSingleton>
    {
        public static readonly object SyncRoot = new object();

        private static volatile TSingleton _instance;
   
        protected Singleton()
        {
            if (_instance == null)
            {
                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = (TSingleton)this;
                }
            }
            else
                throw new InvalidOperationException("MultiSingleton");
        }

        public static void Release()
        {
            _instance.TryDispose();
        }

        public static TSingleton Instance
        {
            get
            {
                TSingleton instance = _instance;

                if (instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            return New.Instance<TSingleton>();
                    }
                }

                return instance;
            }
        }
        protected override void CleanUpResources()
        {
            base.CleanUpResources();

            lock (SyncRoot)
                if (_instance == this)
                    _instance = null;
        }
    }
}