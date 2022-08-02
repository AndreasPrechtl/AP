using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Web
{
    public delegate void ApplicationErrorEventHandler(object sender, ApplicationErrorEventArgs e);

    public class ApplicationErrorEventArgs : ApplicationEventArgs
    {
        public ApplicationErrorEventArgs(HttpContext context, Exception exception)
            : base(context)
        {
            this.Exception = exception;
        }

        public Exception Exception { get; private set; }
    }
}
