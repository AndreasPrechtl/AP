using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AP.Web.Mvc
{
    public class SiteMapItemSeparator : SiteMapItemBase
    {
        public override SiteMapItemBase Clone()
        {
            return new SiteMapItemSeparator();
        }
    }
}
