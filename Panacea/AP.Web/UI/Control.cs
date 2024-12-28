using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Web.UI
{
    public class Control : System.Web.UI.Control
    {       
        public IHtmlHelper Html { get; private set; }
        
        public Control(IHtmlHelper helper = null)
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

        public HttpServerUtility Server
        {
            get { return this.Context.Server; }
        }

        public HttpApplicationState Application
        {
            get { return this.Context.Application; }
        }

        public Cache Cache
        {
            get { return this.Context.Cache; }
        }

        public HttpSessionState Session
        {
            get { return this.Context.Session; }
        }

        public HttpRequest Request
        {
            get { return this.Context.Request; }
        }

        public HttpResponse Response
        {
            get { return this.Context.Response; }
        }
    }
}
