using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Collections;
using AP.ComponentModel.Conversion;

namespace AP.UniformIdentifiers
{
    public interface IQueryableUri : IUri
    {
        UrlQuery Query { get; }
    }

    //internal static class HttpQueryParametersHelper
    //{
    //    public static QueryParametersBase FromString(string queryString)
    //    {}

    //    public static string ToString(QueryParametersBase queryParameters)
    //    {        
    //        StringBuilder sb = new StringBuilder();

    //        if (queryParameters.Count == 0)
    //            return string.Empty;

    //        sb.Append("?");

    //        foreach (KeyValuePair<string, string> kvp in queryParameters)
    //        {
    //            sb.Append(kvp.Key);
    //            sb.Append("=");
    //            sb.Append(kvp.FullName);
    //            sb.Append("&");
    //        }

    //        sb.Remove(sb.Length - 1, 1);
    //    }
    //}
}
