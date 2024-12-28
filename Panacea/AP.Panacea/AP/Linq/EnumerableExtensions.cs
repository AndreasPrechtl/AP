using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using AP.Collections.ObjectModel;
using AP.Collections.ReadOnly;
using AP.Collections.Specialized;
using AP.ComponentModel;
using System.Collections;
using AP.Collections;
using System.Runtime.CompilerServices;

using SC = System.Collections;
using SCG = System.Collections.Generic;

namespace AP.Linq
{
    /// <summary>
    /// Contains extension methods for IEnumerables
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

            if (source is System.Collections.Generic.ICollection<TElement>)
            {
                count = ((System.Collections.Generic.ICollection<TElement>)source).Count;

                return true;
            }
            if (source is System.Collections.Generic.IReadOnlyCollection<TElement>)
            {
                count = ((System.Collections.Generic.IReadOnlyCollection<TElement>)source).Count;

                return true;
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

        /// <summary>
        /// Returns a readonly enumerable.
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ReadOnlyEnumerable<TElement> AsReadOnly<TElement>(this IEnumerable<TElement> source)
        {
            ExceptionHelper.AssertNotNull(() => source);

            return source as ReadOnlyEnumerable<TElement> ?? new ReadOnlyEnumerable<TElement>(source);
        }

        /// <summary>
        /// Returns a readonly enumerable.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ReadOnlyEnumerable AsReadOnly(this IEnumerable source)
        {
            ExceptionHelper.AssertNotNull(() => source);

            return source as ReadOnlyEnumerable ?? new ReadOnlyEnumerable(source);
        }

        public static ReadOnlyList<TSource> ToReadOnlyList<TSource>(this IEnumerable<TSource> source)
        {
            return new ReadOnlyList<TSource>(source);
        }

        public static Set<TSource> ToSet<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer = null)
        {
            return new Set<TSource>(source, comparer);
        }

        public static ReadOnlySet<TSource> ToReadOnlySet<TSource>(this IEnumerable<TSource> source)
        {
            return new ReadOnlySet<TSource>(source);
        }

        //public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        //{
        //    return !enumerable.Any();
        //}

        //public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        //{
        //    return enumerable.IsNull() || enumerable.IsEmpty();
        //}

        public static IOrderedEnumerable<TSource> Sort<TSource, TKey>(this IEnumerable<TSource> source, KeySelector<TSource, TKey> keySelector, SortDirection sortDirection, IComparer<TKey> keyComparer = null)
        {
            return Sort(source, (Func<TSource, TKey>)keySelector.Invoke, sortDirection, keyComparer);
        }

        public static IOrderedEnumerable<TSource> Sort<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortDirection sortDirection, IComparer<TKey> keyComparer = null)
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
                FieldInfo fi = source.GetType().GetField("descending", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.SetField);

                if (fi != null)
                {
                    if ((bool)fi.GetValue(source))
                        return SortDirection.Descending;
                 
                    return SortDirection.Ascending;
                }
            }
            return SortDirection.Unsorted;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            if (source is System.Collections.Generic.ICollection<T>)
                return ((System.Collections.Generic.ICollection<T>)source).Count == 0;

            if (source is System.Collections.Generic.IReadOnlyCollection<T>)
                return ((System.Collections.Generic.IReadOnlyCollection<T>)source).Count == 0;

            if (source is SC.ICollection)
                return ((SC.ICollection)source).Count == 0;
            
            using (IEnumerator<T> e = source.GetEnumerator())
                return !e.MoveNext();
        }

        [MethodImpl((MethodImplOptions)256)]
        public static bool IsDefaultOrEmpty<T>(this IEnumerable<T> source)
        {
            return source.IsDefault() || (source is string ? source.Equals(string.Empty) : source.IsEmpty());
        }

        [MethodImpl((MethodImplOptions)256)]
        public static AP.Collections.ReadOnly.ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
        {
            return new AP.Collections.ReadOnly.ReadOnlyDictionary<TKey, TValue>(dictionary);
        }

        [MethodImpl((MethodImplOptions)256)]
        public static AP.Collections.ReadOnly.ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            return new AP.Collections.ReadOnly.ReadOnlyDictionary<TKey, TValue>(dictionary, keyComparer, valueComparer);
        }

        [MethodImpl((MethodImplOptions)256)]
        public static AP.Collections.Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
        {
            return new AP.Collections.Dictionary<TKey, TValue>(dictionary);
        }

        [MethodImpl((MethodImplOptions)256)]
        public static AP.Collections.Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            return new AP.Collections.Dictionary<TKey, TValue>(dictionary, keyComparer, valueComparer);
        }

        [MethodImpl((MethodImplOptions)256)]
        public static AP.Collections.Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(this IEnumerable<TElement> collection, Func<TElement, TKey> keySelector, IEqualityComparer<TKey> keyComparer = null, IEqualityComparer<TElement> valueComparer = null)
        {
            AP.Collections.Dictionary<TKey, TElement> d = new Collections.Dictionary<TKey, TElement>(keyComparer, valueComparer);

            foreach (TElement current in collection)
            {
                TKey key = keySelector(current);

                if (!d.ContainsKey(key))
                    d.Add(key, current);
            }

            return d;
        }

        [MethodImpl((MethodImplOptions)256)]
        public static AP.Collections.Dictionary<TKey, TElement> ToDictionary<TKey, TElement, TSource>(this IEnumerable<TSource> collection, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> keyComparer = null, IEqualityComparer<TElement> valueComparer = null)
        {
            AP.Collections.Dictionary<TKey, TElement> d = new Collections.Dictionary<TKey, TElement>(keyComparer, valueComparer);
            
            foreach (TSource current in collection)
            {
                TKey key = keySelector(current);

                if (!d.ContainsKey(key))
                    d.Add(key, elementSelector(current));
            }

            return d;
        }

        [MethodImpl((MethodImplOptions)256)]
        public static NameValueDictionary<T> ToNameValueDictionary<T>(this IEnumerable<KeyValuePair<string, T>> dictionary)
        {
            return new NameValueDictionary<T>(dictionary);
        }
        
        [MethodImpl((MethodImplOptions)256)]
        public static NameValueDictionary<T> ToNameValueDictionary<T>(this IEnumerable<KeyValuePair<string, T>> dictionary, IEqualityComparer<string> keyComparer, IEqualityComparer<T> valueComparer)
        {
            return new NameValueDictionary<T>(dictionary, keyComparer, valueComparer);
        }
        
        [MethodImpl((MethodImplOptions)256)]
        public static ReadOnlyNameValueDictionary<T> ToReadOnlyNameValueDictionary<T>(this IEnumerable<KeyValuePair<string, T>> dictionary)
        {
            return new ReadOnlyNameValueDictionary<T>(dictionary);
        }

        [MethodImpl((MethodImplOptions)256)]
        public static ReadOnlyNameValueDictionary<T> ToReadOnlyNameValueDictionary<T>(this IEnumerable<KeyValuePair<string, T>> dictionary, IEqualityComparer<string> keyComparer, IEqualityComparer<T> valueComparer)
        {
            return new ReadOnlyNameValueDictionary<T>(dictionary, keyComparer, valueComparer);
        }
    }
}
