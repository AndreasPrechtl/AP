using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AP.Linq
{
    public static class CollectionExtensions
    {
        public static void MoveItemsTo<T>(this ICollection<T> from, ICollection<T> to)
        {
            for (IEnumerator<T> enumerator = from.GetEnumerator(); enumerator.MoveNext(); )
            {                
                T current = enumerator.Current;
                enumerator = null;
                from.Remove(current);
                to.Add(current);
                enumerator = from.GetEnumerator();
            }
        }

        public static void Add<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection is AP.Collections.IList<T>)
                ((AP.Collections.IList<T>)collection).Add(items);
            else if (collection is System.Collections.Generic.ISet<T>)
                ((System.Collections.Generic.ISet<T>)collection).UnionWith(items);
            else if (collection is System.Collections.Generic.List<T>)
                ((System.Collections.Generic.List<T>)collection).AddRange(items);
            else if (collection is AP.Collections.IStack<T>)
                ((AP.Collections.IStack<T>)collection).Push(items);
            else if (collection is AP.Collections.IQueue<T>)
                ((AP.Collections.IQueue<T>)collection).Enqueue(items);
            else
            {
                foreach (T item in items)
                    collection.Add(item);
            }
        }
        
        public static void Replace<T>(this ICollection<T> collection, IEnumerable<T> items, IEnumerable<T> newItems)
        {
            if (collection is System.Collections.Generic.IList<T>)
                AP.Collections.CollectionsHelper.Replace<T>((System.Collections.Generic.IList<T>)collection, items, newItems);
            else
                AP.Collections.CollectionsHelper.Replace<T>(collection, items, newItems);
        }
        
        public static void Remove<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection is ISet<T>)
                ((ISet<T>) collection).ExceptWith(items);
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
        {
            try
            {                
                value = keyedCollection[key];
                return true;
            }
            catch
            {
                value = default(TValue);
                return false;
            }
        }
    }
}
