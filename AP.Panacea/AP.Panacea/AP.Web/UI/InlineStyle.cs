using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Text;
using System;

namespace AP.Web.UI
{
    [ToolboxData("<{0}:InlineStyle runat=\"server\"></{0}:InlineStyle>"), ToolboxItem(true)]
    public class InlineStyle : Control
    {
        protected const string _styleStartTag = "<style type=\"text/css\">";
        protected const string _styleEndTag = "</style>";

        protected override void AddParsedSubObject(object obj)
        {
            if (!(obj is LiteralControl))
                throw new HttpException("Only stylesheet code is allowed in InlineStyles");

            base.AddParsedSubObject(obj);
        }

        public override void RenderControl(HtmlTextWriter writer)
        {            
            string rendered = null;
            StringBuilder sb = new StringBuilder();
            
            // pre render it
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    this.RenderChildren(htw);
                }
            }
            rendered = sb.ToString().Trim();

            // now remove the <style> </style> tags - if they exist
            if (rendered.StartsWith(_styleStartTag, StringComparison.OrdinalIgnoreCase))
            {
                rendered = rendered.Substring(_styleStartTag.Length);
                rendered = rendered.Remove(rendered.Length - _styleEndTag.Length);
            }

            writer.BeginRender();
            if ((this.Parent is StyleCombiner) || (this.Parent is StylePlaceHolder))
                writer.Write(rendered);
            else
            {
                writer.Write("<style type=\"text/css\"");                
                if (!string.IsNullOrWhiteSpace(this.ID))
                {
                    writer.Write(" id=\"");
                    writer.Write(this.ID);
                    writer.Write("\"");
                }
                writer.Write(">");
                writer.Write(rendered);
                writer.Write("</style>");
            }
            writer.EndRender();
        }
    }
}

