using AP.UI;
using AP.UI.SiteMapping;
using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AP.Panacea.Web.UI
{
    public static class SiteMapExtensions
    {
        public static bool GetCurrentEntry(this AP.UI.SiteMapping.SiteMap<Request> siteMap, out SiteMapEntry<Request> entry)
        {
            return siteMap.FindEntry(new Request(new HttpUrl(HttpContext.Current.Request.Url.ToString())), out entry, true);
        }
    }
}
