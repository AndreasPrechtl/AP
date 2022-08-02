using System.Collections.Generic;

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
}
