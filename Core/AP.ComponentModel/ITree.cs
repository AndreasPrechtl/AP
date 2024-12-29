using System.Collections.Generic;

namespace AP.ComponentModel;

public interface ITree
{
    object Root { get; }
    object Parent { get; }
    ICollection<ITree> Siblings { get; }
    ICollection<ITree> Children { get; }
}

public interface ITree<T>
    where T : ITree<T>
{
    T Root { get; }
    T Parent { get; }
    ICollection<T> Siblings { get; }
    ICollection<T> Children { get; }
}
