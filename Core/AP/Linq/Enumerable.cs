using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AP.Collections.ReadOnly;
using AP.Collections;
using AP.Collections.Specialized;

namespace AP.Linq;

// todo: revisit namespaces
public static class Enumerable
{
    /// <summary>
    /// Returns a readonly enumerable.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static ReadOnlyEnumerable<TElement> AsReadOnly<TElement>(this IEnumerable<TElement> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source as ReadOnlyEnumerable<TElement> ?? new ReadOnlyEnumerable<TElement>(source);
    }

    /// <summary>
    /// Returns a readonly enumerable.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static ReadOnlyEnumerable AsReadOnly(this IEnumerable source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source as ReadOnlyEnumerable ?? new ReadOnlyEnumerable(source);
    }

    public static ReadOnlyList<TSource> ToReadOnlyList<TSource>(this IEnumerable<TSource> source) => new(source);

    public static Set<TSource> ToSet<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource>? comparer = null) => new(source, comparer);

    public static ReadOnlySet<TSource> ToReadOnlySet<TSource>(this IEnumerable<TSource> source) => new(source);


    [MethodImpl((MethodImplOptions)256)]
    public static AP.Collections.ReadOnly.ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
        where TKey : notnull
        => new(dictionary);

    [MethodImpl((MethodImplOptions)256)]
    public static AP.Collections.ReadOnly.ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        where TKey : notnull
        => new(dictionary, keyComparer, valueComparer);

    [MethodImpl((MethodImplOptions)256)]
    public static AP.Collections.Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
        where TKey : notnull
        => new(dictionary);

    [MethodImpl((MethodImplOptions)256)]
    public static AP.Collections.Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        where TKey : notnull
        => new(dictionary, keyComparer, valueComparer);

    [MethodImpl((MethodImplOptions)256)]
    public static AP.Collections.Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(this IEnumerable<TElement> collection, Func<TElement, TKey> keySelector, IEqualityComparer<TKey>? keyComparer = null, IEqualityComparer<TElement>? valueComparer = null)
        where TKey : notnull
    {
        AP.Collections.Dictionary<TKey, TElement> d = new(keyComparer!, valueComparer!);

        foreach (TElement current in collection)
        {
            TKey key = keySelector(current);

            if (!d.ContainsKey(key))
                d.Add(key, current);
        }

        return d;
    }

    [MethodImpl((MethodImplOptions)256)]
    public static AP.Collections.Dictionary<TKey, TElement> ToDictionary<TKey, TElement, TSource>(this IEnumerable<TSource> collection, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? keyComparer = null, IEqualityComparer<TElement>? valueComparer = null)
        where TKey : notnull
    {
        AP.Collections.Dictionary<TKey, TElement> d = new(keyComparer!, valueComparer!);

        foreach (TSource current in collection)
        {
            TKey key = keySelector(current);

            if (!d.ContainsKey(key))
                d.Add(key, elementSelector(current));
        }

        return d;
    }

    [MethodImpl((MethodImplOptions)256)]
    public static NameValueDictionary<T> ToNameValueDictionary<T>(this IEnumerable<KeyValuePair<string, T>> dictionary) => new(dictionary);

    [MethodImpl((MethodImplOptions)256)]
    public static NameValueDictionary<T> ToNameValueDictionary<T>(this IEnumerable<KeyValuePair<string, T>> dictionary, IEqualityComparer<string> keyComparer, IEqualityComparer<T> valueComparer) => new(dictionary, keyComparer, valueComparer);

    [MethodImpl((MethodImplOptions)256)]
    public static ReadOnlyNameValueDictionary<T> ToReadOnlyNameValueDictionary<T>(this IEnumerable<KeyValuePair<string, T>> dictionary) => new(dictionary);

    [MethodImpl((MethodImplOptions)256)]
    public static ReadOnlyNameValueDictionary<T> ToReadOnlyNameValueDictionary<T>(this IEnumerable<KeyValuePair<string, T>> dictionary, IEqualityComparer<string> keyComparer, IEqualityComparer<T> valueComparer) => new(dictionary, keyComparer, valueComparer);
    
    [MethodImpl(256)]
    public static AP.Collections.Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? keyComparer = null, IEqualityComparer<TElement>? valueComparer = null)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(keySelector);
        ArgumentNullException.ThrowIfNull(elementSelector);

        AP.Collections.Dictionary<TKey, TElement> dictionary = new(keyComparer!, valueComparer!);
        
        foreach (TSource local in source)
            dictionary.Add(keySelector(local), elementSelector(local));
        
        return dictionary;
    }

    [MethodImpl(256)]
    public static AP.Collections.List<TSource> ToList<TSource>(this IEnumerable<TSource> source) => new(source);
}
