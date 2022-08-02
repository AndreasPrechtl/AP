using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AP.ComponentModel.ObjectManagement;
using AP.Configuration;
using AP.Security;
using AP.UI;
using AP.UniformIdentifiers;
using AP.UI.SiteMapping;
using AP.Routing;

namespace AP.Panacea.Web
{
    public abstract class ApplicationBase : AP.Web.ApplicationBase, AP.Panacea.IApplication<Request, Response>, IResponseRenderer, IResponseRenderer<Response>, INavigator<Request, Response>
    {
        private readonly Deferrable<ApplicationCore> _core;
        private readonly Deferrable<ResponseRenderer> _renderer;
        private readonly Deferrable<Navigator<Request, Response>> _navigator;

        protected ApplicationBase()
        {
            _core = new Deferrable<ApplicationCore>(this.CreateCore);
            _renderer = new Deferrable<ResponseRenderer>(this.CreateRenderer);
            _navigator = new Deferrable<Navigator<Request, Response>>(this.CreateNavigator);
        }

        protected virtual ResponseRenderer CreateRenderer()
        {
            return new ResponseRenderer();
        }

        private Navigator<Request, Response> CreateNavigator()
        {
            return new Navigator<Request, Response>(this, true);
        }

        protected virtual ApplicationCore CreateCore()
        {
            return new ApplicationCore(this);
        }

        protected ApplicationCore Core { get { return _core.Value; } }        
        protected ResponseRenderer Renderer { get { return _renderer.Value; } }        
        public AP.Routing.RouteTable<Request> RouteTable { get { return this.Core.RouteTable; } }
        protected Navigator<Request, Response> Navigator { get { return _navigator.Value; } }

        protected sealed override IObjectManager CreateObjectManager()
        {
            return this.Core.ObjectManager;
        }
        
        #region IApplication Members

        public static new ApplicationBase Instance 
        { 
            get 
            { 
                return (AP.Panacea.Web.ApplicationBase)AP.Web.ApplicationBase.Instance; 
            } 
        }

        public new IObjectManager ObjectManager
        {
            get { return this.Core.ObjectManager; }
        }

        public AP.UI.SiteMapping.SiteMap<Request> SiteMap
        {
            get { return this.Core.SiteMap; }
        }

        public MembershipContextBase Membership
        {
            get { return this.Core.Membership; }
        }

        public Response GetResponse(Request request)
        {
            return this.Core.GetResponse(request);
        }

        public bool IsAuthorized(Request request)
        {
            return this.Core.IsAuthorized(request);
        }
                     
        /// <summary>
        /// Creates a Panacea Request instance using a HttpContext.
        /// </summary>
        /// <param name="context">The http context.</param>
        /// <returns>The request.</returns>
        public virtual Request CreateRequest(AP.Web.HttpContext context)
        {
            HttpRequest req = context.Request;

            HttpUrl uri = new HttpUrl(req.Url.ToString());
            HttpUrl referrer = req.UrlReferrer != null ? new HttpUrl(req.UrlReferrer.ToString()) : null;

            User user = this.Core.Membership != null ? new User(context.User.Identity.Name) : null;
            
            // build the new request
            return new Request(uri, context, referrer, user, null, new ClientAgent(req.Browser.Id, req.Browser.Version));
        }

        #endregion

        #region INavigator Members

        public void Navigate(HttpContext context, bool updateCurrentUri = true)
        {
            this.Navigate(this.CreateRequest(context), updateCurrentUri);
        }

        public virtual void Navigate(Request request, bool updateCurrentUri = true)
        {
            this.Navigator.Navigate(request, updateCurrentUri);
        }

        protected override void OnRequestStarted()
        {
            HttpContext ctx = HttpContext.Current;

            Request request = this.CreateRequest(ctx);
            Response response = this.GetResponse(request);

            System.Uri u1 = ctx.Request.Url;
            IUri u2 = response.Uri;

            // use the default httpHandler
            if (response.Type == ResponseType.RouteNotFound)
            {
                base.OnRequestStarted();
                return;
            }

            this.Render(response, ctx.Response);
            
            // this is important, otherwise the access will be denied.
            ctx.Response.End();

            base.OnRequestStarted();            
        }
        
        #endregion

        #region IResponseRenderer<Response, AP.Web.HttpResponse> Members

        public void Render(Response response, AP.Web.HttpResponse renderTarget = null)
        {
            this.Renderer.Render(response, renderTarget);
        }

        #endregion

        #region IResponseRenderer<Response> Members

        void IResponseRenderer<Response>.Render(Response response)
        {
            this.Render(response, null);
        }

        #endregion

        public event NavigatingEventHandler<Request, Response> Navigating 
        {
            add { this.Navigator.Navigating += value; }
            remove { this.Navigator.Navigating -= value; }
        }

        public event NavigatedEventHandler<Request, Response> Navigated
        {
            add { this.Navigator.Navigated += value; }
            remove { this.Navigator.Navigated -= value; }
        }
        
        public IUri CurrentUri
        {
            get { return new HttpUrl(HttpContext.Current.Request.Url.ToString()); }
        }

        ApplicationCore<Request, Response> IApplication<Request, Response>.Core
        {
            get { return this.Core; }
        }

        public IUri GetUri(Request request, bool testAuthorization = true)
        {
            return this.Core.GetUri(request, testAuthorization);
        }

        public User CurrentUser
        {
            get 
            {
                if (this.Core.Membership != null)
                {
                    User user = new User(HttpContext.Current.User.Identity.Name);

                    //if (user.Exists)
                        return user;
                }

                return null;
            }
        }

        public ClientAgent CurrentClientAgent
        {
            get 
            {
                return new ClientAgent(HttpContext.Current.Request.UserAgent, HttpContext.Current.Request.Browser.MajorVersion.ToString());
            }
        }
    }
}
