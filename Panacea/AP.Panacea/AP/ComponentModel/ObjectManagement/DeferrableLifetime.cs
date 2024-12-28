using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Diagnostics;
using AP.Linq;

namespace AP.ComponentModel.ObjectManagement
{
    public sealed class DeferrableLifetime<TBase> : ObjectLifetimeBase<TBase>
    {
        private Deferrable<TBase> _inner;
        
        public DeferrableLifetime(TBase instance, object key = null)
            : this(new Deferrable<TBase>(instance), key)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");
        }

        public DeferrableLifetime(Activator<TBase> activator, object key = null)
            : this(new Deferrable<TBase>(activator), key)
        {
            if (activator == null)
                throw new ArgumentNullException("activator");

            _inner = activator;
        }
        
        public DeferrableLifetime(Deferrable<TBase> deferrable, object key = null)
        {
            if (deferrable == null)
                throw new ArgumentNullException("deferrable");

            _inner = deferrable;
        }

        public bool IsInstanceActive
        {
            get { return _inner.IsValueActive; }
        }

        public override ManagedInstance<TBase> Instance
        {
            get
            {
                return new ManagedInstance<TBase>(_inner.Value, false);
            }       
        }

        protected override void CleanUpResources()
        {
            base.CleanUpResources();

            if (this.IsInstanceActive)
            {
                _inner.Value.TryDispose();
                _inner = null;
            }
        }
    }
}
