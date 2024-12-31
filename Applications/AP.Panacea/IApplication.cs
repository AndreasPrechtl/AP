using AP.ComponentModel.ObjectManagement;
using AP.Configuration;
using AP.Routing;
using AP.Security;
using AP.UI.SiteMapping;
using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Panacea
{
    public interface IApplication<TRequest, TResponse> : INavigator<TRequest, TResponse>, AP.IDisposable        
        where TRequest : Request
        where TResponse : Response
    {
        /// <summary>
        /// Gets the ApplicationCore.
        /// </summary>
        /// <remarks>Should be implemented explicitly.</remarks>
        ApplicationCore<TRequest, TResponse> Core { get; }

        IObjectManager ObjectManager { get; }
        MembershipContextBase Membership { get; }
        SiteMap<TRequest> SiteMap { get; }
        RouteTable<TRequest> RouteTable { get; }

        TResponse GetResponse(TRequest request);
        IUri GetUri(TRequest request, bool testAuthorization = true);
        bool IsAuthorized(TRequest request);
        
        User CurrentUser { get; }        
        ClientAgent CurrentClientAgent { get; }
    }
}
