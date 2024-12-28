using System;
using System.Collections.Generic;
using System.Linq;

namespace AP.Linq;

public static class GroupingExtensions
{
    public static AP.Collections.Dictionary<TKey, IEnumerable<TElement>> ToDictionary<TKey, TElement>(this IEnumerable<IGrouping<TKey, TElement>> grouping)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(grouping);

        AP.Collections.Dictionary<TKey, IEnumerable<TElement>> d = [];

        foreach (IGrouping<TKey, TElement> group in grouping)
            d.Add(group.Key, group);

        return d;
    }
}
