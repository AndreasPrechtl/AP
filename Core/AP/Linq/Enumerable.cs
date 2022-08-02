using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class Enumerable
    {   
        [MethodImpl(256)]
        public static int Count<TSource>(this IEnumerable<TSource> source)
        {
            int c;

            if (AP.Linq.EnumerableExtensions.HasCount<TSource>(source, out c))
                return c;

            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                while (enumerator.MoveNext())
                    c++;

            return c;        
        }
     
        [MethodImpl(256)]
        public static TSource ElementAt<TSource>(this IEnumerable<TSource> source, int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");

            if (source == null)
                throw new ArgumentNullException("source");

            if (source is System.Collections.Generic.IList<TSource>)
                return ((System.Collections.Generic.IList<TSource>)source)[index];

            if (source is AP.Collections.IListView<TSource>)
                return ((AP.Collections.IListView<TSource>)source)[index];
            
            TSource current;
            
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                Label_0036:
                if (!enumerator.MoveNext())
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                if (index == 0)
                {
                    current = enumerator.Current;
                }
                else
                {
                    index--;
                    goto Label_0036;
                }
            }
            return current;
        }
        
        [MethodImpl(256)]
        public static TSource ElementAtOrDefault<TSource>(this IEnumerable<TSource> source, int index)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            
            if (index >= 0)
            {
                IList<TSource> list;
                AP.Collections.IListView<TSource> view;

                if ((list = source as IList<TSource>) != null && index < list.Count)
                    return list[index];
                else if ((view = source as AP.Collections.IListView<TSource>) != null && index < view.Count)
                    return view[index];
                else
                {
                    using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                        while (enumerator.MoveNext())
                            if (index-- == 0)
                                return enumerator.Current;
                }
            }

            return default(TSource);
        }
        
        [MethodImpl(256)]
        public static TSource First<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            
            IList<TSource> list;
            AP.Collections.IListView<TSource> view;

            if ((list = source as IList<TSource>) != null && list.Count > 0)            
                return list[0];            
            else if ((view = source as AP.Collections.IListView<TSource>) != null && view.Count > 0)
                return view[0];
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                    if (enumerator.MoveNext())
                        return enumerator.Current;

            }
            throw new ArgumentException("no elements");
        }
        
        
        [MethodImpl(256)]
        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            
            IList<TSource> list;
            AP.Collections.IListView<TSource> view;

            if ((list = source as IList<TSource>) != null && list.Count > 0)
                return list[0];
            else if ((view = source as AP.Collections.IListView<TSource>) != null && view.Count > 0)
                return list[0];
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                    if (enumerator.MoveNext())
                        return enumerator.Current;
            }

            return default(TSource);
        }
        
        [MethodImpl(256)]
        public static TSource Last<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
        
            System.Collections.Generic.IList<TSource> list;
            AP.Collections.IListView<TSource> view;

            if ((list = source as IList<TSource>) != null)
            {
                int c = list.Count;
                if (c > 0)
                    return list[c - 1];                
            }
            else if ((view = source as AP.Collections.IListView<TSource>) != null)
            {
                int c = view.Count;
                if (c > 0)
                    return view[c - 1];
            }
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        TSource current;
                        do
                        {
                            current = enumerator.Current;
                        }
                        while (enumerator.MoveNext());
                        return current;
                    }
                }
            }
            
            throw new ArgumentException("no elements");
        }
                
        [MethodImpl(256)]
        public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            
            System.Collections.Generic.IList<TSource> list;
            AP.Collections.IListView<TSource> view;

            if ((list = source as IList<TSource>) != null)
            {
                int c = list.Count;
                if (c > 0)
                    return list[c - 1];                
            }
            else if ((view = source as AP.Collections.IListView<TSource>) != null)
            {
                int c = view.Count;
                if (c > 0)
                    return view[c - 1];
            }
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        TSource current;
                        do
                        {
                            current = enumerator.Current;
                        }
                        while (enumerator.MoveNext());
                        return current;
                    }
                }
            }
            return default(TSource);
        }
        
        [MethodImpl(256)]
        public static TSource Single<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)            
                throw new ArgumentNullException("source");

            IList<TSource> list;
            AP.Collections.IListView<TSource> view;
            if ((list = source as IList<TSource>) != null)
            {
                switch (list.Count)
                {
                    case 0:
                        throw new ArgumentException("no elements");                    
                    case 1:
                        return list[0];
                }
            }
            else if ((view = source as AP.Collections.IListView<TSource>) != null)
            {
                switch (view.Count)
                {
                    case 0:
                        throw new ArgumentException("no elements");
                    case 1:
                        return view[0];
                }
            }
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                {
                    if (!enumerator.MoveNext())
                        throw new ArgumentException("no elements");

                    TSource current = enumerator.Current;
                    if (!enumerator.MoveNext())
                        return current;                    
                }
            }
            throw new ArgumentException("more than one element");
        }
        
        [MethodImpl(256)]
        public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return System.Linq.Enumerable.Single<TSource>(source, predicate);
        }
        
        [MethodImpl(256)]
        public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            IList<TSource> list;
            AP.Collections.IListView<TSource> view;

            if ((list = source as IList<TSource>) != null)
            {
                switch (list.Count)
                {
                    case 0:
                        return default(TSource);                    
                    case 1:
                        return list[0];
                }
            }
            else if ((view = source as AP.Collections.IListView<TSource>) != null)
            {
                switch (view.Count)
                {
                    case 0:
                        return default(TSource);
                    case 1:
                        return view[0];
                }
            }
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                {
                    if (!enumerator.MoveNext())
                    {
                        return default(TSource);
                    }
                    TSource current = enumerator.Current;
                    if (!enumerator.MoveNext())
                    {
                        return current;
                    }
                }
            }

            throw new ArgumentException("more than one element");
        }

        [MethodImpl(256)]
        public static AP.Collections.Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");

            AP.Collections.Dictionary<TKey, TElement> dictionary = new AP.Collections.Dictionary<TKey, TElement>(comparer);
            
            foreach (TSource local in source)
                dictionary.Add(keySelector(local), elementSelector(local));
            
            return dictionary;
        }
        
        [MethodImpl(256)]
        public static AP.Collections.List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            
            return new List<TSource>(source);
        }        
    }
}
