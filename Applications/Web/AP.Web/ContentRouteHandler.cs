using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;

namespace AP.Web
{
    public class ContentRouteHandler : IRouteHandler
    {
        private readonly string _path;
        public ContentRouteHandler(string path)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(path);
            _path = path;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {   
            requestContext.HttpContext.RewritePath(_path);

            return new DefaultHttpHandler();
        }
    }
}
