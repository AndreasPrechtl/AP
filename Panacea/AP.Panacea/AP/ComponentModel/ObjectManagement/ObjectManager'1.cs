using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AP.ComponentModel.ObjectManagement
{
    public sealed partial class ObjectManager<TSuper> : DisposableObject, IObjectManager<TSuper>, IObjectManagerInternal
    {
        private ObjectManager _inner;

        public ObjectManager()
        {
            _inner = new ObjectManager(this);
            _inner.Disposing += _inner_Disposing;
        }

        void _inner_Disposing(object sender, EventArgs e)
        {
            _inner.Disposing -= _inner_Disposing;
            _inner = null;
        }

        public bool Contains<TBase>(object key = null)
            where TBase : TSuper
        {
            return _inner.Contains<TBase>(key);
        }
        
        public IEnumerable<ObjectLifetimeBase<TBase>> GetLifetimes<TBase>()
            where TBase : TSuper
        {
            return _inner.GetLifetimes<TBase>();
        }

        public IEnumerable<ManagedInstance<TBase>> GetInstances<TBase>()
            where TBase : TSuper
        {
            return _inner.GetInstances<TBase>();
        }

        public ObjectLifetimeBase<TBase> Register<TBase>(ObjectLifetimeBase<TBase> lifetime, bool disposeOnRelease = true)
            where TBase : TSuper
        {
            return _inner.Register(lifetime, disposeOnRelease);
        }

        public ObjectLifetimeBase<TBase> GetLifetime<TBase>(object key = null)
            where TBase : TSuper
        {
            return _inner.GetLifetime<TBase>(key);
        }
        
        public void Release<TBase>(object key = null)
            where TBase : TSuper
        {
            _inner.Release<TBase>(key);
        }
        
        public bool TryGetObjectLifetime<TBase>(out ObjectLifetimeBase<TBase> lifetime, object key = null) 
            where TBase : TSuper
        {
            lifetime = null;
            return _inner.TryGetLifetime<TBase>(out lifetime, key);
        }

        public ManagedInstance<TBase> GetInstance<TBase>(object key = null) 
            where TBase : TSuper
        {
            return _inner.GetInstance<TBase>(key);
        }

        public bool TryGetInstance<TBase>(out ManagedInstance<TBase> instance, object key = null) 
            where TBase : TSuper
        {
            return _inner.TryGetInstance<TBase>(out instance, key);
        }
             
        public void Clear()
        {
            _inner.Clear();
        }

        #region IObjectManager Members

        public Scope GetScope()
        {
            return new Scope(this);
        }

        IObjectManagementScope<TSuper> IObjectManager<TSuper>.GetScope()
        {
            return this.GetScope();
        }

        #endregion

        #region IDisposable Members

        protected override void CleanUpResources()
        {
            base.CleanUpResources();
            
            if (_inner != null && !_inner.IsDisposed)
                _inner.Dispose();
            
            _inner = null;
        }

        #endregion      
    }
}
