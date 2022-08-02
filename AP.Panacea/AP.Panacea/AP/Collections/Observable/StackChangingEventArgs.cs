using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Collections.Observable
{
    public class StackChangingEventArgs<T> : CollectionChangingEventArgs<T>
    {
        public new StackChangeType Type { get { return (StackChangeType)base.Type; } }
        public new IStack<T> Source { get { return (IStack<T>)base.Source; } }

        protected StackChangingEventArgs(IStack<T> source, ICollection<T> newItems = null, ICollection<T> oldItems = null, StackChangeType type = StackChangeType.Push)
            : base(source, newItems, oldItems, (ChangeType)type)
        { }

        public static StackChangingEventArgs<T> Push(IStack<T> source, ICollection<T> newItems)
        {
            return new StackChangingEventArgs<T>(source, newItems, null, StackChangeType.Push);
        }

        public static StackChangingEventArgs<T> Pop(IStack<T> source, ICollection<T> oldItems)
        {
            return new StackChangingEventArgs<T>(source, null, oldItems, StackChangeType.Pop);
        }

        public static StackChangingEventArgs<T> Clear(IStack<T> source, ICollection<T> clearedItems)
        {
            return new StackChangingEventArgs<T>(source, null, clearedItems, StackChangeType.Clear);
        }
    }
}
