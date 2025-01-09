using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Collections;
using AP;
using AP.Linq;
using AP.ComponentModel;
using AP.ComponentModel.ObjectManagement;

namespace AP.Web.objectManagement
{
    public class RequestLifetime<TBase> : ObjectLifetimeBase<TBase>
    {
        // to be changed because each and every new request needs to have that
        private readonly string _requestKey = Guid.NewGuid().ToString();        
        private Activator<TBase> _activator;
                
        public RequestLifetime(Activator<TBase> activator, object key = null)
            : base(key)
        {
            ArgumentNullException.ThrowIfNull(activator);

            _activator = activator;

            ApplicationBase.Instance.RequestEnded += HandleRequestEnded;
        }

        public override ManagedInstance<TBase> Instance
        {
            get
            {
                object instance = HttpContext.Current.Items[_requestKey];

                if (instance == null)
                {                   
                    HttpContext context = HttpContext.Current;
                    context.Items[_requestKey] = instance = _activator();
                }

                return new ManagedInstance<TBase>((TBase)instance);                
            }
        }
        
        private void HandleRequestEnded(object sender, ApplicationEventArgs e)
        {
            HttpContext context = e.HttpContext;
            IDictionary items = context.Items;

            object instance = items[_requestKey];

            if (instance != null)
            {
                items.Remove(_requestKey);
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
            
            IDictionary requestItems = HttpContext.Current.Items;
            object instance = requestItems[_requestKey];

            ApplicationBase.Instance.RequestEnded -= this.HandleRequestEnded;

            if (instance != null)
            {
                requestItems.Remove(_requestKey);
                instance.TryDispose();
            }

            base.CleanUpResources();
        }
    }
}
