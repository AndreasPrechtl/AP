using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using AP.ComponentModel.ObjectManagement;
using System.Net.Mail;
using AP;
using AP.Linq;
using AP.ComponentModel;
using System.Net;

namespace AP.Web
{
    public abstract class ApplicationRunner<TApplication> : System.Web.HttpApplication
        where TApplication : AP.Web.ApplicationBase
    {    
        internal static readonly string _sessionStartKey = Guid.NewGuid().ToString();        
        private static bool _isRunning;

        public static bool IsRunning { get { return _isRunning; } }

        public ApplicationRunner()
        { }
        
        protected void Application_Start()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                this.CreateApplication().Initialize();                
            }
        }

        protected virtual TApplication CreateApplication()
        {
            return New.Instance<TApplication>();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            this.Session[_sessionStartKey] = false;            
            AP.Web.ApplicationBase.Instance.OnSessionEnded();
        }

        protected void Session_Start()
        {
            this.Session[_sessionStartKey] = true;
            AP.Web.ApplicationBase.Instance.OnSessionStarted();
        }

        protected void Application_BeginRequest()
        {
            AP.Web.ApplicationBase.Instance.OnRequestStarted();
        }

        protected void Application_EndRequest()
        {
            AP.Web.ApplicationBase.Instance.OnRequestEnded();
        }

        protected void Application_Error()
        {
            Exception exception = this.Server.GetLastError();

            if (exception != null)
                AP.Web.ApplicationBase.Instance.OnError(exception);
        }
        
        private new void Init()
        {
            base.Init();
        }
    }
}
