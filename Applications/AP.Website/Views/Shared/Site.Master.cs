using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP.Website.Views.Shared
{
    public partial class Site : AP.Web.Mvc.ViewMasterPage
    {        
        public bool _requiresLayoutFix;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            var b = this.Page.Request.Browser;

            if (b.Browser == "IE" && b.MajorVersion <= 7)
                _requiresLayoutFix = true;
        }
    }
}