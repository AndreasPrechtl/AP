using System;
using System.Collections;
using System.Collections.Generic;

namespace AP.Collections;

internal static class DictionaryHelper
{
    //public static KeyValuePairComparer<TKey, TValue> GetComparer<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
    //{
    //    if (dictionary is IComparerUser<KeyValuePair<TKey, TValue>>)
    //    {
    //        IComparer<KeyValuePair<TKey, TValue>> comparer = ((IComparerUser<KeyValuePair<TKey, TValue>>)dictionary).Comparer;

    //        if (comparer is KeyValuePairComparer<TKey, TValue>)
    //            return (KeyValuePairComparer<TKey, TValue>)comparer;

    //    }

    //    IComparer<TKey> keyComparer = null;
    //    IComparer<TValue> valueComparer = null;

    //    if (dictionary is IComparerUser<TKey>)
    //        keyComparer = ((IComparerUser<TKey>)dictionary).Comparer;
    //    else if (dictionary is SCG.SortedDictionary<TKey, TValue>)
    //        keyComparer = ((SCG.SortedDictionary<TKey, TValue>)dictionary).Comparer;
    //    else if (dictionary is SCG.SortedList<TKey, TValue>)
    //        keyComparer = ((SCG.SortedList<TKey, TValue>)dictionary).Comparer;

    //    if (dictionary is IComparerUser<TValue>)
    //        valueComparer = ((IComparerUser<TValue>)dictionary).Comparer;

    //    return new KeyValuePairComparer<TKey, TValue>(keyComparer, valueComparer);
    //}
    //public static KeyValuePairEqualityComparer<TKey, TValue> GetEqualityComparer<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
    //{
    //    if (dictionary is IEqualityComparerUser<KeyValuePair<TKey, TValue>>)
    //    {
    //        IEqualityComparer<KeyValuePair<TKey, TValue>> comparer = ((IEqualityComparerUser<KeyValuePair<TKey, TValue>>)dictionary).Comparer;

    //        if (comparer is KeyValuePairEqualityComparer<TKey, TValue>)
    //            return (KeyValuePairEqualityComparer<TKey, TValue>)comparer;
    //    }

    //    IEqualityComparer<TKey> keyComparer = null;
    //    IEqualityComparer<TValue> valueComparer = null;

    //    if (dictionary is IEqualityComparerUser<TKey>)
    //        keyComparer = ((IEqualityComparerUser<TKey>)dictionary).Comparer;
    //    else if (dictionary is SCG.Dictionary<TKey, TValue>)
    //        keyComparer = ((SCG.Dictionary<TKey, TValue>)dictionary).Comparer;

    //    if (dictionary is IEqualityComparerUser<TValue>)
    //        valueComparer = ((IEqualityComparerUser<TValue>)dictionary).Comparer;

    //    return new KeyValuePairEqualityComparer<TKey, TValue>(keyComparer, valueComparer);
    //}

    public static bool ContainsKeyValuePair<TKey, TValue>(Dictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> keyValuePair, bool compareValues = true)
        where TKey : notnull
    {
        bool hasKey = dictionary.Contains(keyValuePair.Key, out TValue value);

        if (hasKey && compareValues)
            return dictionary.ValueComparer.Equals(keyValuePair.Value, value);

        return hasKey;
    }

    public static bool ContainsKeyValuePair<TKey, TValue>(SortedDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> keyValuePair, bool compareValues = true)
        where TKey : notnull
    {
        bool hasKey = dictionary.Contains(keyValuePair.Key, out TValue ve);

        if (hasKey && compareValues)
            return dictionary.ValueComparer.Equals(keyValuePair.Value, ve);

        return hasKey;
    }

    public static bool ContainsKeyValuePair<TKey, TValue>(IDictionaryView<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> keyValuePair, bool compareValues = true)
        where TKey : notnull
    {
        bool hasKey = dictionary.ContainsKey(keyValuePair.Key);

        if (hasKey && compareValues)
            return dictionary.ContainsValue(keyValuePair.Value);

        return hasKey;
    }

    public static bool ContainsKeyValuePair<TKey, TValue>(IDictionaryView<TKey, TValue> dictionary, object keyValuePair, bool compareValues)
    {
        if (keyValuePair == null)
            return false;

        Type t = keyValuePair.GetType();

        if (t == typeof(KeyValuePair<TKey, TValue>))
        {
            return dictionary.Contains((KeyValuePair<TKey, TValue>)keyValuePair, compareValues);
        }

        if (t == typeof(Tuple<TKey, TValue>))
        {
            Tuple<TKey, TValue> tuple = (Tuple<TKey, TValue>)keyValuePair;

            return dictionary.Contains(new KeyValuePair<TKey, TValue>(tuple.Item1, tuple.Item2), compareValues);
        }

        object key = null;
        object value = null;

        if (t == typeof(DictionaryEntry))
        {
            DictionaryEntry kvp = (DictionaryEntry)keyValuePair;
            key = kvp.Key;
            value = kvp.Value;
        }

        if (CollectionsHelper.IsCompatible<TKey>(key) && CollectionsHelper.IsCompatible<TValue>(value))
            return dictionary.Contains(new KeyValuePair<TKey, TValue>((TKey)key, (TValue)value), compareValues);

        return false;
    }

    public static bool ContainsKey<TKey, TValue>(IDictionaryView<TKey, TValue> dictionary, object key) => CollectionsHelper.IsCompatible<TKey>(key) && dictionary.ContainsKey((TKey)key);

    public static bool ContainsValue<TKey, TValue>(IDictionaryView<TKey, TValue> dictionary, object value) => CollectionsHelper.IsCompatible<TValue>(value) && dictionary.ContainsValue((TValue)value);

    public static bool ContainsItem<TKey, TValue>(IDictionaryView<TKey, TValue> dictionary, object item)
    {
        if (CollectionsHelper.IsCompatible<TKey>(item))
            return dictionary.ContainsKey((TKey)item);

        if (CollectionsHelper.IsCompatible<TValue>(item))
            return dictionary.ContainsValue((TValue)item);

        return ContainsKeyValuePair<TKey, TValue>(dictionary, item, true);
    }
}
