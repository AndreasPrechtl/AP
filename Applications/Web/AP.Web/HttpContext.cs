using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AP.Web
{
    public class HttpContext : HttpContextWrapper
    {
        private readonly System.Web.HttpContext _inner;

        private AP.Web.HttpApplicationState _application;
        private AP.Web.Cache _cache;
        private AP.Web.HttpServerUtility _server;
        private AP.Web.HttpSessionState _session;
        private AP.Web.HttpRequest _request;
        private AP.Web.HttpResponse _response;
                
        public System.Web.HttpContext Inner { get { return _inner; } }

        public static AP.Web.HttpContext Current { get { return new HttpContext(System.Web.HttpContext.Current); } }

        public new AP.Web.ApplicationBase ApplicationInstance { get { return AP.Web.ApplicationBase.Instance; } }        
        public new AP.Web.HttpApplicationState Application 
        { 
            get 
            {
                var application = _application;

                if (application == null)
                    _application = application = new HttpApplicationState(_inner.Application);

                return application; 
            }        
        }

        public new AP.Web.Cache Cache 
        { 
            get 
            {
                var cache = _cache;

                if (cache == null)
                    _cache = cache = new Cache(_inner.Cache);

                return cache; 
            }         
        }
        
        public new AP.Web.HttpServerUtility Server 
        { 
            get 
            {
                var server = _server;

                if (server == null)
                    _server = server = new HttpServerUtility(_inner.Server);
                return server; 
            } 
        }
        
        public new AP.Web.HttpSessionState Session 
        { 
            get 
            {
                var session = _session;

                if (session == null && _inner.Session != null)
                    _session = session = new HttpSessionState(_inner.Session);
                
                return session; 
            }        
        }

        public new AP.Web.HttpRequest Request
        {
            get
            {
                var request = _request;

                if (request == null)
                    _request = request = new HttpRequest(_inner.Request);

                return request;
            }         
        }

        public new AP.Web.HttpResponse Response
        {
            get
            {
                var response = _response;

                if (response == null)
                    _response = response = new HttpResponse(_inner.Response);
                
                return response;
            }
        }
        
        public HttpContext(System.Web.HttpContext httpContext)           
            : base(httpContext)
        {            
            _inner = httpContext;
            
            //_application = new AP.Web.HttpApplicationState(httpContext.Application);
            //_cache = new AP.Web.Cache(httpContext.Cache);
            //_server = new AP.Web.HttpServerUtility(httpContext.Server);
             
            //// these might fail - sessionstate could be null and request or response might throw an exception (not available at the moment)
            //// I'd rather have a null value instead of an omnious exception when it comes to that.            
            //if (httpContext.Session != null)
            //    _session = new HttpSessionState(httpContext.Session);

            //try { _request = new AP.Web.HttpRequest(httpContext.Request); } catch { }
            //try { _response = new AP.Web.HttpResponse(httpContext.Response); } catch { }
        }

        public static implicit operator System.Web.HttpContext(AP.Web.HttpContext httpContext)
        {            
            return httpContext._inner;
        }

        public static implicit operator HttpContext(System.Web.HttpContext httpContext)
        {            
            return new HttpContext(httpContext);
        }
    }
}
