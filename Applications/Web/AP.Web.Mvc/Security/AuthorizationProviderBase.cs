using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using AP.Web.Mvc;
using System.Collections.Generic;
using AP.Web.Mvc.Routing;
using AP.Collections;
using AP.Configuration;
using System.Web.Routing;

namespace AP.Web.Mvc.Security
{    
    public abstract class AuthorizationProviderBase : ProviderBase
    {
        public abstract AuthorizationDescriptorCollection<AreaAuthorizationDescriptor> LocalAreas { get; }   
    
        /// <summary>
        /// Override this method for custom authorization logic
        /// </summary>
        /// <returns>returns false if the user could not be authorized</returns>
        protected internal virtual bool GetCustomAuthorizationResult(ref bool requireHttps, IPrincipal user, string action = null, string controller = null, RouteValueDictionary routeValues = null, string area = null)
        {
            return true;
        }        

        // that could be some custom implementation
        //public static bool IsAuthorizedOnSiteMap(string action, string controller)
        //{
        //    IPrincipal user = HttpContext.Current.User;

        //    SiteMapItem siteMapItem = SiteMapProviderBase.SiteMap.IndexedItems.FirstOrDefault(p => p.Controller == controller && p.Action == action);

        //    if (siteMapItem == null)
        //        return true;

        //    if (siteMapItem.Users.Contains(user.Identity.Name))
        //        return true;

        //    if (siteMapItem.Roles.Any(p => user.IsInRole(p)))
        //        return true;

        //    return false;
        //}        
    }
}