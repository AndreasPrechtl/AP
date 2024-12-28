#if fx45
using System;
using System.Collections.Generic;
using SCI = System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections.Immutable
{
    public interface IImmutableList : IListView
    {
        IImmutableList Add(object item);
        
    }

    public interface IImmutableList<T> : SCI.IImmutableList<T>, IListView<T>
    {
        new IImmutableList<T> Add(T item);
        IImmutableList<T> RemoveAt(int index, int count);

        new IImmutableList<T> Insert(int index, T item);

        new IImmutableList<T> Clear();
    }
}
#endif