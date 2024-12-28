using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections
{
    public interface ISetView<T> : ICollection<T>
    {
        /// <summary>
        /// Determines whether a set is a subset of a specified collection.
        /// </summary>
        /// 
        /// <returns>
        /// true if the current set is a subset of <paramref name="other"/>; otherwise, false.
        /// </returns>
        /// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool IsSubsetOf(IEnumerable<T> other);

        /// <summary>
        /// Determines whether the current set is a superset of a specified collection.
        /// </summary>
        /// 
        /// <returns>
        /// true if the current set is a superset of <paramref name="other"/>; otherwise, false.
        /// </returns>
        /// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool IsSupersetOf(IEnumerable<T> other);

        /// <summary>
        /// Determines whether the current set is a proper (strict) superset of a specified collection.
        /// </summary>
        /// 
        /// <returns>
        /// true if the current set is a proper superset of <paramref name="other"/>; otherwise, false.
        /// </returns>
        /// <param name="other">The collection to compare to the current set. </param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool IsProperSupersetOf(IEnumerable<T> other);

        /// <summary>
        /// Determines whether the current set is a proper (strict) subset of a specified collection.
        /// </summary>
        /// 
        /// <returns>
        /// true if the current set is a proper subset of <paramref name="other"/>; otherwise, false.
        /// </returns>
        /// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool IsProperSubsetOf(IEnumerable<T> other);

        /// <summary>
        /// Determines whether the current set overlaps with the specified collection.
        /// </summary>
        /// 
        /// <returns>
        /// true if the current set and <paramref name="other"/> share at least one common element; otherwise, false.
        /// </returns>
        /// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool Overlaps(IEnumerable<T> other);

        /// <summary>
        /// Determines whether the current set and the specified collection contain the same elements.
        /// </summary>
        /// 
        /// <returns>
        /// true if the current set is equal to <paramref name="other"/>; otherwise, false.
        /// </returns>
        /// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool SetEquals(IEnumerable<T> other);
    }
    
    public interface ISetView : ICollection
    {
        /// <summary>
        /// Determines whether a set is a subset of a specified collection.
        /// </summary>
        /// 
        /// <returns>
        /// true if the current set is a subset of <paramref name="other"/>; otherwise, false.
        /// </returns>
        /// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool IsSubsetOf(IEnumerable other);

        /// <summary>
        /// Determines whether the current set is a superset of a specified collection.
        /// </summary>
        /// 
        /// <returns>
        /// true if the current set is a superset of <paramref name="other"/>; otherwise, false.
        /// </returns>
        /// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool IsSupersetOf(IEnumerable other);

        /// <summary>
        /// Determines whether the current set is a proper (strict) superset of a specified collection.
        /// </summary>
        /// 
        /// <returns>
        /// true if the current set is a proper superset of <paramref name="other"/>; otherwise, false.
        /// </returns>
        /// <param name="other">The collection to compare to the current set. </param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool IsProperSupersetOf(IEnumerable other);

        /// <summary>
        /// Determines whether the current set is a proper (strict) subset of a specified collection.
        /// </summary>
        /// 
        /// <returns>
        /// true if the current set is a proper subset of <paramref name="other"/>; otherwise, false.
        /// </returns>
        /// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool IsProperSubsetOf(IEnumerable other);

        /// <summary>
        /// Determines whether the current set overlaps with the specified collection.
        /// </summary>
        /// 
        /// <returns>
        /// true if the current set and <paramref name="other"/> share at least one common element; otherwise, false.
        /// </returns>
        /// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool Overlaps(IEnumerable other);

        /// <summary>
        /// Determines whether the current set and the specified collection contain the same elements.
        /// </summary>
        /// 
        /// <returns>
        /// true if the current set is equal to <paramref name="other"/>; otherwise, false.
        /// </returns>
        /// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool SetEquals(IEnumerable other);
    }
        
    /// <summary>
    /// Provides the base interface for the abstraction of sets.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    public interface ISet<T> : ISetView<T>, System.Collections.Generic.ISet<T>
    { 
        /// <summary>
        /// UnionWith
        /// </summary>
        /// <param name="items"></param>
        void Add(IEnumerable<T> items);

        /// <summary>
        /// SymmetricExceptWith
        /// </summary>
        /// <param name="items"></param>
        void Remove(IEnumerable<T> items);        
    }

    /// <summary>
    /// Provides the base interface for the abstraction of sets.
    /// </summary>    
    public interface ISet : ICollection, ISetView
    {
        /// <summary>
        /// UnionWith
        /// </summary>
        /// <param name="items"></param>
        void Add(IEnumerable items);

        /// <summary>
        /// SymmetricExceptWith
        /// </summary>
        /// <param name="items"></param>
        void Remove(IEnumerable items);
        
        bool Add(object item);

        bool Remove(object item);

        void ExceptWith(IEnumerable other);

        void IntersectWith(IEnumerable other);

        void SymmetricExceptWith(IEnumerable other);

        void UnionWith(IEnumerable other);
    }
}
