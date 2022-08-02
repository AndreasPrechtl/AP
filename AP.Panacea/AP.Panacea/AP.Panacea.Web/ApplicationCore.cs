using AP.ComponentModel.ObjectManagement;
using AP.Routing;
using AP.Security;
using AP.UI.SiteMapping;
using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Panacea.Web
{
    public class ApplicationCore : AP.Panacea.ApplicationCore<Request, Response>    
    {
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
        public ApplicationCore(ApplicationBase application, IObjectManager objectManager = null, Router<Request> router = null, SiteMap<Request> siteMap = null, MembershipContextBase membershipContext = null, IRequestFilter<Request> requestLogger = null, IResponseFilter<Response> responseLogger = null)
            : base(application, objectManager, router, siteMap, membershipContext, requestLogger, responseLogger)
        { }
   
        protected override Response HandleAuthorizedRequest(Request request, object result, IUri uri)
        {
            return new Response(ResponseType.Authorized, result, uri, request, null, null, null);
        }

        protected override Response HandleRouteBlocked(Request request, IUri uri)
        {
            return new Response(ResponseType.RouteBlocked, null, uri, request, null, null, null);
        }

        protected override Response HandleRouteNotFound(Request request)
        {
            return new Response(ResponseType.RouteNotFound, null, null, request, null, null, null);
        }

        protected override Response HandleUnauthorizedRequest(Request request, IUri uri)
        {
            return new Response(ResponseType.Unauthorized, null, uri, request, null, null, null);
        }
    }
}
