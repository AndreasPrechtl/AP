using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using AP.Web.Mvc;

namespace AP.Website.Html
{
    public static class MenuHelper 
    {
        public static HtmlString CreateMenu(this System.Web.Mvc.HtmlHelper helper)
        {
            var root = App.SiteMaps.RootSiteMap;
            var children = root.Children;
            var current = root.CurrentLocalItem;
            
            // a quick overview how this thing will look like
    //        <td><a class="CurrentMenuEntry" href="/Home/Index"><span>Home</span></a></td>
    //        <td><a class="MenuEntry" href="/Home/References"><span>Referenzen</span></a></td>
    //        <td><a class="MenuEntry" href="/Home/Technologies"><span>Technologien</span></a></td>                                                                
    //        <td><a class="MenuEntry" href="/Blog/Index"><span>Blog</span></a></td>   
    //        <td><a class="MenuEntry" href="/Home/Imprint"><span>Impressum</span></a></td>

            StringBuilder sb = new StringBuilder();

            CreateMenuEntry(sb, current, root);
            foreach (SiteMapItem child in children)
                CreateMenuEntry(sb, current, child);         
            
            return new MvcHtmlString(sb.ToString());
        }

        private static void CreateMenuEntry(StringBuilder sb, SiteMapItem current, SiteMapItem child)
        {
            TagBuilder td = new TagBuilder("td");
            TagBuilder a = new TagBuilder("a");
            TagBuilder span = new TagBuilder("span");

            span.InnerHtml = child.Title;

            if (current == child)
                a.AddCssClass("CurrentMenuEntry");
            else
                a.AddCssClass("MenuEntry");

            a.Attributes.Add("href", child.Url);

            a.InnerHtml = span.ToString(TagRenderMode.Normal);
            td.InnerHtml = a.ToString(TagRenderMode.Normal);

            sb.Append(td);
        }
    }
}