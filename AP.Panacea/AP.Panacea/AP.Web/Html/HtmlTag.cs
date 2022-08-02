using AP.Web.Html;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace AP.Web.Html
{
    public partial class HtmlTag
    {
        private HtmlAttributeSet _attributes;
        private string _innerHtml;
        private string _name;

        public HtmlTag(string tagName)
        {
            this.Name = tagName;
        }

        public HtmlTag(string tagName, IEnumerable<HtmlAttribute> attributes)
        {
            this.Name = tagName;
            this.Attributes = new HtmlAttributeSet(attributes);
        }

        public HtmlTag(string tagName, object attributes)
        {
            this.Name = tagName;
            this.Attributes = new HtmlAttributeSet(attributes);
        }
        
        public HtmlAttributeSet Attributes
        {
            get
            {
                HtmlAttributeSet attributes = _attributes;

                if (attributes == null)
                    _attributes = attributes = new HtmlAttributeSet();

                return attributes;
            }
            set
            {
                _attributes = value;
            }
        }

        public string InnerHtml
        {
            get
            {
                return _innerHtml;
            }
            set
            {
                _innerHtml = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("TagName");

                _name = value.ToLower();
            }
        }

        public HtmlString ToHtmlString(HtmlTagRenderMode tagRenderMode = HtmlTagRenderMode.Normal)
        {            
            return new HtmlString(this.ToString(tagRenderMode));
        }
        
        public override string ToString()
        {
            return this.ToString(HtmlTagRenderMode.Normal);
        }

        public string ToString(HtmlTagRenderMode renderMode = HtmlTagRenderMode.Normal)
        {
            StringBuilder sb = new StringBuilder();
            switch (renderMode)
            {
                case HtmlTagRenderMode.Normal:
                    this.WriteStartTag(sb);
                    this.WriteAttributes(sb);
                    sb.Append(">");
                    this.WriteInnerHtml(sb);
                    this.WriteEndTag(sb);                    
                    break;

                case HtmlTagRenderMode.StartTag:
                    this.WriteStartTag(sb);
                    this.WriteAttributes(sb);
                    sb.Append(">");

                    // now it writes the start tag and a possible anti forgery token - so all what's left to do is close the thing or expand it even further
                    this.WriteInnerHtml(sb);
                    break;

                case HtmlTagRenderMode.EndTag:
                    this.WriteEndTag(sb);
                    break;

                case HtmlTagRenderMode.SelfClosing:
                    this.WriteStartTag(sb);
                    this.WriteAttributes(sb);
                    sb.Append("/>");
                    break;
            }
            return sb.ToString();
        }

        protected void WriteAttributes(StringBuilder sb)
        {
            foreach (HtmlAttribute attribute in this.Attributes)
            {
                if (!string.IsNullOrWhiteSpace(attribute.Name))
                {                    
                    sb.Append(" ");
                    sb.Append(attribute.Name);

                    if (attribute.Value != null && !string.IsNullOrWhiteSpace(attribute.Value.ToString()))
                    {
                        sb.Append("=\"");
                        sb.Append(attribute.Encode ? HttpUtility.HtmlAttributeEncode(attribute.Value.ToString()) : attribute.Value.ToString());
                        sb.Append("\"");
                    }
                }
            }
        }

        protected void WriteEndTag(StringBuilder sb)
        {
            sb.Append("</");
            sb.Append(_name);
            sb.Append(">");
        }

        protected void WriteInnerHtml(StringBuilder sb)
        {
            sb.Append(this.InnerHtml);
        }

        protected void WriteStartTag(StringBuilder sb)
        {
            sb.Append("<");
            sb.Append(_name);
        }
                
        public static implicit operator HtmlString(HtmlTag tag)
        {
            return tag.ToHtmlString();
        }

        public static implicit operator string(HtmlTag tag)
        {
            return tag.ToString();
        }
    }
}