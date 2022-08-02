using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Xaml;
using System.ComponentModel;
using System.Runtime;

namespace AP.Xaml
{
    /// <summary>
    /// Provides generic types for XAML
    /// </summary>
    [Obsolete("Generics support was added to System.Xaml in 3.5 or 4.0")]
    public class TypeExtension : System.Windows.Markup.TypeExtension
    {
        public TypeExtension()
        { }
        public TypeExtension(string typeName)            
            : base(typeName)
        { }
        public TypeExtension(Type type)            
            : base(type)
        { }
        
        public override object ProvideValue(IServiceProvider serviceProvider)
        {               
            // I can probably remove that function, generic support has been added to xaml.
            string tn = this.TypeName;
            string tmp = tn.RemoveWhiteSpaces();
            int startBracket = tn.IndexOf("(");
            string generics = tn.Substring(startBracket + 1);
            generics = generics.Remove(generics.LastIndexOf(")"));

            int genCount = 0;
            foreach (char c in generics)
                if (c == ',')
                    genCount++;
            
            // empty generic type?
            if (genCount != generics.Length)
                return base.ProvideValue(serviceProvider);
            else
            {
                tn = tn.Remove(startBracket);
                int index = tn.IndexOf(":");
                if (index > 0)
                {
                    IXamlNamespaceResolver namespaceResolver = (IXamlNamespaceResolver)serviceProvider.GetService(typeof(IXamlNamespaceResolver));
                    string ns = namespaceResolver.GetNamespace(tn.Substring(0, index));
                    StringBuilder sb = new StringBuilder(ns);

                    int index2 = ns.IndexOf(";");
                    sb.Remove(index2, ns.Length - index2);
                    sb.Remove(0, ns.IndexOf(":") + 1);
                    sb.Append(".");
                    tn = sb.Append(tn.Substring(index + 1)).ToString();
                }
                return Type.GetType(tn + "`" + ++genCount);
            }
        }
    }
}
