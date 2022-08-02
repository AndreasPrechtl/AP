using System.ComponentModel;
using System.Web;
using System.Web.UI;

namespace AP.Web.UI
{
    [ToolboxItem(true), ToolboxData("<{0}:StyleReference runat=\"server\" />"), DefaultProperty("Source")]
    public sealed class StyleReference : Control
    {
        protected override void AddParsedSubObject(object obj)
        {
            throw new HttpException("No child controls are allowed");
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            if (string.IsNullOrWhiteSpace(this.Source))
                throw new HttpException("Source is missing");

            if (!(this.Parent is StyleCombiner) && !(this.Parent is StylePlaceHolder))
            {
                writer.Write("<link rel=\"stylesheet\" type=\"text/css\" href=\"");
                writer.Write(this.ResolveUrl(this.Source));

                if (!(string.IsNullOrWhiteSpace(this.ID)))
                {
                    writer.Write("\" id=\"");
                    writer.Write(this.ID);                    
                }
                writer.Write("\"/>");
            }
        }

        public string Source { get; set; }
    }
}

