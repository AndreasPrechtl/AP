using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AP.Web.Mvc;
using AP.Web.Mvc.Routing;

namespace AP.Website
{
    public class SiteMapProvider : SiteMapProviderBase
    {
        protected override void Initialize()
        {
            base.Initialize();

            this.SiteMap = new Web.Mvc.SiteMap(this)
            {
                Controller = "Home",
                Action = "Index"                
            };

            var c = this.SiteMap.Children;
            c.Add(new SiteMapItem { Controller = "Home", Action = "References", Title = "Referenzen" });
            c.Add(new SiteMapItem { Controller = "Home", Action = "Technologies", Title = "Technologien" });
            c.Add(new SiteMapItem { Controller = "Blog", Action = Actions.Index, Title = "Blog" });
            c.Add(new SiteMapItem { Controller = "Home", Action = "Imprint", Title = "Impressum" });               
        }

        protected override SiteMapItem OnCreateSiteMapItem(System.Web.Routing.RouteValueDictionary routeValues)
        {
            return new SiteMapItem();
        }

        public override bool CanCreateSiteMapItem(System.Web.Routing.RouteValueDictionary routeValues)
        {
            return true;
        }
    }
}