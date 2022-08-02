using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AP.Linq
{
    public static class Enumerable
    {
        [MethodImpl(256)]
        public static TSource Aggregate<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
        {
            return System.Linq.Enumerable.Aggregate<TSource>(source, func);
        }
        
        [MethodImpl(256)]
        public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
        {
            return System.Linq.Enumerable.Aggregate<TSource, TAccumulate>(source, seed, func);
        }
        
        [MethodImpl(256)]
        public static TResult Aggregate<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
        {
            return System.Linq.Enumerable.Aggregate<TSource, TAccumulate, TResult>(source, seed, func, resultSelector);
        }
        
        [MethodImpl(256)]
        public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return System.Linq.Enumerable.All<TSource>(source, predicate);
        }
        
        [MethodImpl(256)]
        public static bool Any<TSource>(this IEnumerable<TSource> source)
        {
            return System.Linq.Enumerable.Any<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return System.Linq.Enumerable.Any<TSource>(source, predicate);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> AsEnumerable<TSource>(this IEnumerable<TSource> source)
        {
            return source;
        }
        
        [MethodImpl(256)]
        public static decimal Average(this IEnumerable<decimal> source)
        {
            return System.Linq.Enumerable.Average(source);
        }
        
        [MethodImpl(256)]
        public static double Average(this IEnumerable<double> source)
        {
            return System.Linq.Enumerable.Average(source);
        }
        
        [MethodImpl(256)]
        public static double Average(this IEnumerable<int> source)
        {
            return System.Linq.Enumerable.Average(source);
        }
        
        [MethodImpl(256)]
        public static double Average(this IEnumerable<long> source)
        {
            return System.Linq.Enumerable.Average(source);
        }
        
        [MethodImpl(256)]
        public static double? Average(this IEnumerable<int?> source)
        {
            return System.Linq.Enumerable.Average(source);
        }
        
        [MethodImpl(256)]
        public static float Average(this IEnumerable<float> source)
        {
            return System.Linq.Enumerable.Average(source);
        }
        
        [MethodImpl(256)]
        public static decimal? Average(this IEnumerable<decimal?> source)
        {
            return System.Linq.Enumerable.Average(source);
        }
        
        [MethodImpl(256)]
        public static double? Average(this IEnumerable<double?> source)
        {
            return System.Linq.Enumerable.Average(source);
        }
        
        [MethodImpl(256)]
        public static double? Average(this IEnumerable<long?> source)
        {
            return System.Linq.Enumerable.Average(source);
        }
        
        [MethodImpl(256)]
        public static float? Average(this IEnumerable<float?> source)
        {
            return System.Linq.Enumerable.Average(source);
        }
        
        [MethodImpl(256)]
        public static decimal Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return System.Linq.Enumerable.Average<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return System.Linq.Enumerable.Average<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return System.Linq.Enumerable.Average<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return System.Linq.Enumerable.Average<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static float Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return System.Linq.Enumerable.Average<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static decimal? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return System.Linq.Enumerable.Average<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return System.Linq.Enumerable.Average<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return System.Linq.Enumerable.Average<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return System.Linq.Enumerable.Average<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static float? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return System.Linq.Enumerable.Average<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> Cast<TResult>(this IEnumerable source)
        {
            return System.Linq.Enumerable.Cast<TResult>(source);
        }

        [MethodImpl(256)]
        public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return System.Linq.Enumerable.Concat<TSource>(first, second);
        }
        
        [MethodImpl(256)]
        public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            return System.Linq.Enumerable.Contains<TSource>(source, value);
        }
        
        [MethodImpl(256)]
        public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            return System.Linq.Enumerable.Contains<TSource>(source, value, comparer);
        }
        
        [MethodImpl(256)]
        public static int Count<TSource>(this IEnumerable<TSource> source)
        {
            int c;

            if (EnumerableExtensions.HasCount<TSource>(source, out c))
                return c;

            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                while (enumerator.MoveNext())
                    c++;

            return c;        
        }
        
        [MethodImpl(256)]
        public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return System.Linq.Enumerable.Count<TSource>(source, predicate);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return System.Linq.Enumerable.DefaultIfEmpty<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
        {
            return System.Linq.Enumerable.DefaultIfEmpty<TSource>(source, defaultValue);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source)
        {
            return System.Linq.Enumerable.Distinct<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            return System.Linq.Enumerable.Distinct<TSource>(source, comparer);
        }
        
        [MethodImpl(256)]
        public static TSource ElementAt<TSource>(this IEnumerable<TSource> source, int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");

            if (source == null)
                throw new ArgumentNullException("source");

            if (source is System.Collections.Generic.IList<TSource>)
                return ((System.Collections.Generic.IList<TSource>)source)[index];

            if (source is AP.Collections.IListView<TSource>)
                return ((AP.Collections.IListView<TSource>)source)[index];
            
            TSource current;
            
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                Label_0036:
                if (!enumerator.MoveNext())
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                if (index == 0)
                {
                    current = enumerator.Current;
                }
                else
                {
                    index--;
                    goto Label_0036;
                }
            }
            return current;
        }
        
        [MethodImpl(256)]
        public static TSource ElementAtOrDefault<TSource>(this IEnumerable<TSource> source, int index)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            
            if (index >= 0)
            {
                IList<TSource> list;
                AP.Collections.IListView<TSource> view;

                if ((list = source as IList<TSource>) != null && index < list.Count)
                    return list[index];
                else if ((view = source as AP.Collections.IListView<TSource>) != null && index < view.Count)
                    return view[index];
                else
                {
                    using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                        while (enumerator.MoveNext())
                            if (index-- == 0)
                                return enumerator.Current;
                }
            }

            return default(TSource);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> Empty<TResult>()
        {
            return System.Linq.Enumerable.Empty<TResult>();
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return System.Linq.Enumerable.Except<TSource>(first, second);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return System.Linq.Enumerable.Except<TSource>(first, second, comparer);
        }
        
        [MethodImpl(256)]
        public static TSource First<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            
            IList<TSource> list;
            AP.Collections.IListView<TSource> view;

            if ((list = source as IList<TSource>) != null && list.Count > 0)            
                return list[0];            
            else if ((view = source as AP.Collections.IListView<TSource>) != null && view.Count > 0)
                return view[0];
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                    if (enumerator.MoveNext())
                        return enumerator.Current;

            }
            throw new ArgumentException("no elements");
        }
        
        [MethodImpl(256)]
        public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return System.Linq.Enumerable.First<TSource>(source, predicate);
        }
        
        [MethodImpl(256)]
        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            
            IList<TSource> list;
            AP.Collections.IListView<TSource> view;

            if ((list = source as IList<TSource>) != null && list.Count > 0)
                return list[0];
            else if ((view = source as AP.Collections.IListView<TSource>) != null && view.Count > 0)
                return list[0];
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                    if (enumerator.MoveNext())
                        return enumerator.Current;
            }

            return default(TSource);
        }
        
        [MethodImpl(256)]
        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {            
            return System.Linq.Enumerable.FirstOrDefault<TSource>(source, predicate);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return System.Linq.Enumerable.GroupBy<TSource, TKey>(source, keySelector);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return System.Linq.Enumerable.GroupBy<TSource, TKey>(source, keySelector, comparer);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return System.Linq.Enumerable.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
        {
            return System.Linq.Enumerable.GroupBy<TSource, TKey, TResult>(source, keySelector, resultSelector);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            return System.Linq.Enumerable.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        {
            return System.Linq.Enumerable.GroupBy<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return System.Linq.Enumerable.GroupBy<TSource, TKey, TResult>(source, keySelector, resultSelector, comparer);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return System.Linq.Enumerable.GroupBy<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, comparer);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
        {
            return System.Linq.Enumerable.GroupJoin<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return System.Linq.Enumerable.GroupJoin<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return System.Linq.Enumerable.Intersect<TSource>(first, second);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return System.Linq.Enumerable.Intersect<TSource>(first, second, comparer);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
        {
            return System.Linq.Enumerable.Join<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return System.Linq.Enumerable.Join<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }
        
        [MethodImpl(256)]
        public static TSource Last<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
        
            System.Collections.Generic.IList<TSource> list;
            AP.Collections.IListView<TSource> view;

            if ((list = source as IList<TSource>) != null)
            {
                int c = list.Count;
                if (c > 0)
                    return list[c - 1];                
            }
            else if ((view = source as AP.Collections.IListView<TSource>) != null)
            {
                int c = view.Count;
                if (c > 0)
                    return view[c - 1];
            }
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        TSource current;
                        do
                        {
                            current = enumerator.Current;
                        }
                        while (enumerator.MoveNext());
                        return current;
                    }
                }
            }
            
            throw new ArgumentException("no elements");
        }
        
        [MethodImpl(256)]
        public static TSource Last<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return System.Linq.Enumerable.Last<TSource>(source, predicate);
        }
        
        [MethodImpl(256)]
        public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            
            System.Collections.Generic.IList<TSource> list;
            AP.Collections.IListView<TSource> view;

            if ((list = source as IList<TSource>) != null)
            {
                int c = list.Count;
                if (c > 0)
                    return list[c - 1];                
            }
            else if ((view = source as AP.Collections.IListView<TSource>) != null)
            {
                int c = view.Count;
                if (c > 0)
                    return view[c - 1];
            }
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        TSource current;
                        do
                        {
                            current = enumerator.Current;
                        }
                        while (enumerator.MoveNext());
                        return current;
                    }
                }
            }
            return default(TSource);
        }
        
        [MethodImpl(256)]
        public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {            
            return System.Linq.Enumerable.LastOrDefault<TSource>(source, predicate);
        }
        
        [MethodImpl(256)]
        public static long LongCount<TSource>(this IEnumerable<TSource> source)
        {
            return System.Linq.Enumerable.LongCount<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static long LongCount<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return System.Linq.Enumerable.LongCount<TSource>(source, predicate);
        }
        
        [MethodImpl(256)]
        public static decimal Max(this IEnumerable<decimal> source)
        {
            return System.Linq.Enumerable.Max(source);
        }
        
        [MethodImpl(256)]
        public static double Max(this IEnumerable<double> source)
        {
            return System.Linq.Enumerable.Max(source);
        }
        
        [MethodImpl(256)]
        public static int Max(this IEnumerable<int> source)
        {
            return System.Linq.Enumerable.Max(source);
        }
        
        [MethodImpl(256)]
        public static long Max(this IEnumerable<long> source)
        {
            return System.Linq.Enumerable.Max(source);
        }
        
        [MethodImpl(256)]
        public static decimal? Max(this IEnumerable<decimal?> source)
        {
            return System.Linq.Enumerable.Max(source);
        }
        
        [MethodImpl(256)]
        public static float Max(this IEnumerable<float> source)
        {
            return System.Linq.Enumerable.Max(source);
        }
        
        [MethodImpl(256)]
        public static double? Max(this IEnumerable<double?> source)
        {
            return System.Linq.Enumerable.Max(source);
        }
        
        [MethodImpl(256)]
        public static int? Max(this IEnumerable<int?> source)
        {
            return System.Linq.Enumerable.Max(source);
        }
        
        [MethodImpl(256)]
        public static long? Max(this IEnumerable<long?> source)
        {
            return System.Linq.Enumerable.Max(source);
        }
        
        [MethodImpl(256)]
        public static float? Max(this IEnumerable<float?> source)
        {
            return System.Linq.Enumerable.Max(source);
        }
        
        [MethodImpl(256)]
        public static TSource Max<TSource>(this IEnumerable<TSource> source)
        {
            return System.Linq.Enumerable.Max<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static decimal Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return System.Linq.Enumerable.Max<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static double Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return System.Linq.Enumerable.Max<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static int Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return System.Linq.Enumerable.Max<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static long Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return System.Linq.Enumerable.Max<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static decimal? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return System.Linq.Enumerable.Max<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static double? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return System.Linq.Enumerable.Max<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static int? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return System.Linq.Enumerable.Max<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static float? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return System.Linq.Enumerable.Max<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static float Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return System.Linq.Enumerable.Max<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static long? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return System.Linq.Enumerable.Max<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static TResult Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return System.Linq.Enumerable.Max<TSource, TResult>(source, selector);
        }
        
        [MethodImpl(256)]
        public static decimal Min(this IEnumerable<decimal> source)
        {
            return System.Linq.Enumerable.Min(source);
        }
        
        [MethodImpl(256)]
        public static double Min(this IEnumerable<double> source)
        {
            return System.Linq.Enumerable.Min(source);
        }
        
        [MethodImpl(256)]
        public static int Min(this IEnumerable<int> source)
        {
            return System.Linq.Enumerable.Min(source);
        }
        
        [MethodImpl(256)]
        public static long Min(this IEnumerable<long> source)
        {
            return System.Linq.Enumerable.Min(source);
        }
        
        [MethodImpl(256)]
        public static decimal? Min(this IEnumerable<decimal?> source)
        {
            return System.Linq.Enumerable.Min(source);
        }
        
        [MethodImpl(256)]
        public static long? Min(this IEnumerable<long?> source)
        {
            return System.Linq.Enumerable.Min(source);
        }
        
        [MethodImpl(256)]
        public static float Min(this IEnumerable<float> source)
        { 
            return System.Linq.Enumerable.Min(source); 
        }
        
        [MethodImpl(256)]
        public static double? Min(this IEnumerable<double?> source)
        {
            return System.Linq.Enumerable.Min(source);
        }
        
        [MethodImpl(256)]
        public static int? Min(this IEnumerable<int?> source)
        {
            return System.Linq.Enumerable.Min(source);
        }
        
        [MethodImpl(256)]
        public static float? Min(this IEnumerable<float?> source)
        {
            return System.Linq.Enumerable.Min(source);
        }
        
        [MethodImpl(256)]
        public static TSource Min<TSource>(this IEnumerable<TSource> source)
        {
            return System.Linq.Enumerable.Min<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static decimal Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return System.Linq.Enumerable.Min<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static double Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return System.Linq.Enumerable.Min<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static int Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return System.Linq.Enumerable.Min<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static long Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return System.Linq.Enumerable.Min<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static decimal? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return System.Linq.Enumerable.Min<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static double? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return System.Linq.Enumerable.Min<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static int? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return System.Linq.Enumerable.Min<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static long? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return System.Linq.Enumerable.Min<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static float? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return System.Linq.Enumerable.Min<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static float Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return System.Linq.Enumerable.Min<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static TResult Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return System.Linq.Enumerable.Min<TSource, TResult>(source, selector);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> OfType<TResult>(this IEnumerable source)
        {
            return System.Linq.Enumerable.OfType<TResult>(source);
        }
        
        [MethodImpl(256)]
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return System.Linq.Enumerable.OrderBy<TSource, TKey>(source, keySelector);
        }
        
        [MethodImpl(256)]
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            return System.Linq.Enumerable.OrderBy<TSource, TKey>(source, keySelector, comparer);
        }
        
        [MethodImpl(256)]
        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return System.Linq.Enumerable.OrderByDescending<TSource, TKey>(source, keySelector);
        }
        
        [MethodImpl(256)]
        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            return System.Linq.Enumerable.OrderByDescending<TSource, TKey>(source, keySelector, comparer);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<int> Range(int start, int count)
        {
            return System.Linq.Enumerable.Range(start, count);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> Repeat<TResult>(TResult element, int count)
        {
            return System.Linq.Enumerable.Repeat<TResult>(element, count);
        }

        [MethodImpl(256)]
        public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source)
        {
            return System.Linq.Enumerable.Reverse<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return System.Linq.Enumerable.Select<TSource, TResult>(source, selector);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
        {
            return System.Linq.Enumerable.Select<TSource, TResult>(source, selector);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            return System.Linq.Enumerable.SelectMany<TSource, TResult>(source, selector);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
        {
            return System.Linq.Enumerable.SelectMany<TSource, TResult>(source, selector);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            return System.Linq.Enumerable.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            return System.Linq.Enumerable.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }
        
        [MethodImpl(256)]
        public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return System.Linq.Enumerable.SequenceEqual<TSource>(first, second);
        }
        
        [MethodImpl(256)]
        public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return System.Linq.Enumerable.SequenceEqual<TSource>(first, second, comparer);
        }
        
        [MethodImpl(256)]
        public static TSource Single<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)            
                throw new ArgumentNullException("source");

            IList<TSource> list;
            AP.Collections.IListView<TSource> view;
            if ((list = source as IList<TSource>) != null)
            {
                switch (list.Count)
                {
                    case 0:
                        throw new ArgumentException("no elements");                    
                    case 1:
                        return list[0];
                }
            }
            else if ((view = source as AP.Collections.IListView<TSource>) != null)
            {
                switch (view.Count)
                {
                    case 0:
                        throw new ArgumentException("no elements");
                    case 1:
                        return view[0];
                }
            }
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                {
                    if (!enumerator.MoveNext())
                        throw new ArgumentException("no elements");

                    TSource current = enumerator.Current;
                    if (!enumerator.MoveNext())
                        return current;                    
                }
            }
            throw new ArgumentException("more than one element");
        }
        
        [MethodImpl(256)]
        public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return System.Linq.Enumerable.Single<TSource>(source, predicate);
        }
        
        [MethodImpl(256)]
        public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            IList<TSource> list;
            AP.Collections.IListView<TSource> view;

            if ((list = source as IList<TSource>) != null)
            {
                switch (list.Count)
                {
                    case 0:
                        return default(TSource);                    
                    case 1:
                        return list[0];
                }
            }
            else if ((view = source as AP.Collections.IListView<TSource>) != null)
            {
                switch (view.Count)
                {
                    case 0:
                        return default(TSource);
                    case 1:
                        return view[0];
                }
            }
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                {
                    if (!enumerator.MoveNext())
                    {
                        return default(TSource);
                    }
                    TSource current = enumerator.Current;
                    if (!enumerator.MoveNext())
                    {
                        return current;
                    }
                }
            }

            throw new ArgumentException("more than one element");
        }
        
        [MethodImpl(256)]
        public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return System.Linq.Enumerable.SingleOrDefault<TSource>(source, predicate);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> source, int count)
        {
            return System.Linq.Enumerable.Skip<TSource>(source, count);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return System.Linq.Enumerable.SkipWhile<TSource>(source, predicate);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            return System.Linq.Enumerable.SkipWhile<TSource>(source, predicate);
        }
        
        [MethodImpl(256)]
        public static decimal Sum(this IEnumerable<decimal> source)
        {
            return System.Linq.Enumerable.Sum(source);
        }
        
        [MethodImpl(256)]
        public static double Sum(this IEnumerable<double> source)
        {
            return System.Linq.Enumerable.Sum(source);
        }
        
        [MethodImpl(256)]
        public static int Sum(this IEnumerable<int> source)
        {
            return System.Linq.Enumerable.Sum(source);
        }
        
        [MethodImpl(256)]
        public static long Sum(this IEnumerable<long> source)
        {
            return System.Linq.Enumerable.Sum(source);
        }
        
        [MethodImpl(256)]
        public static decimal? Sum(this IEnumerable<decimal?> source)
        {
            return System.Linq.Enumerable.Sum(source);
        }
        
        [MethodImpl(256)]
        public static double? Sum(this IEnumerable<double?> source)
        {
            return System.Linq.Enumerable.Sum(source);
        }
        
        [MethodImpl(256)]
        public static int? Sum(this IEnumerable<int?> source)
        {
            return System.Linq.Enumerable.Sum(source);
        }
        
        [MethodImpl(256)]
        public static long? Sum(this IEnumerable<long?> source)
        {
            return System.Linq.Enumerable.Sum(source);
        }
        
        [MethodImpl(256)]
        public static float Sum(this IEnumerable<float> source)
        {
            return System.Linq.Enumerable.Sum(source);
        }
        
        [MethodImpl(256)]
        public static float? Sum(this IEnumerable<float?> source)
        {
            return System.Linq.Enumerable.Sum(source);
        }
        
        [MethodImpl(256)]
        public static decimal Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return System.Linq.Enumerable.Sum<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static double Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return System.Linq.Enumerable.Sum<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return System.Linq.Enumerable.Sum<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static long Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return System.Linq.Enumerable.Sum<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static float Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return System.Linq.Enumerable.Sum<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static decimal? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return System.Linq.Enumerable.Sum<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static double? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return System.Linq.Enumerable.Sum<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static int? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return System.Linq.Enumerable.Sum<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static long? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return System.Linq.Enumerable.Sum<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static float? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return System.Linq.Enumerable.Sum<TSource>(source, selector);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count)
        {
            return System.Linq.Enumerable.Take<TSource>(source, count);
        }

        [MethodImpl(256)]
        public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return System.Linq.Enumerable.TakeWhile<TSource>(source, predicate);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            return System.Linq.Enumerable.TakeWhile<TSource>(source, predicate);
        }
        
        [MethodImpl(256)]
        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return System.Linq.Enumerable.ThenBy<TSource, TKey>(source, keySelector);
        }
        
        [MethodImpl(256)]
        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            return System.Linq.Enumerable.ThenBy<TSource, TKey>(source, keySelector, comparer);
        }
        
        [MethodImpl(256)]
        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return System.Linq.Enumerable.ThenByDescending<TSource, TKey>(source, keySelector);
        }
        
        [MethodImpl(256)]
        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            return System.Linq.Enumerable.ThenByDescending<TSource, TKey>(source, keySelector, comparer);
        }
        
        [MethodImpl(256)]
        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
        {
            return System.Linq.Enumerable.ToArray<TSource>(source);
        }
                
        [MethodImpl(256)]
        public static AP.Collections.Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return ToDictionary<TSource, TKey, TSource>(source, keySelector, p => p);
        }
        
        [MethodImpl(256)]
        public static AP.Collections.Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return ToDictionary<TSource, TKey, TSource>(source, keySelector, p => p, comparer);
        }

        [MethodImpl(256)]
        public static AP.Collections.Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return ToDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector, null);
        }
        
        [MethodImpl(256)]
        public static AP.Collections.Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");

            AP.Collections.Dictionary<TKey, TElement> dictionary = new AP.Collections.Dictionary<TKey, TElement>(comparer);
            
            foreach (TSource local in source)
                dictionary.Add(keySelector(local), elementSelector(local));
            
            return dictionary;
        }
        
        [MethodImpl(256)]
        public static AP.Collections.List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            
            return new List<TSource>(source);
        }
        
        [MethodImpl(256)]
        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return System.Linq.Enumerable.ToLookup<TSource, TKey>(source, keySelector);
        }
        
        [MethodImpl(256)]
        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return System.Linq.Enumerable.ToLookup<TSource, TKey>(source, keySelector, comparer);
        }
        
        [MethodImpl(256)]
        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return System.Linq.Enumerable.ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector);
        }
        
        [MethodImpl(256)]
        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            return System.Linq.Enumerable.ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {        
            return System.Linq.Enumerable.Union<TSource>(first, second);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return System.Linq.Enumerable.Union<TSource>(first, second, comparer);
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return System.Linq.Enumerable.Where<TSource>(source, predicate);    
        }
        
        [MethodImpl(256)]
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            return System.Linq.Enumerable.Where<TSource>(source, predicate);    
        }
        

        [MethodImpl(256)]
        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            return System.Linq.Enumerable.Zip<TFirst, TSecond, TResult>(first, second, resultSelector);    
        }        
    }
}
