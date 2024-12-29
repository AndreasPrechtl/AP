using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Resources;

namespace AP.Web
{
    public static class ResourcesHelper
    {
        private const string DefaultResourcesNamespace = "Resources.";
        private const string ResourcePrefix = "@";
        private const string GlobalResources = "App_GlobalResources";
        private static readonly char[] Separators = new char[] { '.' };

        private static readonly Assembly _assembly;

        static ResourcesHelper()
        {
            try // design mode
            {
                _assembly = Assembly.Load(GlobalResources);
            }
            catch { }
        }
                
        public static string GetFromResources(string s)
        {            
            if (_assembly != null && !string.IsNullOrWhiteSpace(s))
            {
                string[] components = s.Split(Separators, 3, StringSplitOptions.RemoveEmptyEntries);

                if (components.Length > 1)
                {
                    try
                    {
                        string resourcesNamespace = DefaultResourcesNamespace;
                        string typeName = components[0];
                        string propertyName = components[1];

                        if (components.Length > 2)
                        {
                            resourcesNamespace = components[0] + ".";
                            typeName = components[1];
                            propertyName = components[2];
                        }
                        return new ResourceManager(resourcesNamespace + typeName, _assembly).GetString(propertyName);
                    }
                    catch { }
                }
            }            
            return s;
        }
    }
}
