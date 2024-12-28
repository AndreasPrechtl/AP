#if fx40

using System.Collections;
using System.Runtime.CompilerServices;

// 4.0 compatibility patch enforced 
namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a strongly-typed, read-only collection of elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements.This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived. For more information about covariance and contravariance, see Covariance and Contravariance in Generics.</typeparam>
    public interface IReadOnlyCollection<out T> : IEnumerable<T>, IEnumerable
    {
        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        /// 
        /// <returns>
        /// The number of elements in the collection.
        /// </returns>
        int Count { get; }
    }

    public interface IReadOnlyList<out T> : IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable
    {
        T this[int index] { get; }
    }

    public interface IReadOnlyDictionary<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        bool ContainsKey(TKey key);
        bool TryGetValue(TKey key, out TValue value);
        TValue this[TKey key] { get; }
        
        IEnumerable<TKey> Keys { get; }        
        IEnumerable<TValue> Values { get; }
    }
}

#endif