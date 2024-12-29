using System;
using System.Collections.Generic;
using AP.Collections;
using AP.Collections.ReadOnly;

namespace AP.Linq;

public static class ListExtensions
{
    public static ReadOnlyList<TElement> AsReadOnly<TElement>(this System.Collections.Generic.IList<TElement> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new ReadOnlyList<TElement>(source);
    }

    public static void Insert<T>(this System.Collections.Generic.IList<T> list, int index, params IEnumerable<T> items)
    {
        switch (list)
        {
            case Collections.List<T> list1:
                list1.Insert(index, items);
                break;
            case Collections.ObjectModel.ExtendableList<T> list2:
                list2.Insert(index, items);
                break;
            case System.Collections.Generic.List<T> list3:
                list3.InsertRange(index, items);
                break;
            default:
                {
                    foreach (T item in items)
                        list.Insert(index++, item);
                    break;
                }
        }
    }

    public static void RemoveAt<T>(this System.Collections.Generic.IList<T> list, int index, int count)
    {
        if (list is AP.Collections.List<T> list1)
            list1.Remove(index, count);
        else if (list is AP.Collections.ObjectModel.ExtendableList<T> list2)
            list2.Remove(index, count);
        else if (list is System.Collections.Generic.List<T> list3)
            list3.RemoveRange(index, count);
        else
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index + count, list.Count);
            
            for (int i = 0; i < count; ++i)
                list.RemoveAt(index++);
        }
    }

    public static void Move<T>(this System.Collections.Generic.IList<T> list, int index, int newIndex, int count = 1)
    {
        if (list is AP.Collections.IUnsortedList<T> list1)
            list1.Move(index, newIndex, count);
        else
            CollectionsHelper.Move<T>(list, index, newIndex, count);            
    }
    public static bool TryGetItem<T>(this System.Collections.Generic.IList<T> list, int index, out T? value)
    {
        if (list is AP.Collections.IListView<T> view)
            return view.TryGetItem(index, out value);

        if (list is AP.Collections.IUnsortedList<T> list1)
            return list1.TryGetItem(index, out value);

        if (index < list.Count)
        {
            value = list[index];
            return true;
        }

        value = default;
        return false;
    }
}
