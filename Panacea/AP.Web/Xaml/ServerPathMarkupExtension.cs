using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Markup;

namespace AP.Web.Xaml
{
    [MarkupExtensionReturnType(typeof(string))]
    [ContentProperty("FileName")]
    public sealed class ServerPath : MarkupExtension
    {
        public string FileName { get; set; }

        public ServerPath(string fileName)
        {
            this.FileName = fileName;
        }

        public ServerPath()
        { }
        
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return HttpContext.Current.Server.MapPath(this.FileName);
        }
    }
}
