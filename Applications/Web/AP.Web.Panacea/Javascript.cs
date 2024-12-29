using System.Net.Mime;

namespace AP.Panacea.Web
{
    public sealed class Javascript
    {
        public string Value { get; private set; }
        public static readonly string ContentType = "application/javascript";
        
        public Javascript(string value)
        {
            this.Value = value;
        }
    }
}