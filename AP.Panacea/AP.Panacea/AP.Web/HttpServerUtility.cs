using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Web
{
    public class HttpServerUtility : System.Web.HttpServerUtilityWrapper
    {
        public System.Web.HttpServerUtility Inner { get { return _inner; } }
        private readonly System.Web.HttpServerUtility _inner; 
        public static HttpServerUtility Current { get { return HttpContext.Current.Server; } }

        public HttpServerUtility(System.Web.HttpServerUtility httpServerUtility)
            : base(httpServerUtility)
        {
            _inner = httpServerUtility;
        }

        public static implicit operator System.Web.HttpServerUtility(AP.Web.HttpServerUtility httpServerUtility)
        {
            return httpServerUtility._inner;
        }
        public static implicit operator AP.Web.HttpServerUtility(System.Web.HttpServerUtility httpServerUtility)
        {
            return new AP.Web.HttpServerUtility(httpServerUtility);
        }
    }
}
