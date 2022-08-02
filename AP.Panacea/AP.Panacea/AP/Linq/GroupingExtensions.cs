using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Linq
{
    public static class GroupingExtensions
    {
        public static AP.Collections.Dictionary<TKey, IEnumerable<TElement>> ToDictionary<TKey, TElement>(this IEnumerable<IGrouping<TKey, TElement>> grouping)
        {
            if (grouping == null)
                throw new ArgumentNullException("grouping");

            AP.Collections.Dictionary<TKey, IEnumerable<TElement>> d = new AP.Collections.Dictionary<TKey, IEnumerable<TElement>>();

            foreach (IGrouping<TKey, TElement> group in grouping)
                d.Add(group.Key, group);

            return d;
        }
    }
}
