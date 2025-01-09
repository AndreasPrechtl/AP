using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

using AP;
using AP.Linq;
using AP.ComponentModel;
using AP.ComponentModel.ObjectManagement;

namespace AP.Web.objectManagement
{
    public class SessionLifetime<TBase> : ObjectLifetimeBase<TBase>
    {
        private readonly string _sessionKey = Guid.NewGuid().ToString();
        protected string SessionKey { get { return _sessionKey; } }
        
        private Activator<TBase> _activator;

        public SessionLifetime(Activator<TBase> activator, object key = null)
            : base(key)
        {
            ArgumentNullException.ThrowIfNull(activator);

            _activator = activator;
        }

        public override ManagedInstance<TBase> Instance
        {
            get            
            {
                HttpContext context = HttpContext.Current;
                HttpSessionState state = context.Session;
                object instance = state[_sessionKey];

                if (instance == null)
                {
                    state[_sessionKey] = instance = _activator();
                    ApplicationBase.Instance.SessionEnded += HandleSessionEnded;
                }
                
                return new ManagedInstance<TBase>((TBase)instance, false);
            }        
        }

        private void HandleSessionEnded(object sender, ApplicationEventArgs e)
        {
            HttpContext context = e.HttpContext;
            ApplicationBase.Instance.ApplicationEnded -= HandleSessionEnded;

            HttpSessionState state = context.Session;

            object instance = state[_sessionKey];

            if (instance != null)
            {
                state.Remove(_sessionKey);
                instance.TryDispose();
            }
        }

        protected override void CleanUpResources()
        {
            base.CleanUpResources();
            
            _activator = null;
            
            HttpContext context = HttpContext.Current;

            if (context == null)
                return;
                        
            HttpSessionState session = HttpContext.Current.Session;
            object instance = session[_sessionKey];
            
            if (instance != null)
            {
                session.Remove(_sessionKey);
                instance.TryDispose();
            }
        }
    }
}
