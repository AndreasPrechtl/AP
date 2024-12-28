using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AP.Web
{
    public class HttpSessionState : HttpSessionStateWrapper
    {
        public System.Web.SessionState.HttpSessionState Inner { get { return _inner; } }
        private readonly System.Web.SessionState.HttpSessionState _inner;
        public static HttpSessionState Current { get { return HttpContext.Current.Session; } }

        public HttpSessionState(System.Web.SessionState.HttpSessionState httpSessionState)
            : base(httpSessionState)
        {
            _inner = httpSessionState;
        }

        public static implicit operator System.Web.SessionState.HttpSessionState(AP.Web.HttpSessionState httpSessionState)
        {
            return httpSessionState._inner;
        }
        public static implicit operator AP.Web.HttpSessionState(System.Web.SessionState.HttpSessionState httpSessionState)
        {
            return new AP.Web.HttpSessionState(httpSessionState);
        }
    }
}
