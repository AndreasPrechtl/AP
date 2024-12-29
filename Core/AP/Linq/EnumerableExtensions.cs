using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AP.Collections.ReadOnly;
using AP.Collections.Specialized;
using System.Collections;
using AP.Collections;
using System.Runtime.CompilerServices;

using SC = System.Collections;

namespace AP.Linq;

/// <summary>
/// Contains extension methods for IEnumerable sources
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Returns true if the collection has a count property.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool HasCount<TElement>(this IEnumerable<TElement> source)
    {
        if (source.IsDefault())
            return false;

        bool b = source is System.Collections.Generic.ICollection<TElement> ||
                 source is System.Collections.Generic.IReadOnlyCollection<TElement> ||
                 source is System.Collections.ICollection;

        if (!b)
        {
            try
            {
                dynamic d = source;
                
                b = !object.ReferenceEquals(null, d.Count);
            }
            catch (Exception)
            { }
        }

        return b;
    }

    /// <summary>
    /// Returns true when a collection has a count property.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="source"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static bool HasCount<TElement>(this IEnumerable<TElement> source, out int count)
    {
        if (source.IsDefault())
        {
            count = -1;
            return false;
        }

        if (source is System.Collections.Generic.ICollection<TElement> collection0)
        {
            count = collection0.Count;

            return true;
        }
        if (source is System.Collections.Generic.IReadOnlyCollection<TElement> collection1)
        {
            count = collection1.Count;

            return true;
        }
        if (source is System.Collections.ICollection collection2)
        {
            count = collection2.Count;

            return true;
        }
        
        try
        {
            dynamic d = source;
            count = d.Count;
            return true;
        }
        catch (Exception)
        {
            count = -1;
            return false;
        }
    }

    /// <summary>
    /// Returns true when a collection has a count property.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static bool HasCount(this IEnumerable source, out int count)
    {
        if (source.IsDefault())
        {
            count = -1;
            return false;
        }

        if (source is System.Collections.ICollection)
        {
            count = ((System.Collections.ICollection)source).Count;

            return true;
        }

        try
        {
            dynamic d = source;
            count = d.Count;
            return true;
        }
        catch (Exception)
        {
            count = -1;
            return false;
        }
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.IsNull() || enumerable.IsEmpty<T>();
    }

    public static IOrderedEnumerable<TSource> Sort<TSource, TKey>(this IEnumerable<TSource> source, KeySelector<TSource, TKey> keySelector, SortDirection sortDirection, IComparer<TKey>? keyComparer = null) => Sort(source, (Func<TSource, TKey>)keySelector.Invoke, sortDirection, keyComparer);

    public static IOrderedEnumerable<TSource> Sort<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortDirection sortDirection, IComparer<TKey>? keyComparer = null)
    {
        if (sortDirection == SortDirection.Unsorted)
            throw new ArgumentException("sortDirection");

        if (sortDirection == SortDirection.Ascending)
            return source.OrderBy(keySelector, keyComparer);

        return source.OrderByDescending(keySelector, keyComparer);                
    }

    public static SortDirection GetSortDirection<TSource>(this IEnumerable<TSource> source)
    {
        if (source is IOrderedEnumerable<TSource>)
        {
            var fi = source.GetType().GetField("descending", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.SetField);

            if (fi != null)
            {
                if (fi.GetValue(source) is true)
                    return SortDirection.Descending;
             
                return SortDirection.Ascending;
            }
        }
        return SortDirection.Unsorted;
    }

    public static bool IsEmpty<T>(this IEnumerable<T> source)
    {
        if (source is System.Collections.Generic.ICollection<T> collection)
            return collection.Count == 0;

        if (source is System.Collections.Generic.IReadOnlyCollection<T> collection1)
            return collection1.Count == 0;

        if (source is SC.ICollection collection2)
            return collection2.Count == 0;
        
        using (IEnumerator<T> e = source.GetEnumerator())
            return !e.MoveNext();
    }

    [MethodImpl((MethodImplOptions)256)]
    public static bool IsDefaultOrEmpty<T>(this IEnumerable<T> source)
    {
        return source.IsDefault() || (source is string s ? s.Equals(string.Empty) : source.IsEmpty());
    }
}
