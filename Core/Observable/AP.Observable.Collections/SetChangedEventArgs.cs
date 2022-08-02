using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Collections;

namespace AP.Observable.Collections
{
    public class SetChangedEventArgs<T> : CollectionChangedEventArgs<T>
    {
        public new SetChangeType Type { get { return (SetChangeType)base.Type; } }
        public new ISetView<T> Source { get { return (ISetView<T>)base.Source; } }

        public new ISetView<T> OldItems { get { return (ISetView<T>)base.OldItems; } }
        public new ISetView<T> NewItems { get { return (ISetView<T>)base.NewItems; } }

        protected SetChangedEventArgs(ISetView<T> source, ISetView<T> newItems = null, ISetView<T> oldItems = null, SetChangeType type = SetChangeType.Add)
            : base(source, newItems, oldItems, (ChangeType)type)
        { }

        public static SetChangedEventArgs<T> Add(AP.Collections.ISet<T> source, ISetView<T> addedItems)
        {
            return Union(source, addedItems);
        }

        public static SetChangedEventArgs<T> Union(AP.Collections.ISet<T> source, ISetView<T> addedItems)
        {
            return new SetChangedEventArgs<T>(source, addedItems, null, SetChangeType.Union);
        }

        public static SetChangedEventArgs<T> Clear(AP.Collections.ISet<T> source, ISetView<T> clearedItems)
        {
            return new SetChangedEventArgs<T>(source, null, clearedItems, SetChangeType.Clear);
        }

        public static SetChangedEventArgs<T> Remove(AP.Collections.ISet<T> source, ISetView<T> removedItems)
        {
            return Except(source, removedItems);
        }

        public static SetChangedEventArgs<T> Except(AP.Collections.ISet<T> source, ISetView<T> removedItems)
        {
            return new SetChangedEventArgs<T>(source, null, removedItems, SetChangeType.Except);
        }

        public static SetChangedEventArgs<T> Intersect(AP.Collections.ISet<T> source, ISetView<T> removedItems)
        {
            return new SetChangedEventArgs<T>(source, removedItems, null, SetChangeType.Intersect);
        }

        public static SetChangedEventArgs<T> SymmetricExcept(AP.Collections.ISet<T> source, ISetView<T> addedItems, ISetView<T> removedItems)
        {
            return new SetChangedEventArgs<T>(source, addedItems, removedItems, SetChangeType.SymmetricExcept);
        }
    }
}
