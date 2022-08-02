using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Routing;
using System.Web;
using System.ComponentModel;

namespace AP.Web
{
    public static class FormHelper
    {        
        public static object GetFormValue(this RequestContext requestContext, string key)
        {
            return GetFormValue(requestContext.HttpContext.Request, key);
        }

        public static T GetFormValue<T>(this RequestContext requestContext, string key)
        {
            return GetFormValue<T>(requestContext.HttpContext.Request, key);
        }

        public static object GetFormValue(this HttpRequest request, string key)
        {
            return GetFormValue(new HttpRequestWrapper(request), key);
        }

        public static T GetFormValue<T>(this HttpRequest request, string key)
        {
            return GetFormValue<T>(new HttpRequestWrapper(request), key);
        }
        
        public static object GetFormValue(this HttpRequestBase request, string key)
        {
            return request.Form.GetValues(key).FirstOrDefault();
        }

        public static T GetFormValue<T>(this HttpRequestBase request, string key)
        {          
            object value = GetFormValue(request, key);
            if (value is T)
                return (T)value;
            else
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);
        }
    }
}
