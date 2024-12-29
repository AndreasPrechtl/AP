using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Web.UI
{
    public class UserControl : System.Web.UI.UserControl
    {
        public IHtmlHelper Html { get; private set; }

        public UserControl(IHtmlHelper helper = null)
        {
            this.Html = helper ?? HtmlHelper.Instance;
        }
        
        private HttpContext _context;

        public new HttpContext Context
        {
            get
            {
                var context = _context;

                if (context == null)
                    _context = context = new HttpContext(base.Context);

                return context;
            }
        }

        public new HttpServerUtility Server
        {
            get { return this.Context.Server; }
        }

        public new HttpApplicationState Application
        {
            get { return this.Context.Application; }
        }

        public new Cache Cache
        {
            get { return this.Context.Cache; }
        }

        public new HttpSessionState Session
        {
            get { return this.Context.Session; }
        }

        public new HttpRequest Request
        {
            get { return this.Context.Request; }
        }

        public new HttpResponse Response
        {
            get { return this.Context.Response; }
        }
    }
}
