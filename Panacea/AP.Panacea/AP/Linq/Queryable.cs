using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AP.Linq
{
    public static class Queryable
    {
        [MethodImpl(256)]
        public static TSource Aggregate<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, TSource, TSource>> func)
        {
            return System.Linq.Queryable.Aggregate<TSource>(source, func);
        }

        [MethodImpl(256)]
        public static TAccumulate Aggregate<TSource, TAccumulate>(this IQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func)
        {
            return System.Linq.Queryable.Aggregate<TSource, TAccumulate>(source, seed, func);
        }

        [MethodImpl(256)]
        public static TResult Aggregate<TSource, TAccumulate, TResult>(this IQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector)
        {
            return System.Linq.Queryable.Aggregate<TSource, TAccumulate, TResult>(source, seed, func, selector);
        }

        [MethodImpl(256)]
        public static bool All<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return System.Linq.Queryable.All<TSource>(source, predicate);
        }

        [MethodImpl(256)]
        public static bool Any<TSource>(this IQueryable<TSource> source)
        {
            return System.Linq.Queryable.Any<TSource>(source);
        }

        [MethodImpl(256)]
        public static bool Any<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return System.Linq.Queryable.Any<TSource>(source, predicate);
        }

        [MethodImpl(256)]
        public static IQueryable<TElement> AsQueryable<TElement>(this IEnumerable<TElement> source)
        {
            return System.Linq.Queryable.AsQueryable<TElement>(source);
        }

        [MethodImpl(256)]
        public static IQueryable AsQueryable(this IEnumerable source)
        {
            return System.Linq.Queryable.AsQueryable(source);
        }

        [MethodImpl(256)]
        public static decimal Average(this IQueryable<decimal> source)
        {
            return System.Linq.Queryable.Average(source);
        }

        [MethodImpl(256)]
        public static double Average(this IQueryable<double> source)
        {
            return System.Linq.Queryable.Average(source);
        }

        [MethodImpl(256)]
        public static double Average(this IQueryable<int> source)
        {
            return System.Linq.Queryable.Average(source);
        }

        [MethodImpl(256)]
        public static double Average(this IQueryable<long> source)
        {
            return System.Linq.Queryable.Average(source);
        }

        [MethodImpl(256)]
        public static double? Average(this IQueryable<int?> source)
        {
            return System.Linq.Queryable.Average(source);
        }

        [MethodImpl(256)]
        public static double? Average(this IQueryable<long?> source)
        {
            return System.Linq.Queryable.Average(source);
        }

        [MethodImpl(256)]
        public static float Average(this IQueryable<float> source)
        {
            return System.Linq.Queryable.Average(source);
        }

        [MethodImpl(256)]
        public static decimal? Average(this IQueryable<decimal?> source)
        {
            return System.Linq.Queryable.Average(source);
        }

        [MethodImpl(256)]
        public static double? Average(this IQueryable<double?> source)
        {
            return System.Linq.Queryable.Average(source);
        }

        [MethodImpl(256)]
        public static float? Average(this IQueryable<float?> source)
        {
            return System.Linq.Queryable.Average(source);
        }

        [MethodImpl(256)]
        public static decimal Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            return System.Linq.Queryable.Average<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static double Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector)
        {
            return System.Linq.Queryable.Average<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static double Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector)
        {
            return System.Linq.Queryable.Average<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static double Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector)
        {
            return System.Linq.Queryable.Average<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static float Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector)
        {
            return System.Linq.Queryable.Average<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static decimal? Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
        {
            return System.Linq.Queryable.Average<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static double? Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
        {
            return System.Linq.Queryable.Average<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static double? Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
        {
            return System.Linq.Queryable.Average<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static double? Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
        {
            return System.Linq.Queryable.Average<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static float? Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float?>> selector)
        {
            return System.Linq.Queryable.Average<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static IQueryable<TResult> Cast<TResult>(this IQueryable source)
        {
            return System.Linq.Queryable.Cast<TResult>(source);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> Concat<TSource>(this IQueryable<TSource> first, IEnumerable<TSource> second)
        {
            return System.Linq.Queryable.Concat<TSource>(first, second);
        }

        [MethodImpl(256)]
        public static bool Contains<TSource>(this IQueryable<TSource> source, TSource item)
        {
            return System.Linq.Queryable.Contains<TSource>(source, item);
        }

        [MethodImpl(256)]
        public static bool Contains<TSource>(this IQueryable<TSource> source, TSource item, IEqualityComparer<TSource> comparer)
        {
            return System.Linq.Queryable.Contains<TSource>(source, item, comparer);
        }

        [MethodImpl(256)]
        public static int Count<TSource>(this IQueryable<TSource> source)
        {
            return System.Linq.Queryable.Count<TSource>(source);
        }

        [MethodImpl(256)]
        public static int Count<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return System.Linq.Queryable.Count<TSource>(source, predicate);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> DefaultIfEmpty<TSource>(this IQueryable<TSource> source)
        {
            return System.Linq.Queryable.DefaultIfEmpty<TSource>(source);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> DefaultIfEmpty<TSource>(this IQueryable<TSource> source, TSource defaultValue)
        {
            return System.Linq.Queryable.DefaultIfEmpty<TSource>(source, defaultValue);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> Distinct<TSource>(this IQueryable<TSource> source)
        {
            return System.Linq.Queryable.Distinct<TSource>(source);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> Distinct<TSource>(this IQueryable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            return System.Linq.Queryable.Distinct<TSource>(source, comparer);
        }

        [MethodImpl(256)]
        public static TSource ElementAt<TSource>(this IQueryable<TSource> source, int index)
        {
            return System.Linq.Queryable.ElementAt<TSource>(source, index);
        }

        [MethodImpl(256)]
        public static TSource ElementAtOrDefault<TSource>(this IQueryable<TSource> source, int index)
        {
            return System.Linq.Queryable.ElementAtOrDefault<TSource>(source, index);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> Except<TSource>(this IQueryable<TSource> first, IEnumerable<TSource> second)
        {
            return System.Linq.Queryable.Except<TSource>(first, second);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> Except<TSource>(this IQueryable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return System.Linq.Queryable.Except<TSource>(first, second, comparer);
        }

        [MethodImpl(256)]
        public static TSource First<TSource>(this IQueryable<TSource> source)
        {
            return System.Linq.Queryable.First<TSource>(source);
        }

        [MethodImpl(256)]
        public static TSource First<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return System.Linq.Queryable.First<TSource>(source, predicate);
        }

        [MethodImpl(256)]
        public static TSource FirstOrDefault<TSource>(this IQueryable<TSource> source)
        {
            return System.Linq.Queryable.FirstOrDefault<TSource>(source);
        }

        [MethodImpl(256)]
        public static TSource FirstOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return System.Linq.Queryable.FirstOrDefault<TSource>(source, predicate);
        }
        
        [MethodImpl(256)]
        public static IQueryable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return System.Linq.Queryable.GroupBy<TSource, TKey>(source, keySelector);
        }

        [MethodImpl(256)]
        public static IQueryable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            return System.Linq.Queryable.GroupBy<TSource, TKey>(source, keySelector, comparer);
        }

        [MethodImpl(256)]
        public static IQueryable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector)
        {
            return System.Linq.Queryable.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector);
        }

        [MethodImpl(256)]
        public static IQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector)
        {
            return System.Linq.Queryable.GroupBy<TSource, TKey, TResult>(source, keySelector, resultSelector);
        }

        [MethodImpl(256)]
        public static IQueryable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer)
        {
            return System.Linq.Queryable.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
        }

        [MethodImpl(256)]
        public static IQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        {
            return System.Linq.Queryable.GroupBy<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector);
        }

        [MethodImpl(256)]
        public static IQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return System.Linq.Queryable.GroupBy<TSource, TKey, TResult>(source, keySelector, resultSelector, comparer);
        }

        [MethodImpl(256)]
        public static IQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return System.Linq.Queryable.GroupBy<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, comparer);
        }

        [MethodImpl(256)]
        public static IQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IQueryable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector)
        {
            return System.Linq.Queryable.GroupJoin<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        [MethodImpl(256)]
        public static IQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IQueryable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return System.Linq.Queryable.GroupJoin<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> Intersect<TSource>(this IQueryable<TSource> first, IEnumerable<TSource> second)
        {
            return System.Linq.Queryable.Intersect<TSource>(first, second);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> Intersect<TSource>(this IQueryable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return System.Linq.Queryable.Intersect<TSource>(first, second, comparer);
        }

        [MethodImpl(256)]
        public static IQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IQueryable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            return System.Linq.Queryable.Join<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        [MethodImpl(256)]
        public static IQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IQueryable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return System.Linq.Queryable.Join<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        [MethodImpl(256)]
        public static TSource Last<TSource>(this IQueryable<TSource> source)
        {
            return System.Linq.Queryable.Last<TSource>(source);
        }

        [MethodImpl(256)]
        public static TSource Last<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return System.Linq.Queryable.Last<TSource>(source, predicate);
        }

        [MethodImpl(256)]
        public static TSource LastOrDefault<TSource>(this IQueryable<TSource> source)
        {
            return System.Linq.Queryable.LastOrDefault<TSource>(source);
        }

        [MethodImpl(256)]
        public static TSource LastOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return System.Linq.Queryable.LastOrDefault<TSource>(source, predicate);
        }

        [MethodImpl(256)]
        public static long LongCount<TSource>(this IQueryable<TSource> source)
        {
            return System.Linq.Queryable.LongCount<TSource>(source);
        }

        [MethodImpl(256)]
        public static long LongCount<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return System.Linq.Queryable.LongCount<TSource>(source, predicate);
        }

        [MethodImpl(256)]
        public static TSource Max<TSource>(this IQueryable<TSource> source)
        {
            return System.Linq.Queryable.Max<TSource>(source);
        }

        [MethodImpl(256)]
        public static TResult Max<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return System.Linq.Queryable.Max<TSource, TResult>(source, selector);
        }

        [MethodImpl(256)]
        public static TSource Min<TSource>(this IQueryable<TSource> source)
        {
            return System.Linq.Queryable.Min<TSource, TResult>(source);
        }

        [MethodImpl(256)]
        public static TResult Min<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return System.Linq.Queryable.Min<TSource, TResult>(source, selector);
        }

        [MethodImpl(256)]
        public static IQueryable<TResult> OfType<TResult>(this IQueryable source)
        {
            return System.Linq.Queryable.OfType<TResult>(source);
        }

        [MethodImpl(256)]
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return System.Linq.Queryable.OrderBy<TSource, TKey>(source, keySelector);
        }

        [MethodImpl(256)]
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
        {
            return System.Linq.Queryable.OrderBy<TSource, TKey>(source, keySelector, comparer);
        }

        [MethodImpl(256)]
        public static IOrderedQueryable<TSource> OrderByDescending<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return System.Linq.Queryable.OrderByDescending<TSource, TKey>(source, keySelector);
        }

        [MethodImpl(256)]
        public static IOrderedQueryable<TSource> OrderByDescending<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
        {
            return System.Linq.Queryable.OrderByDescending<TSource, TKey>(source, keySelector, comparer);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> Reverse<TSource>(this IQueryable<TSource> source)
        {
            return System.Linq.Queryable.Reverse<TSource>(source);
        }

        [MethodImpl(256)]
        public static IQueryable<TResult> Select<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return System.Linq.Queryable.Select<TSource, TResult>(source, selector);
        }

        [MethodImpl(256)]
        public static IQueryable<TResult> Select<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, int, TResult>> selector)
        {
            return System.Linq.Queryable.Select<TSource, TResult>(source, selector);
        }

        [MethodImpl(256)]
        public static IQueryable<TResult> SelectMany<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> selector)
        {
            return System.Linq.Queryable.SelectMany<TSource, TResult>(source, selector);
        }

        [MethodImpl(256)]
        public static IQueryable<TResult> SelectMany<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, int, IEnumerable<TResult>>> selector)
        {
            return System.Linq.Queryable.SelectMany<TSource, TResult>(source, selector);
        }

        [MethodImpl(256)]
        public static IQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
        {
            return System.Linq.Queryable.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        [MethodImpl(256)]
        public static IQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
        {
            return System.Linq.Queryable.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        [MethodImpl(256)]
        public static bool SequenceEqual<TSource>(this IQueryable<TSource> first, IEnumerable<TSource> second)
        {
            return System.Linq.Queryable.SequenceEqual<TSource>(first, second);
        }

        [MethodImpl(256)]
        public static bool SequenceEqual<TSource>(this IQueryable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return System.Linq.Queryable.SequenceEqual<TSource>(first, second, comparer);
        }

        [MethodImpl(256)]
        public static TSource Single<TSource>(this IQueryable<TSource> source)
        {
            return System.Linq.Queryable.Single<TSource>(source);
        }

        [MethodImpl(256)]
        public static TSource Single<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return System.Linq.Queryable.Single<TSource>(source, predicate);
        }

        [MethodImpl(256)]
        public static TSource SingleOrDefault<TSource>(this IQueryable<TSource> source)
        {
            return System.Linq.Queryable.SingleOrDefault<TSource>(source);
        }

        [MethodImpl(256)]
        public static TSource SingleOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return System.Linq.Queryable.SingleOrDefault<TSource>(source, predicate);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> Skip<TSource>(this IQueryable<TSource> source, int count)
        {
            return System.Linq.Queryable.Skip<TSource>(source, count);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> SkipWhile<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return System.Linq.Queryable.SkipWhile<TSource>(source, predicate);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> SkipWhile<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            return System.Linq.Queryable.SkipWhile<TSource>(source, predicate);
        }

        [MethodImpl(256)]
        public static decimal Sum(this IQueryable<decimal> source)
        {
            return System.Linq.Queryable.Sum(source);
        }

        [MethodImpl(256)]
        public static double Sum(this IQueryable<double> source)
        {
            return System.Linq.Queryable.Sum(source);
        }

        [MethodImpl(256)]
        public static int Sum(this IQueryable<int> source)
        {
            return System.Linq.Queryable.Sum(source);
        }

        [MethodImpl(256)]
        public static long Sum(this IQueryable<long> source)
        {
            return System.Linq.Queryable.Sum(source);
        }

        [MethodImpl(256)]
        public static decimal? Sum(this IQueryable<decimal?> source)
        {
            return System.Linq.Queryable.Sum(source);
        }

        [MethodImpl(256)]
        public static int? Sum(this IQueryable<int?> source)
        {
            return System.Linq.Queryable.Sum(source);
        }

        [MethodImpl(256)]
        public static long? Sum(this IQueryable<long?> source)
        {
            return System.Linq.Queryable.Sum(source);
        }

        [MethodImpl(256)]
        public static float Sum(this IQueryable<float> source)
        { 
            return System.Linq.Queryable.Sum(source); 
        }

        [MethodImpl(256)]
        public static double? Sum(this IQueryable<double?> source)
        {
            return System.Linq.Queryable.Sum(source);
        }

        [MethodImpl(256)]
        public static float? Sum(this IQueryable<float?> source)
        {
            return System.Linq.Queryable.Sum(source);
        }

        [MethodImpl(256)]
        public static decimal Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            return System.Linq.Queryable.Sum<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static double Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector)
        {
            return System.Linq.Queryable.Sum<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static int Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector)
        {
            return System.Linq.Queryable.Sum<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static long Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector)
        {
            return System.Linq.Queryable.Sum<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static decimal? Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
        {
            return System.Linq.Queryable.Sum<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static double? Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
        {
            return System.Linq.Queryable.Sum<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static int? Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
        {
            return System.Linq.Queryable.Sum<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static float? Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float?>> selector)
        {
            return System.Linq.Queryable.Sum<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static float Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector)
        {
            return System.Linq.Queryable.Sum<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static long? Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
        {
            return System.Linq.Queryable.Sum<TSource>(source, selector);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> Take<TSource>(this IQueryable<TSource> source, int count)
        {
            return System.Linq.Queryable.Take<TSource>(source, count);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> TakeWhile<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return System.Linq.Queryable.TakeWhile<TSource>(source, predicate);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> TakeWhile<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            return System.Linq.Queryable.TakeWhile<TSource>(source, predicate);
        }

        [MethodImpl(256)]
        public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return System.Linq.Queryable.ThenBy<TSource, TKey>(source, keySelector, keySelector);
        }

        [MethodImpl(256)]
        public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
        {
            return System.Linq.Queryable.ThenBy<TSource, TKey>(source, keySelector, comparer);
        }

        [MethodImpl(256)]
        public static IOrderedQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return System.Linq.Queryable.ThenByDescending<TSource, TKey>(source, keySelector);
        }

        [MethodImpl(256)]
        public static IOrderedQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
        {
            return System.Linq.Queryable.ThenByDescending<TSource, TKey>(source, keySelector, comparer);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> Union<TSource>(this IQueryable<TSource> first, IEnumerable<TSource> second)
        {
            return System.Linq.Queryable.Union<TSource>(first, second);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> Union<TSource>(this IQueryable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return System.Linq.Queryable.Union<TSource>(first, second, comparer);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return System.Linq.Queryable.Where<TSource>(source, predicate);
        }

        [MethodImpl(256)]
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            return System.Linq.Queryable.Where<TSource>(source, predicate);
        }

        [MethodImpl(256)]
        public static IQueryable<TResult> Zip<TFirst, TSecond, TResult>(this IQueryable<TFirst> first, IEnumerable<TSecond> second, Expression<Func<TFirst, TSecond, TResult>> resultSelector)
        {
            return System.Linq.Queryable.Zip<TFirst, TSecond, TResult>(first, second, resultSelector);
        }
    }
}
