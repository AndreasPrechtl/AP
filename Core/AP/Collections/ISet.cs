using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections
{
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
}
