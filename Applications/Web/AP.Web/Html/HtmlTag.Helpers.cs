using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AP.Web.Html
{
    public partial class HtmlTag
    {
        public static HtmlTag Form(string title, string url, Method method = Method.Post, object attributes = null)
        {
            HtmlTag f = new HtmlTag("form");

            f.Attributes.Add(new HtmlAttribute("title", title));
            f.Attributes.Add(new HtmlAttribute("method", method == Method.Post ? "post" : "get"));
            f.Attributes.Add(new HtmlAttributeSet(attributes), false);

            return f;
        }

        public static HtmlTag NavigationLink(string title, string url, object attributes = null)
        {
            HtmlTag a = new HtmlTag("a");
            
            a.Attributes.Add(new HtmlAttribute("href", url));
            a.Attributes.Add(new HtmlAttributeSet(attributes), false);
            a.InnerHtml = HttpUtility.HtmlEncode(title);

            return a;
        }

        public static HtmlTag Submit(string title, object attributes = null)
        {
            HtmlTag submit = new HtmlTag("input", attributes);

            submit.Attributes.Add(new HtmlAttribute("type", "submit"), true);
            submit.Attributes.Add(new HtmlAttribute("title", title), true);
            submit.Attributes.Add(new HtmlAttribute("value", title), true);
            submit.Attributes.Add(new HtmlAttributeSet(attributes), false);

            return submit;
        }
    }
}
