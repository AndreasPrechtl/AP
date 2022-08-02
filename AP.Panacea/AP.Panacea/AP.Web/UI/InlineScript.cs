using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Text;
using System.IO;
using System;

namespace AP.Web.UI
{    
    [ToolboxData("<{0}:InlineScript runat=\"server\"></{0}:InlineScript>"), ToolboxItem(true)]
    public class InlineScript : Control
    {
        protected const string _scriptStartTag = "<script type=\"text/javascript\">";
        protected const string _scriptEndTag = "</script>";

        protected override void AddParsedSubObject(object obj)
        {
            if (!(obj is LiteralControl))
                throw new HttpException("Only javascript is allowed in Scriptblocks");

            base.AddParsedSubObject(obj);
        }
        
        public override void RenderControl(HtmlTextWriter writer)
        {
            string rendered = null;
            StringBuilder sb = new StringBuilder();
            
            // render it
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    this.RenderChildren(htw);
                }
            }
            rendered = sb.ToString().Trim();

            // now remove the <script type="text/javascript"> </script> tags - if they exist
            if (rendered.StartsWith(_scriptStartTag, StringComparison.OrdinalIgnoreCase))
            {
                rendered = rendered.Substring(_scriptStartTag.Length);
                rendered = rendered.Remove(rendered.Length - _scriptEndTag.Length);
            }

            writer.BeginRender();
            if ((this.Parent is ScriptCombiner) || (this.Parent is ScriptPlaceHolder))
                writer.Write(rendered);
            else
            {
                writer.Write("<script type=\"text/javascript\"");
                if (!string.IsNullOrWhiteSpace(this.ID))
                {
                    writer.Write(" id=\"");
                    writer.Write(this.ID);
                    writer.Write("\"");
                }
                writer.Write(">");
                writer.Write(rendered);
                writer.Write("</script>");
            }
            writer.EndRender();
        }
    }
}

