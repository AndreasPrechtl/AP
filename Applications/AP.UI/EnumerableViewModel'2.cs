using System;
using System.Collections.Generic;
using AP.Linq;

namespace AP.UI
{
    public sealed class EnumerableViewModel<T, TKey> : IViewModel<T>
        where TKey : notnull
    {
        private readonly ResultSet<T> _results;

        private static ResultSet<T> CreateResultSet(IEnumerable<T> source, TKey currentKey, KeySelector<T, TKey> keySelector, SortDirection sortDirection, IComparer<TKey> keyComparer)
        {
            // no longer pre-sorting - makes it slow! 

            int count = 0;
                
            T? first = default;
            T? previous = default;
            T? current = default;
            T? next = default;
            T? last = default;

            TKey? firstKey = default;
            TKey? previousKey = default;
            TKey? nextKey = default;
            TKey? lastKey = default;

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

        public SortDirection SortDirection => _results.SortDirection;

        public EnumerableViewModel(IEnumerable<T> source, KeySelector<T, TKey> keySelector, TKey currentKey, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey>? keyComparer = null)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(keySelector);
            ArgumentNullException.ThrowIfNull(currentKey);
            ArgumentOutOfRangeException.ThrowIfEqual((int)sortDirection, (int)SortDirection.Unsorted);

            _results = CreateResultSet(source, currentKey, keySelector, sortDirection, keyComparer ?? Comparer<TKey>.Default);
        }

        #region IViewModel<T> Members

        public int Count => _results.Count;

        public T First => _results.First;
        public T Previous => _results.Previous;
        public T Current => _results.Current;
        public T Next => _results.Next;
        public T Last => _results.Last;

        public bool HasFirst => _results.HasFirst;
        public bool HasPrevious => _results.HasPrevious;
        public bool HasCurrent => _results.HasCurrent;
        public bool HasNext => _results.HasNext;
        public bool HasLast => _results.HasLast;

        #endregion
    }
}
