using System.Collections.Generic;

namespace AP.Collections;

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
    void Add(params IEnumerable<T> items);

    /// <summary>
    /// SymmetricExceptWith
    /// </summary>
    /// <param name="items"></param>
    void Remove(params IEnumerable<T> items);        
}
