using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace AP.Web.Controls
{
    [ControlBuilder(typeof(System.Web.UI.HtmlControls.HtmlEmptyTagControlBuilder))]
    public class MetaTagPlaceHolder : ContentPlaceHolder
    {
        public string Content { get; set; }
        public string HttpEquiv { get; set; }
        public string Name { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }
}
