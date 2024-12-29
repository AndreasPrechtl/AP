using System.Net.Mime;
namespace AP.Panacea.Web
{
    public sealed class Css
    {
        public string Value { get; private set; }

        public static readonly string ContentType = "text/css";
        
        public Css(string value)
        {
            this.Value = value;
        }
    }
}