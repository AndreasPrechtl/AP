using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using AP.Configuration;
using AP.ComponentModel;
using System.Threading;
using AP.ComponentModel.ObjectManagement;
using AP.Routing;
using AP.Security;
using AP.UniformIdentifiers;
using AP.UI.SiteMapping;
using AP.UI;

namespace AP.Panacea
{
    public class ApplicationCore<TRequest, TResponse> : DisposableObject
        where TRequest : Request
        where TResponse : Response
    {
        private MembershipContextBase _membershipContext;        
        private IObjectManager _objectManager;
        private Router<TRequest> _router;
        private IRequestFilter<TRequest> _requestLogger;
        private IResponseFilter<TResponse> _responseLogger;
        private SiteMap<TRequest> _siteMap;
        private IApplication<TRequest, TResponse> _application;
                
        /// <summary>
        /// Creates a new ApplicationCore instance.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="objectManager">The object manager.</param>
        /// <param name="router">The router.</param>
        /// <param name="siteMap">The sitemap.</param>
        /// <param name="membershipContext">The membership context.</param>
        /// <param name="requestLogger">The request logger.</param>
        /// <param name="responseLogger">The response logger.</param>
        public ApplicationCore(IApplication<TRequest, TResponse> application, IObjectManager objectManager = null, Router<TRequest> router = null, SiteMap<TRequest> siteMap = null, AP.Security.MembershipContextBase membershipContext = null, IRequestFilter<TRequest> requestLogger = null, IResponseFilter<TResponse> responseLogger = null)
        {
            if (application == null)
                throw new ArgumentNullException("application");

            _application = application;
            _objectManager = objectManager ?? ManagedObjects.Manager;
            _membershipContext = membershipContext;

            _router = router ?? new Router<TRequest>(new RouteTable<TRequest>());
            _requestLogger = requestLogger;
            _responseLogger = responseLogger;
            _siteMap = siteMap;
        }
        
        /// <summary>
        /// Gets the object manager (IoC).
        /// </summary>
        public IObjectManager ObjectManager
        {
            get { return _objectManager; }
        }

        /// <summary>
        /// Gets the sitemap.
        /// </summary>
        public SiteMap<TRequest> SiteMap
        {
            get { return _siteMap; }
        }

        /// <summary>
        /// Gets the membership context.
        /// </summary>
        public MembershipContextBase Membership
        {
            get { return _membershipContext; }
        }
        
        /// <summary>
        /// Used to initialize the request and add missing values.
        /// </summary>
        /// <param name="request">The request.</param>
        public void InitializeRequest(TRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            request.User = request.User = request.User ?? _application.CurrentUser;
            request.Referrer = request.Referrer ?? _application.CurrentUri;
            request.ClientAgent = request.ClientAgent ?? _application.CurrentClientAgent;
        }

        /// <summary>
        /// Logs the request.
        /// </summary>
        /// <param name="request">The request.</param>
        private void LogRequest(TRequest request)
        {
            IRequestFilter<TRequest> logger = _requestLogger;

            if (logger != null)
                logger.Filter(request);
        }

        /// <summary>
        /// Logs the response.
        /// </summary>
        /// <param name="response">The response.</param>
        private void LogResponse(TResponse response)
        {
            IResponseFilter<TResponse> logger = _responseLogger;

            if (logger != null)
                logger.Filter(response);
        }

        /// <summary>
        /// Tests if a request is authorized.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Returns true if the request is authorized.</returns>
        public bool IsAuthorized(TRequest request)
        {
            IUri uri = this.GetUri(request, false);

            return uri != null || this.IsAuthorized(request.User, uri);                
        }
        
        /// <summary>
        /// Gets the uri to a request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="testAuthorization">Flag if authorization for that uri should be tested.</param>
        /// <returns>The uri (authorized) or null (unauthorized, route not found, route blocked).</returns>
        public IUri GetUri(TRequest request, bool testAuthorization = true)
        {
            IUri uri = _router.GetUri(request);

            if (uri != null && (!testAuthorization || this.IsAuthorized(request.User, uri)))
                return uri;

            return null;
        }

        /// <summary>
        /// Checks if the user is authorized.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="uri">The uri.</param>
        /// <returns></returns>
        private bool IsAuthorized(User user, IUri uri)
        {
            return _membershipContext == null || _membershipContext.IsAuthorized(user.Name, uri.FullName);
        }

        /// <summary>
        /// Gets the response to a request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        public TResponse GetResponse(TRequest request)
        {
            this.InitializeRequest(request);            

            this.LogRequest(request);

            TResponse response = null;
            RoutingResult<TRequest> result = _router.GetResult(request);

            if (result == null || result.Type == ResultType.NoMatch)
                response = this.HandleRouteNotFound(request);
            
            if (result.Type == ResultType.Denied)
                response = this.HandleRouteBlocked(request, result.Uri);

            if (!this.IsAuthorized(request.User, result.Uri))
                response = this.HandleUnauthorizedRequest(request, result.Uri);

            if (response == null)
                response = this.HandleAuthorizedRequest(request, result.Value, result.Uri);

            this.LogResponse(response);

            return response;
        }

        /// <summary>
        /// Creates an authorized response.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual TResponse HandleAuthorizedRequest(TRequest request, object result, IUri uri)
        {
            return New.Instance<TResponse>(ResponseType.Authorized, result, uri, request, null, null);
        }
        
        /// <summary>
        /// Creates a response for unauthorized requests.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected virtual TResponse HandleUnauthorizedRequest(TRequest request, IUri uri)
        {
            return New.Instance<TResponse>(ResponseType.Unauthorized, null, uri, request, null, null);
        }

        /// <summary>
        /// Creates a response for requests that use a blocked route.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected virtual TResponse HandleRouteBlocked(TRequest request, IUri uri)
        {
            return New.Instance<TResponse>(ResponseType.RouteBlocked, null, uri, request, null, null);
        }

        /// <summary>
        /// Creates a response for requests that couldn't be routed.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual TResponse HandleRouteNotFound(TRequest request)
        {
            return New.Instance<TResponse>(ResponseType.RouteNotFound, null, null, request, null, null);
        }

        /// <summary>
        /// Gets the route table.
        /// </summary>
        public RouteTable<TRequest> RouteTable { get { return _router.Routes; } }       
        
        #region IDisposable Members

        protected override void CleanUpResources()
        {
            base.CleanUpResources();

            _membershipContext.TryDispose();
            _membershipContext = null;

            _objectManager.TryDispose();
            _objectManager = null;

            _router.TryDispose();
            _router = null;

            _siteMap.TryDispose();
            _siteMap = null;

            _requestLogger.TryDispose();
            _requestLogger = null;

            _responseLogger.TryDispose();
            _responseLogger = null;
        }

        #endregion            
    }
}
