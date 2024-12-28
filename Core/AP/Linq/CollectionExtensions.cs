using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AP.Linq;

public static class CollectionExtensions
{
    public static void MoveItemsTo<T>(this ICollection<T> from, ICollection<T> to)
    {
        for (IEnumerator<T> enumerator = from.GetEnumerator(); enumerator.MoveNext(); )
        {                
            T current = enumerator.Current;
            enumerator = null!;
            from.Remove(current);
            to.Add(current);
            enumerator = from.GetEnumerator();
        }
    }

    public static void Add<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        switch (collection)
        {
            case Collections.IList<T>:
                ((Collections.IList<T>)collection).Add(items);
                break;
            case ISet<T>:
                ((ISet<T>)collection).UnionWith(items);
                break;
            case List<T>:
                ((List<T>)collection).AddRange(items);
                break;
            case Collections.IStack<T>:
                ((Collections.IStack<T>)collection).Push(items);
                break;
            case Collections.IQueue<T>:
                ((Collections.IQueue<T>)collection).Enqueue(items);
                break;
            default:
                {
                    foreach (T item in items)
                        collection.Add(item);
                    break;
                }
        }
    }
    
    public static void Replace<T>(this ICollection<T> collection, IEnumerable<T> items, IEnumerable<T> newItems)
    {
        if (collection is System.Collections.Generic.IList<T> list)
            AP.Collections.CollectionsHelper.Replace<T>(list, items, newItems);
        else
            AP.Collections.CollectionsHelper.Replace<T>(collection, items, newItems);
    }
    
    public static void Remove<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        if (collection is ISet<T> set)
            set.ExceptWith(items);
        else
        {
            foreach (T item in items)
                collection.Remove(item);
        }
    }

    public static void CopyItemsTo<T>(this ICollection<T> from, ICollection<T> to)
    {
        foreach (T current in from)
            to.Add(current);            
    }

    public static bool TryGetValue<TKey, TValue>(this KeyedCollection<TKey, TValue> keyedCollection, TKey key, out TValue value)
        where TKey : notnull
    {
        try
        {                
            value = keyedCollection[key];
            return true;
        }
        catch
        {
            value = default!;
            return false;
        }
    }
}
