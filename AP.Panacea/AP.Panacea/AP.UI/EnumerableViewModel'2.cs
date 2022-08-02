using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.ComponentModel;
using AP.Linq;
using System.Linq.Expressions;

namespace AP.UI
{
    public sealed class EnumerableViewModel<T, TKey> : IViewModel<T>
    {
        private readonly ResultSet<T> _results;
        
        private ResultSet<T> Results
        {
            get { return _results; }
        }

        private static ResultSet<T> CreateResultSet(IEnumerable<T> source, TKey currentKey, KeySelector<T, TKey> keySelector, SortDirection sortDirection, IComparer<TKey> keyComparer)
        {
            // no longer pre-sorting - makes it slow! 

            int count = 0;
                
            T first = default(T);
            T previous = default(T);
            T current = default(T);
            T next = default(T);
            T last = default(T);

            TKey firstKey = default(TKey);
            TKey previousKey = default(TKey);
            TKey nextKey = default(TKey);
            TKey lastKey = default(TKey);

            bool firstInitialized = false;
            bool lastInitialized = false;

            foreach (T tmp in source)
            {
                TKey tmpKey = keySelector(tmp);

                int crc = keyComparer.Compare(currentKey, tmpKey);

                if (crc > 0)
                {
                    if (firstInitialized)
                    {
                        if (keyComparer.Compare(firstKey, tmpKey) > 0)
                        {
                            first = tmp;
                            firstKey = tmpKey;
                        }
                        else if (keyComparer.Compare(previousKey, tmpKey) < 0)
                        {
                            previous = tmp;
                            previousKey = tmpKey;
                        }
                    }
                    else
                    {
                        // setup initial values or all other comparisons will fail
                        first = previous = tmp;
                        firstKey = previousKey = tmpKey;
                        firstInitialized = true;
                    }
                }
                else if (crc == 0)
                    current = tmp;
                else
                {
                    if (lastInitialized)
                    {
                        if (keyComparer.Compare(lastKey, tmpKey) < 0)
                        {
                            last = tmp;
                            lastKey = tmpKey;
                        }
                        else if (keyComparer.Compare(nextKey, tmpKey) > 0)
                        {
                            next = tmp;
                            nextKey = tmpKey;
                        }
                    }
                    else
                    {
                        // setup initial values or the comparisons will fail
                        last = next = tmp;
                        lastKey = nextKey = tmpKey;
                        lastInitialized = true;
                    }
                }

                count++;
            }

            if (sortDirection == SortDirection.Ascending)
                return new ResultSet<T>(first, previous, current, next, last, count, sortDirection);
            
            // just invert the order of appearance - if it was ordered by descending
            return new ResultSet<T>(last, next, current, previous, first, count, sortDirection);
        }

        public SortDirection SortDirection
        {
            get { return this.Results.SortDirection; }
        }
        
        public EnumerableViewModel(IEnumerable<T> source, KeySelector<T, TKey> keySelector, TKey currentKey, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey> keyComparer = null)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            if (sortDirection == Linq.SortDirection.Unsorted)
                throw new ArgumentException("sortDirection");

            if (currentKey == null)
                throw new ArgumentNullException("currentKey");

            _results = CreateResultSet(source, currentKey, keySelector, sortDirection, keyComparer ?? Comparer<TKey>.Default);
        }

        #region IViewModel<T> Members

        public int Count { get { return this.Results.Count; } }

        public T First
        {
            get { return this.Results.First; }
        }

        public T Previous
        {
            get { return this.Results.Previous; }
        }

        public T Current
        {
            get { return this.Results.Current; }
        }

        public T Next
        {
            get { return this.Results.Next; }
        }

        public T Last
        {
            get { return this.Results.Last; }
        }

        public bool HasFirst { get { return this.Results.HasFirst; } }
        public bool HasPrevious { get { return this.Results.HasPrevious; } }
        public bool HasCurrent { get { return this.Results.HasCurrent; } }
        public bool HasNext { get { return this.Results.HasNext; } }
        public bool HasLast { get { return this.Results.HasLast; } }

        #endregion
    }
}
