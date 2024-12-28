using System;
using System.Collections.Generic;
using AP.Collections;
using AP.Collections.ReadOnly;

namespace AP.Linq;

public static class QueueExtensions
{
    public static ReadOnlyList<TElement> AsReadOnly<TElement>(this IQueue<TElement> queue) => new(queue);
    public static ReadOnlyList<TElement> AsReadOnly<TElement>(this System.Collections.Generic.Queue<TElement> queue) => new(queue);

    public static void Enqueue<T>(this IQueue<T> queue, IEnumerable<T> items)
    {
        foreach (T item in items)
            queue.Enqueue(item);
    }

    public static IEnumerable<T> Dequeue<T>(this IQueue<T> queue, int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);

        T[] array = new T[count];

        for (int i = 0; i < count; ++i)
            array[i] = queue.Dequeue();

        return array;
    }
}
