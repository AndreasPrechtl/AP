using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AP.Web
{
    public delegate void ApplicationEventHandler(object sender, ApplicationEventArgs e);

    public class ApplicationEventArgs
    {
        public ApplicationEventArgs(HttpContext context)
        {
            this.HttpContext = context;
        }

        public HttpContext HttpContext { get; private set; }
    }
}
