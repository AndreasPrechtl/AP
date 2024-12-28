#if fx45
using System;
using System.Collections;
using System.Collections.Generic;
using SCI = System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections.Immutable
{
    public interface IImmutableStack : ICollectionView
    {
        IImmutableStack Pop(out object item);
        IImmutableStack Pop(out IEnumerable items, int count);
        IImmutableStack Push(object item);
        IImmutableStack Push(IEnumerable items);
        IImmutableStack Clear();
        object Peek();
    }

    public interface IImmutableStack<T> : SCI.IImmutableStack<T>, ICollectionView<T>
    {
        new IImmutableStack<T> Push(T item);
        IImmutableStack<T> Push(IEnumerable<T> items);
        IImmutableStack<T> Pop(out T item);
        new IImmutableStack<T> Clear();
    }
}
#endif