using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using AP.Collections.ReadOnly;

namespace AP.Linq
{
    public static class DictionaryExtensions
    {
        public static AP.Collections.ReadOnly.ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> source)
        {
            ExceptionHelper.AssertNotNull(() => source);

            return new AP.Collections.ReadOnly.ReadOnlyDictionary<TKey, TValue>(source);
        }

        public static bool Update<TKey, TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> item)
        {
            AP.Collections.IDictionary<TKey, TValue> d = dictionary as AP.Collections.IDictionary<TKey, TValue>;

            if (d != null)
                d.Update(item);
                        
            return Update(dictionary, item.Key, item.Value);
        }

        public static bool Update<TKey, TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            AP.Collections.IDictionary<TKey, TValue> d = dictionary as AP.Collections.IDictionary<TKey, TValue>;

            if (d != null)
                d.Update(key, value);
            
            bool b = dictionary.ContainsKey(key);
            
            if (b)
                dictionary[key] = value;

            return b;
        }

        public static void Update<TKey, TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            AP.Collections.IDictionary<TKey, TValue> d = dictionary as AP.Collections.IDictionary<TKey, TValue>;

            if (d != null)
                d.Update(items);
                       
            foreach (KeyValuePair<TKey, TValue> kvp in items)
                Update(dictionary, kvp);
        }

        /// <summary>
        /// Replaces or adds a value for the given key
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static bool AddOrUpdate<TKey, TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.IsReadOnly)
                return false;

            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);

            return true;
        }

        /// <summary>
        /// Replaces or adds a value for the given key
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static bool AddOrUpdate<TKey, TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> item)
        {
            return AddOrUpdate(dictionary, item.Key, item.Value);
        }

        /// <summary>
        /// Replaces or adds values for the given keys
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOrUpdate<TKey, TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            foreach (KeyValuePair<TKey, TValue> item in items)
                AddOrUpdate(dictionary, item.Key, item.Value);
        }
        
        public static bool Add<TKey, TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            AP.Collections.IDictionary<TKey, TValue> d = dictionary as AP.Collections.IDictionary<TKey, TValue>;

            if (d != null)
                return d.Add(key, value);
            
            bool b = dictionary.ContainsKey(key);

            if (!b)
                dictionary.Add(key, value);

            return b;
        }

        public static bool Add<TKey, TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> item)
        {
            AP.Collections.IDictionary<TKey, TValue> d = dictionary as AP.Collections.IDictionary<TKey, TValue>;
            
            if (d != null)
                return d.Add(item);

            bool b = dictionary.ContainsKey(item.Key);

            if (!b)
                dictionary.Add(item);

            return b;
        }

        public static void Add<TKey, TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            AP.Collections.IDictionary<TKey, TValue> d = dictionary as AP.Collections.IDictionary<TKey, TValue>;

            if (d != null)
                d.Add(items);
            else
            {
                foreach (KeyValuePair<TKey, TValue> kvp in items)
                    dictionary.Add(kvp);
            }
        }

        public static void Remove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            AP.Collections.IDictionary<TKey, TValue> d = dictionary as AP.Collections.IDictionary<TKey, TValue>;

            if (d != null)
                d.Remove(items);
            else
            {
                foreach (KeyValuePair<TKey, TValue> kvp in items)
                    dictionary.Remove(kvp);
            }
        }



        //public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEqualityComparer<TKey> comparer = null)
        //{
        //    if (dictionary is IDictionary<TKey, TValue>)
        //        return new Dictionary<TKey, TValue>((IDictionary<TKey, TValue>)dictionary, comparer);
            
        //    Dictionary<TKey, TValue> d = new Dictionary<TKey, TValue>(comparer);
        //    d.AddRange(dictionary);

        //    return d;
        //}
    }
}
