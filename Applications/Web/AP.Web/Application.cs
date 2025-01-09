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
using System.Web.Routing;

namespace AP.Web
{
    public abstract class ApplicationBase : Singleton<ApplicationBase>, IApplication
    {
        private static bool _isInitialized = false;
        private readonly Lazy<IObjectManager> _objectManager;

        protected ApplicationBase()
        {
            _objectManager = new(this.CreateObjectManager);
        }

        protected abstract IObjectManager CreateObjectManager();

        public event ApplicationEventHandler RequestEnded;
        public event ApplicationEventHandler RequestStarted;

        public event ApplicationEventHandler SessionStarted;
        public event ApplicationEventHandler SessionEnded;

        public event ApplicationEventHandler ApplicationStarted;
        public event ApplicationEventHandler ApplicationEnded;

        public event ApplicationErrorEventHandler Error;

        public HttpApplicationState State { get { return this.Context.Application; } }
        public HttpContext Context { get { return new HttpContext(System.Web.HttpContext.Current); } }

        public IObjectManager ObjectManager 
        { 
            get 
            {
                return _objectManager.Value;
            } 
        }

        protected abstract string SmtpAddress { get; }
        protected virtual ushort SmtpPort { get { return 25; } }
        protected abstract MailAddress DefaultFromEmailAddress { get; }
        protected abstract MailAddressCollection DefaultToEmailAddresses { get; }

        protected virtual ICredentialsByHost Credentials { get { return null; } }


        // fixes problems for compiling master and other pages - when they use my HttpContext
        public static implicit operator System.Web.HttpApplication(ApplicationBase application)
        {
            return System.Web.HttpContext.Current.ApplicationInstance;
        }

        protected virtual void InitializeComponents()
        {
        }

        internal void Initialize()
        {
            if (!_isInitialized)
            {
                this.InitializeComponents();
                _isInitialized = true;

                ApplicationEventHandler handler = this.ApplicationStarted;
                if (handler != null)
                    handler(this, new ApplicationEventArgs(this.Context));
            }
        }

        protected internal virtual void OnRequestStarted()
        {
            ApplicationEventHandler handler = this.RequestStarted;
            if (handler != null)
                handler(this, new ApplicationEventArgs(this.Context));
        }

        protected internal virtual void OnRequestEnded()
        {
            ApplicationEventHandler handler = this.RequestEnded;
            if (handler != null)
                handler(this, new ApplicationEventArgs(HttpContext.Current));
        }

        protected internal virtual void OnSessionEnded()
        {
            ApplicationEventHandler handler = this.SessionEnded;
            if (handler != null)
                handler(this, new ApplicationEventArgs(this.Context));
        }

        protected internal virtual void OnSessionStarted()
        {
            ApplicationEventHandler handler = this.SessionStarted;
            if (handler != null)
                handler(this, new ApplicationEventArgs(this.Context));
        }

        protected internal virtual void OnError(Exception exception)
        {
            ApplicationErrorEventHandler handler = this.Error;

            if (handler != null)
                handler(this, new ApplicationErrorEventArgs(this.Context, exception));

            if (exception is HttpException)
            {
                HttpException hex = (HttpException)exception;

                // ignore that one
                if ((HttpStatusCode)hex.GetHttpCode() == HttpStatusCode.NotFound)
                    return;
            }
            this.SendMail(this.CreateErrorMail(exception));
        }

        protected override void CleanUpResources()
        {
            base.CleanUpResources();
            ApplicationEventHandler handler = this.ApplicationEnded;

            _isInitialized = false;

            if (handler != null)
                handler(this, new ApplicationEventArgs(this.Context));

            if (_objectManager.IsValueActive)
                _objectManager.Reset();            
        }

        protected virtual MailMessage CreateErrorMail(Exception exception)
        {
            ExceptionHelper.AssertNotNull(() => exception);

            HttpRequest request = HttpRequest.Current;
            
            string body = string.Format
            (@" 
                <html> 
                    <body> 
                        <p>
                            Date / Time: {0}
                        </p>
                        <p>
                            RawUrl: {1}<br/>                            
                            UrlReferer: {2}                            
                        </p>
                        <p>
                            IP: {3}<br/>
                            ClientName: {4}<br/>
                            Browser: {5}<br/>                            
                            Identity: {6}
                        <p>
                        <p>
                            Exception: {7}
                        <p>
                    </body> 
                </html>",
                DateTimeOffset.UtcNow,
                request.RawUrl,
                request.UrlReferrer,
                request.ClientIPAddress,
                request.ClientName,
                request.UserAgent,
                this.Context.User != null ? this.Context.User.Identity.Name : string.Empty,
                exception.ToString().Replace(Environment.NewLine, "<br/>")
            );

            return new MailMessage
            {
                Body = body,
                IsBodyHtml = true,
                Priority = MailPriority.High,
                Subject = "An error occured"
            };
        }

        public void SendMail(string subject, string body, string from, string to, MailPriority priority = MailPriority.Normal, bool throwException = false)
        {
            MailMessage msg = new MailMessage
            {
                Subject = subject,
                Body = body,
                Priority = priority
            };

            if (!from.IsNullOrWhiteSpace())
                msg.From = new MailAddress(from);

            if (!to.IsNullOrWhiteSpace())
                msg.To.Add(new MailAddress(to));

            this.SendMail(msg, throwException);
        }

        public virtual void SendMail(MailMessage message, bool throwException = false)
        {
            ArgumentNullException.ThrowIfNull(message);

            if (message.From == null)
                message.From = this.DefaultFromEmailAddress;

            if (message.To.IsDefaultOrEmpty())
                message.To.Add(this.DefaultToEmailAddresses);

            using (SmtpClient smtp = new SmtpClient(this.SmtpAddress, (int)this.SmtpPort))
            {
                ICredentialsByHost credentials = this.Credentials;

                if (credentials != null)
                    smtp.Credentials = credentials;

                if (throwException)
                    smtp.Send(message);
                else
                {
                    try
                    {
                        smtp.Send(message);
                    }
                    catch { }
                }
            }
        }
    }
}