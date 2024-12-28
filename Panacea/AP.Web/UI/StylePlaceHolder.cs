using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP.Web.UI
{    
    public class StylePlaceHolder : ContentPlaceHolder
    {
        protected override void OnPreRender(EventArgs e)
        {
            if ((!(this.Parent is StyleCombiner) && !(this.Parent is StylePlaceHolder)) || !(base.TemplateControl is System.Web.UI.MasterPage))
                throw new HttpException("StylePlaceHolders can only be hosted in MasterPage StyleCombiners");
           
            base.OnPreRender(e);
        }
    }
}

