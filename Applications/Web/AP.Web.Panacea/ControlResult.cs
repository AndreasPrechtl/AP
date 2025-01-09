using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.UniformIdentifiers;

namespace AP.Panacea.Web
{
    public class ControlResult
    {
        public IUri Uri { get; private set; }
        public Encoding Encoding { get; private set; }

        public static Encoding DefaultEncoding = Encoding.UTF8;

        public ControlResult(IUri uri, Encoding encoding = null)
        {
            ArgumentNullException.ThrowIfNull(uri);

            this.Uri = uri;
            this.Encoding = encoding ?? DefaultEncoding;
        }
    }
}
