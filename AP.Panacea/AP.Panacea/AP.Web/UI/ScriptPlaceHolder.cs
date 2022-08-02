using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP.Web.UI
{    
    public class ScriptPlaceHolder : ContentPlaceHolder
    {
        protected override void OnPreRender(EventArgs e)
        {
            if ((!(this.Parent is ScriptCombiner) && !(this.Parent is ScriptPlaceHolder)) || !(base.TemplateControl is System.Web.UI.MasterPage))
            {
                throw new HttpException("ScriptPlaceHolders can only be hosted in MasterPage ScriptCombiners");
            }
            base.OnPreRender(e);
        }
    }
}

