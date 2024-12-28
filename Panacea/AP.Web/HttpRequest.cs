using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace AP.Web
{
    public class HttpRequest : System.Web.HttpRequestWrapper
    {
        private readonly System.Web.HttpRequest _inner;
        public System.Web.HttpRequest Inner { get { return _inner; } }
        public static HttpRequest Current { get { return HttpContext.Current.Request; } }

        public HttpRequest(System.Web.HttpRequest httpRequest)
            : base(httpRequest)
        {
            _inner = httpRequest;
        }

        public string ClientName
        {
            get
            {
                return Dns.GetHostEntry(this.ServerVariables["remote_addr"]).HostName;
            }
        }

        public IPAddress ClientIPAddress
        {
            get
            {
                string s = this.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(s))
                    s = this.ServerVariables["REMOTE_ADDR"];

                if (string.IsNullOrEmpty(s))
                    s = this.UserHostAddress;

                IPAddress ip = null;
                IPAddress.TryParse(s, out ip);

                return ip;
            }
        }

        public static implicit operator System.Web.HttpRequest(AP.Web.HttpRequest httpRequest)
        {
            return httpRequest._inner;
        }
        public static implicit operator AP.Web.HttpRequest(System.Web.HttpRequest httpRequest)
        {
            return new AP.Web.HttpRequest(httpRequest);
        }
    }
}
