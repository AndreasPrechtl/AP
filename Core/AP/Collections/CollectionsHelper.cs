using System;
using System.Collections;
using AP.Linq;
using SCG = System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using AP.Reflection;

namespace AP.Collections;

internal static class CollectionsHelper
{
    public static bool Contains<T>(ICollection<T> collection, object item) => IsCompatible<T>(item) && collection.Contains((T)item);

    public static bool IsCompatible<T>(object value)
    {
        if (value is T)
            return true;
        if (value == null)
            return object.ReferenceEquals(null, default(T));
        
        return false;
    }

    public static void CopyTo<T>(ICollection<T> collection, Array array, int index)
    {
        ArgumentNullException.ThrowIfNull(array);

        if (array is T[])
            collection.CopyTo((T[])array, index);
        else
        {
            if (index < 0  || index >= collection.Count)
                throw new IndexOutOfRangeException("index");

            foreach (T item in collection)
                array.SetValue(item, index++);
        }
    }

    public static void CopyTo<T>(ICollection<T> collection, T[] array, int index)
    {
        ArgumentNullException.ThrowIfNull(array);

        if (index < 0 || index >= collection.Count)
            throw new IndexOutOfRangeException("index");
        
        foreach (T item in collection)
            array[index++] = item;
    }

    public static bool IsSorted<T>(IEnumerable<T> collection)
    {
        if (collection is IOrderedEnumerable<T>)
            return true;

        if (collection is IOrderedQueryable)
            return true;

        if (collection is SCG.SortedSet<T>)
            return true;

        if (collection is OrderedParallelQuery<T>)
            return true;

        Type t = collection.GetType();

        if (t.IsGenericType)
        {
            t = t.GetGenericTypeDefinition();
            if (t.Is(typeof(SCG.SortedDictionary<,>)))
                return true;

            if (t.Is(typeof(SCG.SortedList<,>)))
                return true;

            if (t.Is(typeof(SCG.SortedSet<>)))
                return true;
        }
        return false;
    }
    public static bool IsSorted<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        where TKey : notnull
    {
        if (collection is SCG.SortedDictionary<TKey, TValue>)
            return true;

        if (collection is SCG.SortedList<TKey, TValue>)
            return true;

        return false;
    }

    public static bool IsSorted(IEnumerable collection)
    {
        if (collection is IOrderedQueryable)
            return true;

        Type t = collection.GetType();

        if (t.IsGenericType)
        {
            t = t.GetGenericTypeDefinition();

            if (t.Is(typeof(IOrderedEnumerable<>)))
                return true;
            
            if (t.Is(typeof(OrderedParallelQuery<>)))
                return true;
            
            if (t.Is(typeof(SCG.SortedDictionary<,>)))
                return true;

            if (t.Is(typeof(SCG.SortedSet<>)))
                return true;

            if (t.Is(typeof(SCG.SortedList<,>)))
                return true;
        }
        return false;
    }

    public static string ToString<T>(ICollection<T> collection)
    {
        StringBuilder sb = new();

        sb.Append('{');

        int count = collection.Count;
        foreach (T value in collection)
        {
            sb.Append(value);
            sb.Append(',');
        }
        if (count > 0)
            sb.Remove(sb.Length - 1);

        sb.Append('}');

        return sb.ToString();
    }
    

    public static void Replace<T>(System.Collections.Generic.ICollection<T> collection, IEnumerable<T> items, IEnumerable<T> newItems)
    {
        SCG.List<T> list = new(collection.Count);
        SCG.List<T> newList = new(collection.Count);
        
        IEnumerator<T> enumerator1 = items.GetEnumerator();
        IEnumerator<T> enumerator2 = items.GetEnumerator();
        
        while (enumerator1.MoveNext() && enumerator2.MoveNext())
        {
            T item = enumerator1.Current;

            if (collection.Contains(item))
            {
                list.Add(item);
                newList.Add(enumerator2.Current);
            }
        }

        if (enumerator1.MoveNext() || enumerator2.MoveNext())
            throw new ArgumentOutOfRangeException("items and newItems do not have the same size");
        
        enumerator1 = list.GetEnumerator();
        enumerator2 = newList.GetEnumerator();

        while (enumerator1.MoveNext() && enumerator2.MoveNext())
        {
            collection.Remove(enumerator1.Current);
            collection.Add(enumerator2.Current);
        }
    }

    public static void Replace<T>(System.Collections.Generic.IList<T> list, IEnumerable<T> items, IEnumerable<T> newItems)
    {
        var enumerator1 = items.GetEnumerator();
        var enumerator2 = newItems.GetEnumerator();

        SCG.List<int> indexes = new(list.Count);
        SCG.List<T> newItemsList = new(list.Count);

        while (enumerator1.MoveNext() && enumerator2.MoveNext())
        {
            T item = enumerator1.Current;

            int index = list.IndexOf(item);

            if (index > -1)
            {
                indexes.Add(index);
                newItemsList.Add(enumerator2.Current);
            }
        }
        if (enumerator1.MoveNext() || enumerator2.MoveNext())
            throw new ArgumentOutOfRangeException("items and newItems have not the same size");
  
        enumerator2 = newItemsList.GetEnumerator();
        foreach (int index in indexes)
        {
            enumerator2.MoveNext();
            list[index] = enumerator2.Current;
        }
    }

    public static void Move<T>(System.Collections.Generic.IList<T> list, int index, int newIndex, int count = 1)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);
        ArgumentOutOfRangeException.ThrowIfEqual(index, newIndex);

        int maxIndex = list.Count - count;

        if (index < 0 || index > maxIndex)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (newIndex < 0 || newIndex > maxIndex)
            throw new ArgumentOutOfRangeException(nameof(newIndex));

        if (count == 1)
        {
            T tmp = list[index]; 
            
            list.Insert(newIndex, tmp);
            
            if (index > newIndex)
                index--;
            
            list.RemoveAt(index);
        }
        else
        {
            T[] array = new T[count];

            int currentIndex = index;

            for (int i = 0; i < count; ++i)
                array[i] = list[currentIndex++];

            list.Insert(newIndex, array);

            if (index > newIndex)
                index += count;

            list.RemoveAt(index, count);
        }
    }
}