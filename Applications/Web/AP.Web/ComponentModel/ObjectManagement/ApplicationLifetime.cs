using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using AP;
using AP.Linq;
using AP.ComponentModel;
using AP.ComponentModel.ObjectManagement;

namespace AP.Web.objectManagement
{
    public class ApplicationLifetime<TBase> : ObjectLifetimeBase<TBase>
    {
        private readonly string _applicationKey = Guid.NewGuid().ToString();
        protected string ApplicationKey { get { return _applicationKey; } }

        private Activator<TBase> _activator;

        public ApplicationLifetime(Activator<TBase> activator, object key = null)
            : base(key)
        {
            if (activator == null)
                throw new ArgumentNullException("activator");

            _activator = activator;
        }

        public ApplicationLifetime(TBase instance, object key = null)
            : this(() => instance, key)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            HttpContext.Current.Application[_applicationKey] = instance;
            ApplicationBase.Instance.ApplicationEnded += HandleApplicationEnded;
        }

        public override ManagedInstance<TBase> Instance
        {
            get
            {
                HttpContext current = HttpContext.Current;
                HttpApplicationState state = current.Application;
                object instance = state[_applicationKey];

                if (instance == null)
                {
                    state[_applicationKey] = instance = _activator();
                    ApplicationBase.Instance.ApplicationEnded += HandleApplicationEnded;
                }
                return new ManagedInstance<TBase>((TBase)instance, false);               
            }
        }
                
        private void HandleApplicationEnded(object sender, ApplicationEventArgs e)
        {
            HttpContext context = e.HttpContext;
            
            ApplicationBase.Instance.ApplicationEnded -= HandleApplicationEnded;
            HttpApplicationState state = context.Application;
            
            object instance = state[_applicationKey];
            
            if (instance != null)
                state.Remove(_applicationKey);

            instance.TryDispose();
        }

        protected override void CleanUpResources()
        {
            HttpContext context = HttpContext.Current;

            if (context != null)
            {
                HttpApplicationState app = HttpContext.Current.Application;
                TBase instance = (TBase)app[_applicationKey];
                app.Remove(_applicationKey);

                instance.TryDispose();
            }

            base.CleanUpResources();
        }
    }
}
