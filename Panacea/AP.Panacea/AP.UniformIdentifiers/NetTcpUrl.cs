using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.UniformIdentifiers
{
    [Serializable]
    public class NetTcpUrl : WebResourceUrlBase
    {
        protected override WebResourceUrlBase CreateEmptyInstance()
        {
            return new NetTcpUrl();
        }

        public override string Scheme
        {
            get { return System.Uri.UriSchemeNetTcp; }
        }
    }
}
