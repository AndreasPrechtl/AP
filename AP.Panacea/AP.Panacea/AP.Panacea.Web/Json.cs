using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace AP.Panacea.Web
{
    public sealed class Json
    {
        public string Value { get; private set; }
        public Encoding Encoding { get; private set; }
        
        public static readonly string ContentType = "application/json";
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;

        public Json(string value, Encoding encoding = null)
        {
            this.Value = value;
            this.Encoding = encoding ?? DefaultEncoding;
        }
    }
}
