using AP.Collections;
using System;
using System.Collections.Generic;
using AP.Collections.Specialized;

namespace AP.Linq;

public static class StringEnumerable
{
    public const string DefaultSeparator = ",";

    public static bool Contains(this IEnumerable<string> source, string value, StringComparison comparisonType = StringComparison.Ordinal)
    {
        if (source is IStringEnumerable enumerable)
            return enumerable.Contains(value);

        return StringEnumerableHelperInternal.Contains(source, value, comparisonType);
    }

    /// <summary>
    /// Combines a collection of strings into a single string
    /// </summary>
    /// <param name="source">the collection</param>
    /// <param name="separator">the separator - default = ","</param>
    /// <returns></returns>
    public static string ToString(this IEnumerable<string> source, string separator = DefaultSeparator, bool filterWhitespaces = true)
    {
        if (source is IStringEnumerable enumerable)
            return enumerable.ToString(separator, filterWhitespaces);

        return StringEnumerableHelperInternal.ToString(source, separator, filterWhitespaces);
    }        

    public static StringList ToCollection(this string value, string separator = DefaultSeparator)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(separator);
        return new StringList(value, separator);
    }

    public static StringSet ToSet(this string value, string separator = DefaultSeparator)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(separator);
        return new StringSet(value, separator);
    }
}
