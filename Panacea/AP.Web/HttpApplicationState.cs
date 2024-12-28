using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AP.Web
{
    public class HttpApplicationState : HttpApplicationStateWrapper
    {
        public AP.Web.ApplicationBase Instance { get { return AP.Web.ApplicationBase.Instance; } }
        public System.Web.HttpApplicationState Inner { get { return _inner; } }

        private readonly System.Web.HttpApplicationState _inner;

        public HttpApplicationState(System.Web.HttpApplicationState httpApplicationState)
            : base(httpApplicationState)
        {
            _inner = httpApplicationState;
        }

        public static implicit operator System.Web.HttpApplicationState(AP.Web.HttpApplicationState httpApplicationState)
        {
            return httpApplicationState._inner;
        }
        public static implicit operator AP.Web.HttpApplicationState(System.Web.HttpApplicationState httpApplicationState)
        {
            return new HttpApplicationState(httpApplicationState);
        }
    }
}
