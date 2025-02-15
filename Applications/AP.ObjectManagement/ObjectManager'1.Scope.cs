﻿using System.Collections.Generic;

namespace AP.ComponentModel.ObjectManagement;

public partial class ObjectManager<TSuper>
    where TSuper : notnull
{
    public sealed class Scope : IObjectManagementScope<TSuper>
    {
        private ObjectManager<TSuper> _manager;
        private ObjectManager.Scope _inner;

        internal Scope(ObjectManager<TSuper> manager)
        {
            _manager = manager;
            _inner = manager._inner.GetScope();
        }

        #region IObjectManagementScope<TSuper> Members

        public ManagedInstance<TBase>? GetInstance<TBase>(object? key = null)
            where TBase : TSuper
            => _inner.GetInstance<TBase>(key);

        public IEnumerable<ManagedInstance<TBase>?> GetInstances<TBase>()
            where TBase : TSuper
            => _inner.GetInstances<TBase>();

        public bool TryGetInstance<TBase>(out ManagedInstance<TBase>? instance, object? key = null) 
            where TBase : TSuper
            => _inner.TryGetInstance<TBase>(out instance, key);

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _manager = null!;
            _inner.Dispose();
        }

        #endregion
    }
}
