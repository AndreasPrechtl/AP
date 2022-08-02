using AP.Collections;
using AP.Collections.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Linq
{
    public static class StackExtensions
    {
        public static ReadOnlyList<TElement> AsReadOnly<TElement>(this IStack<TElement> stack)
        {
            return new ReadOnlyList<TElement>(stack);
        }
        public static ReadOnlyList<TElement> AsReadOnly<TElement>(this System.Collections.Generic.Stack<TElement> stack)
        {
            return new ReadOnlyList<TElement>(stack);
        }
        public static void Push<T>(this System.Collections.Generic.Stack<T> stack, IEnumerable<T> items)
        {
            foreach (T item in items)
                stack.Push(item);
        }
        
        public static IEnumerable<T> Pop<T>(this IStack<T> stack, int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException("count");
            
            T[] array = new T[count];

            for (int i = 0; i < count; ++i)
                array[i] = stack.Pop();

            return array;
        }
    }
}
