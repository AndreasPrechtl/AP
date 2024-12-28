using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Collections.Observable
{
    public class StackChangedEventArgs<T> : CollectionChangedEventArgs<T>
    {
        public new StackChangeType Type { get { return (StackChangeType)base.Type; } }
        public new IStack<T> Source { get { return (IStack<T>)base.Source; } }

        protected StackChangedEventArgs(IStack<T> source, ICollection<T> newItems = null, ICollection<T> oldItems = null, StackChangeType type = StackChangeType.Push)
            : base(source, newItems, oldItems, (ChangeType)type)
        { }

        public static StackChangedEventArgs<T> Push(IStack<T> source, ICollection<T> newItems)
        {
            return new StackChangedEventArgs<T>(source, newItems, null, StackChangeType.Push);
        }

        public static StackChangedEventArgs<T> Pop(IStack<T> source, ICollection<T> oldItems)
        {
            return new StackChangedEventArgs<T>(source, null, oldItems, StackChangeType.Pop);
        }

        public static StackChangedEventArgs<T> Clear(IStack<T> source, ICollection<T> clearedItems)
        {
            return new StackChangedEventArgs<T>(source, null, clearedItems, StackChangeType.Clear);
        }
    }
}
