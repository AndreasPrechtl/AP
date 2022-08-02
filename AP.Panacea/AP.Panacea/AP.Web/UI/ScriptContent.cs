using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP.Web.UI
{
    [ToolboxItem(false), Designer("System.Web.UI.Design.WebControls.ContentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public class ScriptContent : Content
    {
        protected override void AddParsedSubObject(object obj)
        {
            if ((!(obj is InlineScript) && !(obj is ScriptReference)) && (!(obj is ScriptPlaceHolder) || !(base.TemplateControl is MasterPage)))
            {
                throw new HttpException("Only InlineScripts/References/PlaceHolders are allowed in ScriptContent");
            }
            base.AddParsedSubObject(obj);
        }
    }
}

