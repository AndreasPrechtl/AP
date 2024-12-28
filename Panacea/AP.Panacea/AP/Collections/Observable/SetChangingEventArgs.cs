using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Collections.Observable
{
    public class SetChangingEventArgs<T> : CollectionChangingEventArgs<T>
    {
        public new SetChangeType Type { get { return (SetChangeType)base.Type; } }
        public new ISetView<T> Source { get { return (ISetView<T>)base.Source; } }

        public SetChangingEventArgs(ISetView<T> source, ICollection<T> newItems = null, ICollection<T> oldItems = null, SetChangeType type = SetChangeType.Add)
            : base(source, newItems, oldItems, (ChangeType)type)
        { }

        public static SetChangingEventArgs<T> Add(ISet<T> source, ISetView<T> addedItems)
        {
            return Union(source, addedItems);
        }

        public static SetChangingEventArgs<T> Union(ISet<T> source, ISetView<T> addedItems)
        {
            return new SetChangingEventArgs<T>(source, addedItems, null, SetChangeType.Union);
        }

        public static SetChangingEventArgs<T> Clear(ISet<T> source, ISetView<T> clearedItems)
        {
            return new SetChangingEventArgs<T>(source, null, clearedItems, SetChangeType.Clear);
        }

        public static SetChangingEventArgs<T> Remove(ISet<T> source, ISetView<T> removedItems)
        {
            return Except(source, removedItems);
        }

        public static SetChangingEventArgs<T> Except(ISet<T> source, ISetView<T> removedItems)
        {
            return new SetChangingEventArgs<T>(source, null, removedItems, SetChangeType.Except);
        }

        public static SetChangingEventArgs<T> Intersect(ISet<T> source, ISetView<T> removedItems)
        {
            return new SetChangingEventArgs<T>(source, removedItems, null, SetChangeType.Intersect);
        }

        public static SetChangingEventArgs<T> SymmetricExcept(ISet<T> source, ISetView<T> addedItems, ISetView<T> removedItems)
        {
            return new SetChangingEventArgs<T>(source, addedItems, removedItems, SetChangeType.SymmetricExcept);
        }
    }
}
