using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP.Web.UI
{
    [ToolboxItem(false), Designer("System.Web.UI.Design.WebControls.ContentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public class StyleContent : Content
    {
        protected override void AddParsedSubObject(object obj)
        {
            if ((!(obj is InlineStyle) && !(obj is StyleReference)) && (!(obj is StylePlaceHolder) || !(base.TemplateControl is MasterPage)))
                throw new HttpException("Only InlineStyles/References/PlaceHolders are allowed in StyleContent");
            
            base.AddParsedSubObject(obj);
        }
    }
}

