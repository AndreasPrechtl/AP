using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.HtmlControls;

namespace AP.Web.UI
{
    public static class PageHelper
    {
        public static void EnsureHeaderExistsOnInit(System.Web.UI.Page page, EventArgs e)
        {            
            if (page.Header == null)
            {
                HtmlHead head = new HtmlHead();
                head.Page = page;
                typeof(HtmlHead).InvokeMember("OnInit", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.NonPublic, null, head, new[] { e });
            }
        }
    }
}
