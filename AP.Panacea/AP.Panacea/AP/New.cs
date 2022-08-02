using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AP.Collections;
using AP.Collections.ObjectModel;
using AP.Collections.ReadOnly;
using AP.Collections.Specialized;
using AP.Linq;
using AP.Reflection;
using System.Runtime.CompilerServices;

namespace AP
{
    /// <summary>
    /// Provides a set of shorthand methods for creating objects - mostly collection types
    /// </summary>
    public abstract class New : StaticType
    {
        /// <summary>
        /// Short method for System.Activator.CreateInstance&lt;T&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        [MethodImpl((MethodImplOptions)256)]
        public static T Instance<T>(params object[] args)
        {
            return Objects.New<T>(args);
        }

        /// <summary>
        /// Short method for System.Activator.CreateInstance&lt;T&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        [MethodImpl((MethodImplOptions)256)]
        public static T Object<T>(params object[] args)
        {
            return Objects.New<T>(args);
        }

        public static TException Exception<TException>(string message, Exception innerException)
            where TException : Exception
        {
            if (innerException != null)
                return (TException)Activator.CreateInstance(typeof (TException), message, innerException);

            return (TException)Activator.CreateInstance(typeof (TException), message);
        }

        public static TException Exception<TException>(string message)
            where TException : Exception
        {
            return (TException)Activator.CreateInstance(typeof (TException), message);
        }

        /// <summary>
        /// Short method for System.Activator.CreateInstance
        /// Warning, if there are default values in the constructor it WILL throw a MissingMethodException (setting the values manually helps)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [MethodImpl((MethodImplOptions)256)]
        public static object Instance(Type type, params object[] args)
        {
            return System.Activator.CreateInstance(type, (args == null || args.Length == 0) ? null : args);
        }

        /// <summary>
        /// Short method for System.Activator.CreateInstance
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [MethodImpl((MethodImplOptions)256)]
        public static object Object(Type type, params object[] args)
        {
            return New.Instance(type, args);
        }

        /// <summary>
        /// Creates a new System.Guid
        /// </summary>
        /// <returns>The Guid</returns>
        [MethodImpl((MethodImplOptions)256)]
        public static Guid Guid()
        {            
            return System.Guid.NewGuid();
        }


        /// <summary>
        /// Creates a new Deferrable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="activator"></param>
        /// <param name="isFrozen"></param>
        /// <returns></returns>
        [MethodImpl((MethodImplOptions)256)]
        public static Deferrable<T> Deferrable<T>(Activator<T> activator = null, bool isFrozen = true)
        {
            return new Deferrable<T>(activator, isFrozen);
        }

        /// <summary>
        /// Clearly only a cosmetic method - instead of creating the array like new string[] { "1234", "foo", "bar" };
        /// you can use New.Array("1234", "foo", "bar")
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [MethodImpl((MethodImplOptions)256)]
        public static T[] Array<T>(params T[] parameters)
        {
            return parameters;
        }

        /// <summary>
        /// Casts an Array to an IEnumerable - probably helpful for methods overloaded with both: params T[] and IEnumerable&lt;T&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [MethodImpl((MethodImplOptions)256)]
        public static IEnumerable<T> Enumerable<T>(params T[] parameters)
        {
            return parameters;
        }

        [MethodImpl((MethodImplOptions)256)]
        public static AP.Collections.List<T> List<T>(params T[] items)
        {
            return new AP.Collections.List<T>(items);
        }

        [MethodImpl((MethodImplOptions)256)]
        public static AP.Collections.Set<T> Set<T>(params T[] items)
        {
            return new Set<T>(items);
        }

        [MethodImpl((MethodImplOptions)256)]
        public static AP.Collections.Queue<T> Queue<T>(params T[] items)
        {
            return new AP.Collections.Queue<T>(items);
        }

        [MethodImpl((MethodImplOptions)256)]
        public static AP.Collections.Stack<T> Stack<T>(params T[] items)
        {
            return new AP.Collections.Stack<T>(items);
        }

        //[MethodImpl((MethodImplOptions)256)]
        //public static CollectionBase<T> Collection<T>(params T[] items)
        //{
        //    return new CollectionBase<T>(items);
        //}

        [MethodImpl((MethodImplOptions)256)]
        public static AP.Collections.Dictionary<TKey, TValue> Dictionary<TKey, TValue>(params KeyValuePair<TKey, TValue>[] dictionary)
        {
            return new AP.Collections.Dictionary<TKey, TValue>(dictionary);
        }

        //[MethodImpl((MethodImplOptions)256)]
        //public static Collections.Dictionary<TKey, TValue> Dictionary<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary), IEqualityComparer<TKey> keyComparer = null, IEqualityComparer<TValue> valueComparer = null)
        //{
        //    return new AP.Collections.Dictionary<TKey, TValue>(dictionary, keyComparer, valueComparer);
        //}

        //[MethodImpl((MethodImplOptions)256)]
        //public static Collections.Dictionary<TKey, TValue> Dictionary<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IKeyValuePairEqualityComparer<TKey, TValue> comparer = null)
        //{
        //    return new AP.Collections.Dictionary<TKey, TValue>(dictionary, comparer);
        //}

        //[MethodImpl((MethodImplOptions)256)]
        //public static NameValueDictionary<T> NameValueDictionary<T>(IEnumerable<KeyValuePair<string, T>> dictionary, IEqualityComparer<string> nameComparer = null, IEqualityComparer<T> valueComparer = null)
        //{
        //    return new NameValueDictionary<T>(dictionary, nameComparer, valueComparer);
        //}

        [MethodImpl((MethodImplOptions)256)]
        public static NameValueDictionary<T> NameValueDictionary<T>(params KeyValuePair<string, T>[] dictionary)
        {
            return new NameValueDictionary<T>(dictionary);
        }

        [MethodImpl((MethodImplOptions)256)]
        public static ReadOnlyNameValueDictionary<T> ReadOnlyNameValueDictionary<T>(IEnumerable<KeyValuePair<string, T>> dictionary)
        {
            return new ReadOnlyNameValueDictionary<T>(dictionary);
        }

        [MethodImpl((MethodImplOptions)256)]
        public static ReadOnlyNameValueDictionary<T> ReadOnlyNameValueDictionary<T>(params KeyValuePair<string, T>[] dictionary)
        {
            return new ReadOnlyNameValueDictionary<T>(dictionary);
        }
        
        [MethodImpl((MethodImplOptions)256)]
        public static ReadOnlyList<T> ReadOnlyList<T>(params T[] items)
        {
            return new ReadOnlyList<T>(items);
        }

        [MethodImpl((MethodImplOptions)256)]
        public static ReadOnlyDictionary<TKey, TValue> ReadOnlyDictionary<TKey, TValue>(params KeyValuePair<TKey, TValue>[] dictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }

        [MethodImpl((MethodImplOptions)256)]
        public static ReadOnlySet<T> ReadOnlySet<T>(params T[] items)
        {
            return new ReadOnlySet<T>(items);
        }

        [MethodImpl((MethodImplOptions)256)]
        public static ReadOnlyEnumerable<T> ReadOnlyEnumerable<T>(params T[] items)
        {
            return new ReadOnlyEnumerable<T>(items);
        }
    }
}
