using System.Collections;
using System.Collections.Generic;

namespace AP.Collections;

public interface IEqualityComparerUser
{
    IEqualityComparer Comparer { get; }
}

public interface IEqualityComparerUser<in T>
{
    IEqualityComparer<T> Comparer { get; }
}

public interface IComparerUser
{
    IComparer Comparer { get; }
}

public interface IComparerUser<in T> //: IComparerUser
{
    IComparer<T> Comparer { get; }
}
