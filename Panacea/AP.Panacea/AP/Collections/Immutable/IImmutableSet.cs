#if fx45
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections.Immutable
{
    public interface IImmutableSet : ISetView
    {
        IImmutableSet Add(object item);
        IImmutableSet Remove(object item);
        IImmutableSet Clear();

        IImmutableSet Intersect(IEnumerable other);
        IImmutableSet Union(IEnumerable other);
        IImmutableSet Except(IEnumerable other);
        IImmutableSet SymmetricExcept(IEnumerable other);
    }

    public interface IImmutableSet<T> : ISetView<T>, System.Collections.Immutable.IImmutableSet<T>
    {
        new IImmutableSet<T> Intersect(IEnumerable<T> other);
        new IImmutableSet<T> Union(IEnumerable<T> other);
        new IImmutableSet<T> Except(IEnumerable<T> other);
        new IImmutableSet<T> SymmetricExcept(IEnumerable<T> other);
    }
}
#endif