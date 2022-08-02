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
    public interface IImmutableQueue : ICollectionView
    {
        IImmutableQueue Dequeue();
        IImmutableQueue Enqueue(object item);
        IImmutableQueue Clear();
        object Peek();
        bool IsEmpty { get; }
    }

    public interface IImmutableQueue<T> : SCI.IImmutableQueue<T>, ICollectionView<T>
    {
        IImmutableQueue<T> Dequeue(out T item);
        new IImmutableQueue<T> Enqueue(T item);
        new IImmutableQueue<T> Clear();
    }
}
#endif