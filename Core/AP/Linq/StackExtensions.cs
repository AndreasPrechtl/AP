using AP.Collections;
using AP.Collections.ReadOnly;
using System;
using System.Collections.Generic;

namespace AP.Linq;

public static class StackExtensions
{
    public static ReadOnlyList<TElement> AsReadOnly<TElement>(this IStack<TElement> stack) => new(stack);
    public static ReadOnlyList<TElement> AsReadOnly<TElement>(this System.Collections.Generic.Stack<TElement> stack) => new(stack);
    public static void Push<T>(this System.Collections.Generic.Stack<T> stack, IEnumerable<T> items)
    {
        foreach (T item in items)
            stack.Push(item);
    }
    
    public static IEnumerable<T> Pop<T>(this IStack<T> stack, int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);

        T[] array = new T[count];

        for (int i = 0; i < count; ++i)
            array[i] = stack.Pop();

        return array;
    }
}
