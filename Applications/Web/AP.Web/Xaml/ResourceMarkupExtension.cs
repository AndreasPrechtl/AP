using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Resources;

namespace AP.Web.Xaml
{
    [MarkupExtensionReturnType(typeof(string))]
    [ContentProperty("HandlerUrl")]
    public sealed class Resource : MarkupExtension
    {        
        public string ResourceKey { get; set; }

        public Resource(string resourceKey)
        {
            this.ResourceKey = resourceKey;
        }

        public Resource()
        { }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {       
            return ResourcesHelper.GetFromResources(this.ResourceKey);
        }
    }
}
