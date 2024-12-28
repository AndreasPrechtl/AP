using System;
using System.Collections.Generic;

namespace AP.Collections.ObjectModel;

public abstract class ExtendableQueue<T> : QueueBase<T>, IEqualityComparerUser<T>
{
    private readonly AP.Collections.Queue<T> _inner;

    protected Queue<T> Inner => _inner;

    protected ExtendableQueue()
        : this(0, null)
    { }

    protected ExtendableQueue(IEqualityComparer<T> comparer)
        : this(0, comparer)
    { }

    protected ExtendableQueue(int capacity)
        : this(capacity, null)
    { }

    protected ExtendableQueue(int capacity, IEqualityComparer<T> comparer)
        : this(new Queue<T>(capacity, comparer))
    { }

    protected ExtendableQueue(IEnumerable<T> collection)
        : this(collection, null)
    { }

    protected ExtendableQueue(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        : this(new Queue<T>(collection, comparer))
    { }

    protected ExtendableQueue(AP.Collections.Queue<T> inner)
    {
        ArgumentNullException.ThrowIfNull(inner);
        _inner = inner;
    }

    public new ExtendableQueue<T> Clone() => (ExtendableQueue<T>)this.OnClone();

    public override string ToString() => _inner.ToString();

    #region IQueue<T> Members

    public override void Enqueue(T item) => _inner.Enqueue(item);

    public override void Enqueue(IEnumerable<T> items)
    {
        foreach (T item in items)
            this.Enqueue(item);
    }

    public override T Dequeue() => _inner.Dequeue();

    public override IEnumerable<T> Dequeue(int count)
    {
        if (count < 1 || count > this.Count)
            throw new ArgumentOutOfRangeException(nameof(count));

        for (int i = 0; i < count; ++i) 
            yield return this.Dequeue();
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
