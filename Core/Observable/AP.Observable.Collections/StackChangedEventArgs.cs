using AP.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Observable.Collections
{
    public class StackChangedEventArgs<T> : CollectionChangedEventArgs<T>
    {
        public new StackChangeType Type { get { return (StackChangeType)base.Type; } }
        public new IStack<T> Source { get { return (IStack<T>)base.Source; } }

        protected StackChangedEventArgs(IStack<T> source, IListView<T> newItems = null, IListView<T> oldItems = null, StackChangeType type = StackChangeType.Push)
            : base(source, newItems, oldItems, (ChangeType)type)
        { }

        public static StackChangedEventArgs<T> Push(IStack<T> source, IListView<T> newItems)
        {
            return new StackChangedEventArgs<T>(source, newItems, null, StackChangeType.Push);
        }

        public static StackChangedEventArgs<T> Pop(IStack<T> source, IListView<T> oldItems)
        {
            return new StackChangedEventArgs<T>(source, null, oldItems, StackChangeType.Pop);
        }

        public static StackChangedEventArgs<T> Clear(IStack<T> source, IListView<T> clearedItems)
        {
            return new StackChangedEventArgs<T>(source, null, clearedItems, StackChangeType.Clear);
        }
    }
}
