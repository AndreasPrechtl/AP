using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Web
{
    public class HttpResponse : System.Web.HttpResponseWrapper
    {
        private readonly System.Web.HttpResponse _inner;
        public System.Web.HttpResponse Inner { get { return _inner; } }
        public static HttpResponse Current { get { return HttpContext.Current.Response; } }

        public HttpResponse(System.Web.HttpResponse httpResponse)
            : base(httpResponse)
        {
            _inner = httpResponse;
        }

        public static implicit operator System.Web.HttpResponse(AP.Web.HttpResponse httpResponse)
        {
            return httpResponse._inner;
        }
        public static implicit operator AP.Web.HttpResponse(System.Web.HttpResponse httpResponse)
        {
            return new AP.Web.HttpResponse(httpResponse);
        }
    }
}
