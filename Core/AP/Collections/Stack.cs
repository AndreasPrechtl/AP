using System;
using System.Collections.Generic;
using System.Collections;
using SCG = System.Collections.Generic;

namespace AP.Collections;

public class Stack<T> : IStack<T>, IEqualityComparerUser<T>
{
    private readonly System.Collections.Generic.Stack<T> _inner;
    private readonly IEqualityComparer<T> _comparer;

    public Stack()
        : this(0)
    { }

    public Stack(IEqualityComparer<T> comparer)
        : this(0, comparer)
    { }

    public Stack(int capacity)
        : this(capacity, null)
    { }
    
    public Stack(int capacity, IEqualityComparer<T> comparer)
        : this(new SCG.Stack<T>(capacity), comparer ?? EqualityComparer<T>.Default)
    { }

    public Stack(IEnumerable<T> collection)
        : this(collection, null)
    { }

    public Stack(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        : this(collection == null ? null : new SCG.Stack<T>(collection), comparer ?? EqualityComparer<T>.Default)
    { }

    protected Stack(SCG.Stack<T> inner, IEqualityComparer<T> comparer)
    {
        ArgumentNullException.ThrowIfNull(inner);

        ArgumentNullException.ThrowIfNull(comparer);

        _inner = inner ?? new SCG.Stack<T>(0);
        _comparer = comparer;
    }

    public override string ToString() => _inner.ToString();

    public static Stack<T> Wrap(SCG.Stack<T> stack, IEqualityComparer<T> comparer) => new(stack, comparer);

    public static implicit operator AP.Collections.Stack<T>(SCG.Stack<T> stack)
    {
        return new AP.Collections.Stack<T>(stack, EqualityComparer<T>.Default);
    }

    public static implicit operator SCG.Stack<T>(AP.Collections.Stack<T> stack)
    {
        return stack._inner;
    }

    #region IStack<T> Members

    public T Pop() => _inner.Pop();

    public IEnumerable<T> Pop(int count)
    {
        for (int i = 0; i < count; ++i)
            yield return _inner.Pop();
    }

    public void Push(T item) => _inner.Push(item);

    public void Push(IEnumerable<T> items)
    {
        foreach (T item in items)
            _inner.Push(item);
    }

    public T Peek() => _inner.Peek();

    public void Clear() => _inner.Clear();

    #endregion

    #region ICollection<T> Members

    public int Count => _inner.Count;

    public bool Contains(T item)
    {
        foreach (T current in _inner)
            if (_comparer.Equals(item, current))
                return true;
                    
        return false;
    }

    #endregion

    #region IEnumerable<T> Members

    public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

    #endregion

    #region IEnumerable Members

    IEnumerator IEnumerable.GetEnumerator() => _inner.GetEnumerator();

    #endregion

    #region ICloneable Members

    public Stack<T> Clone() => this.OnClone();

    protected virtual Stack<T> OnClone() => new(this, _comparer);

    object ICloneable.Clone() => this.OnClone();

    #endregion

    #region IEqualityComparerUser<T> Members

    public IEqualityComparer<T> Comparer => _comparer;

    #endregion
}
