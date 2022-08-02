using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using AP.Collections;
using AP.Collections.ReadOnly;

namespace AP.Linq
{
    public static class ListExtensions
    {
        //public static void MoveItemsTo(this IList from, IList to)
        //{
        //    for (IEnumerator enumerator = from.GetEnumerator(); enumerator.MoveNext(); )
            //{
            //    object current = enumerator.Current;
            //    enumerator = null;
            //    from.Remove(current);
            //    to.Add(current);
            //    enumerator = from.GetEnumerator();
            //}
        //}
        public static ReadOnlyList<TElement> AsReadOnly<TElement>(this System.Collections.Generic.IList<TElement> source)
        {
            ExceptionHelper.AssertNotNull(() => source);

            return new ReadOnlyList<TElement>(source);
        }

        public static void Insert<T>(this System.Collections.Generic.IList<T> list, int index, IEnumerable<T> items)
        {
            if (list is AP.Collections.List<T>)
                ((AP.Collections.List<T>)list).Insert(index, items);
            else if (list is AP.Collections.ObjectModel.ExtendableList<T>)
                ((AP.Collections.ObjectModel.ExtendableList<T>)list).Insert(index, items);
            else if (list is System.Collections.Generic.List<T>)
                ((System.Collections.Generic.List<T>) list).InsertRange(index, items);
            else
            {
                foreach (T item in items)
                    list.Insert(index++, item);
            }
        }

        public static void RemoveAt<T>(this System.Collections.Generic.IList<T> list, int index, int count)
        {
            if (list is AP.Collections.List<T>)
                ((AP.Collections.List<T>)list).Remove(index, count);
            else if (list is AP.Collections.ObjectModel.ExtendableList<T>)
                ((AP.Collections.ObjectModel.ExtendableList<T>)list).Remove(index, count);
            else if (list is System.Collections.Generic.List<T>)
                ((System.Collections.Generic.List<T>)list).RemoveRange(index, count);
            else
            {
                if (index < 0 || index + count >= list.Count)
                    throw new ArgumentOutOfRangeException("index or count");

                for (int i = 0; i < count; ++i)
                    list.RemoveAt(index++);
            }
        }

        public static void Move<T>(this System.Collections.Generic.IList<T> list, int index, int newIndex, int count = 1)
        {
            if (list is AP.Collections.IUnsortedList<T>)
                ((AP.Collections.IUnsortedList<T>)list).Move(index, newIndex, count);
            else
                CollectionsHelper.Move<T>(list, index, newIndex, count);            
        }
        public static bool TryGetItem<T>(this System.Collections.Generic.IList<T> list, int index, out T value)
        {
            if (list is AP.Collections.IListView<T>)
                return ((AP.Collections.IListView<T>)list).TryGetItem(index, out value);

            if (list is AP.Collections.IUnsortedList<T>)
                return ((AP.Collections.IUnsortedList<T>)list).TryGetItem(index, out value);

            if (index < list.Count)
            {
                value = list[index];
                return true;
            }

            value = default(T);
            return false;
        }
    }
}
