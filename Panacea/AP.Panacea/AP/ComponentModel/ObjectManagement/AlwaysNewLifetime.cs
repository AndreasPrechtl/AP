using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace AP.ComponentModel.ObjectManagement
{
    public sealed class AlwaysNewLifetime<TBase> : ObjectLifetimeBase<TBase>
    {
        private Activator<TBase> _activator;
        
        public AlwaysNewLifetime(Activator<TBase> activator, object key = null)
            : base(key)
        {
            if (activator == null)
                throw new ArgumentNullException("activator");

            _activator = activator;
        }

        public override ManagedInstance<TBase> Instance
        {
            get { return new ManagedInstance<TBase>(_activator(), true); }
        }

        protected override void CleanUpResources()
        {
            _activator = null;
            base.CleanUpResources();
        }
    }
}
