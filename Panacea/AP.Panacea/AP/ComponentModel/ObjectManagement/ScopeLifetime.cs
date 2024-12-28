using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.ComponentModel.ObjectManagement
{
    internal interface IScopeLifetimeInternal
    { }

    public sealed class ScopeLifetime<TBase> : ObjectLifetimeBase<TBase>, IScopeLifetimeInternal
    {
        private sealed class Lookup : Dictionary<ScopeLifetime<TBase>, TBase>
        { }
        
        private Activator<TBase> _activator;
        public Activator<TBase> Activator { get { return _activator; } }
        
        public ScopeLifetime(Activator<TBase> activator, object key = null)
            : base(key)
        {
            if (activator == null)
                throw new ArgumentNullException("activator");

            _activator = activator;
        }

        public override ManagedInstance<TBase> Instance
        {
            get 
            {
                return new ManagedInstance<TBase>(_activator(), false);            
            }
        }

        protected override void CleanUpResources()
        {
            _activator = null;
            base.CleanUpResources();
        }
    }
}
