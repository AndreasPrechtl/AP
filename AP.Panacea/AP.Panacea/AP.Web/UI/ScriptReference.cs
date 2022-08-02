using System.ComponentModel;
using System.Web;
using System.Web.UI;

namespace AP.Web.UI
{
    [ToolboxItem(true), ToolboxData("<{0}:ScriptReference runat=\"server\" />"), DefaultProperty("Source")]
    public sealed class ScriptReference : Control
    {
        protected override void AddParsedSubObject(object obj)
        {
            throw new HttpException("no child controls are allowed");
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            if (string.IsNullOrWhiteSpace(this.Source))
                throw new HttpException("Source is missing");
            
            if (!(this.Parent is ScriptCombiner) && !(this.Parent is ScriptPlaceHolder))
            {
                writer.Write("<script type=\"text/javascript\" src=\"");
                writer.Write(this.ResolveUrl(this.Source));

                if (!string.IsNullOrWhiteSpace(this.ID))
                {
                    writer.Write("\" id=\"");
                    writer.Write(this.ID);
                }
                writer.Write("\"></script>");
            }
        }

        public string Source { get; set; }
    }
}

