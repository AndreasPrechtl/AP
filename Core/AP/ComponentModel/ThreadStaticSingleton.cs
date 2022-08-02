using System.Reflection;
using System.Threading;
using System;
using System.Linq;

namespace AP.ComponentModel
{
    public abstract class ThreadStaticSingleton<TSingleton> : FinalizableObject
        where TSingleton : ThreadStaticSingleton<TSingleton>
    {
        public static readonly object SyncRoot = new object();

        [ThreadStatic]
        private static volatile TSingleton _current;

        protected ThreadStaticSingleton()
        {
            if (_current == null)
            {
                lock (SyncRoot)
                {
                    if (_current == null)
                        _current = (TSingleton)this;
                }
            }
            else
                throw new InvalidOperationException("MultiSingleton");
        }

        public static void Release()
        {
            _current.TryDispose();
        }

        public static TSingleton Current
        {
            get
            {
                TSingleton instance = _current;

                if (instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_current == null)
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
                if (_current == this)
                    _current = null;
        }      
    }
}