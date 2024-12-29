using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Web.Mvc
{
    /// <summary>
    /// A SiteMapItem that's not being displayed but can be used for fallback values
    /// </summary>
    public sealed class SiteMapItemTemplate : SiteMapItem
    {
        public SiteMapItemTemplate()
        {
            this.Action = string.Empty;
            this.RouteValues = new object();
            this.IsVisible = false;
        }

        public override string Title
        {
            get
            {
                return _title;
            }
            set
            {
                base.Title = value;
            }
        }
    }
}