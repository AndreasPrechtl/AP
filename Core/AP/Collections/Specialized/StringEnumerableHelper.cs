using AP.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AP.Collections.Specialized
{
    internal static class StringEnumerableHelperInternal
    {
        internal static string ToString(IEnumerable<string> collection, string separator = ",", bool filterWhitespaces = true)
        {
            StringBuilder sb = new StringBuilder();

            int count = 0;
            foreach (string s in collection)
            {
                if (filterWhitespaces && s.IsNullOrWhiteSpace())
                    continue;

                sb.Append(s);
                sb.Append(separator);
                ++count;
            }

            if (count > 0)
                sb.Remove(sb.Length - separator.Length);

            return sb.ToString();
        }


        internal static bool Contains(IEnumerable<string> source, string value, StringComparison comparisonType)
        {
            foreach (string s in source)
            {
                if (s.Equals(value, comparisonType))
                    return true;
            }
            return false;
        }
    }
}
