using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP.ComponentModel;
using AP.Linq;

namespace AP.UniformIdentifiers
{
    public class UriParser : Singleton<UriParser>
    {
        protected UriParser()
        { }

        public virtual bool TryParse(string uri, out IUri value)
        {
            string trimmed = uri.Trim();
            value = null;

            if (trimmed.IsEmpty())
                return false;

            if (trimmed.StartsWith("/") || trimmed.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) || trimmed.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
            {
                value = new HttpUrl(uri);
                return true;
            }
            if (trimmed.StartsWith("ftp://"))
            {
                value = new FtpUrl(uri);
                return true;
            }
            if (trimmed[0] == '~')
            {
                value = new UnixUrl(uri);
                return true;
            }
            if (trimmed.StartsWith("\\") || (trimmed.Length > 1 && char.IsLetter(trimmed[0]) && trimmed[1] == ':'))
            {
                value = new Unc(uri);
                return true;
            }
            
            int queryIndex = trimmed.IndexOf('?');
            int fragmentsIndex = trimmed.IndexOf('#');
            int portIndex = trimmed.IndexOf(':');

            if (portIndex > -1 || queryIndex > -1 || fragmentsIndex > -1)
            {
                value = new HttpUrl(uri);
                return true;
            }
            
            return false;
        }

        public IUri Parse(string uri)
        {
            IUri value = null;
            if (this.TryParse(uri, out value))
                return value;

            throw new ArgumentException("uri cannot be parsed");
        }
    }
}
