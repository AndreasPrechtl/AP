using System;
using System.Collections.Generic;

namespace AP.Collections.ObjectModel;

public abstract class ExtendableStack<T> : StackBase<T>, IEqualityComparerUser<T>
{
    private readonly AP.Collections.Stack<T> _inner;

    protected Stack<T> Inner => _inner;

    protected ExtendableStack()
        : this(0, null)
    { }

    protected ExtendableStack(IEqualityComparer<T> comparer)
        : this(0, comparer)
    { }

    protected ExtendableStack(int capacity)
        : this(capacity, null)
    { }

    protected ExtendableStack(int capacity, IEqualityComparer<T> comparer)
        : this(new Stack<T>(capacity, comparer))
    { }

    protected ExtendableStack(IEnumerable<T> collection)
        : this(collection, null)
    { }

    protected ExtendableStack(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        : this(new Stack<T>(collection, comparer))
    { }

    protected ExtendableStack(AP.Collections.Stack<T> inner)
    {
        ArgumentNullException.ThrowIfNull(inner);
        _inner = inner;
    }

    public new ExtendableStack<T> Clone() => (ExtendableStack<T>)this.OnClone();

    public override string ToString() => _inner.ToString();

    #region IStack<T> Members

    public override void Push(T item) => _inner.Push(item);

    public override void Push(IEnumerable<T> items)
    {
        foreach (T item in items)
            this.Push(item);
    }

    public override T Pop() => _inner.Pop();

    public override IEnumerable<T> Pop(int count)
    {
        if (count < 1 || count > this.Count)
            throw new ArgumentOutOfRangeException(nameof(count));

        for (int i = 0; i < count; ++i)
            yield return this.Pop();
    }

    public override T Peek() => _inner.Peek();

    #endregion

    #region ICollection<T> Members

    public override void Clear() => _inner.Clear();

    public override bool Contains(T item) => _inner.Contains(item);

    public override void CopyTo(T[] array, int arrayIndex) => _inner.CopyTo(array, arrayIndex);

    public override int Count => _inner.Count;

    #endregion

    #region IEnumerable<T> Members

    public override IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

    #endregion

    #region IEqualityComparerUser<T> Members

    public IEqualityComparer<T> Comparer => _inner.Comparer;

    #endregion
}
