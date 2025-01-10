using System;
using System.Collections.Generic;
using AP.Collections;
using AP.Linq;

namespace AP.UI
{
    public sealed class EnumerablePagedViewModel<T, TKey> : IPagedViewModel<T>
    {
        private readonly PagedResultSet<T> _results;

        public EnumerablePagedViewModel(IEnumerable<T> source, KeySelector<T, TKey> keySelector, int currentPage = 0, int pageSize = 1, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey>? keyComparer = null)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(keySelector);
            ArgumentOutOfRangeException.ThrowIfNegative(currentPage);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);

            _results = CreateResultSet(source, keySelector, sortDirection, currentPage, pageSize, keyComparer);
        }

        #region IPagedViewModel<T> Members

        public int Count => this.Results.Count;
        public int PageSize => this.Results.PageSize;
        public int CurrentPage => this.Results.CurrentPage;
        public int PageCount => this.Results.PageCount;

        #endregion
        
        #region IViewModel<IEnumerable<T>> Members
        
        private static PagedResultSet<T> CreateResultSet(IEnumerable<T> source, KeySelector<T, TKey> keySelector, SortDirection sortDirection, int currentPage, int pageSize, IComparer<TKey>? keyComparer)
        {
            IEnumerable<T> sorted = sortDirection == SortDirection.Unsorted ? source : source.Sort(keySelector, sortDirection, keyComparer);

            using (IEnumerator<T> en = sorted.GetEnumerator())
            {
                AP.Collections.List<T> first = [];
                AP.Collections.List<T> previous = currentPage > 0 ? new(pageSize) : [];
                AP.Collections.List<T> current = new(pageSize);
                AP.Collections.List<T> next = [];
                AP.Collections.List<T> last = [];

                int globalIndex = 0;

                int currentStartIndex = currentPage * pageSize;
                int nextStartIndex = currentStartIndex + pageSize;
                int minLastStartIndex = nextStartIndex + pageSize;

                for (int i = 0; en.MoveNext(); i++, globalIndex++)
                {
                    if (globalIndex < currentStartIndex)
                    {
                        // this cannot be null :)
                        if (globalIndex == pageSize)
                        {
                            first = previous;
                            previous = new AP.Collections.List<T>(pageSize);
                        }
                        previous.Add(en.Current);
                    }
                    else if (globalIndex >= minLastStartIndex)
                    {
                        last = last ?? new AP.Collections.List<T>(pageSize);

                        if (globalIndex == minLastStartIndex)
                        {
                            last.Clear();
                            // move the minimum index for last
                            minLastStartIndex += pageSize;
                        }
                        last.Add(en.Current);
                    }
                    else if (globalIndex >= nextStartIndex)
                    {
                        next = next ?? new AP.Collections.List<T>(pageSize);

                        next.Add(en.Current);
                    }
                    else
                    {
                        current.Add(en.Current);
                    }
                }
                return new PagedResultSet<T>(first, previous, current, next, last, pageSize, currentPage, globalIndex + 1, sortDirection);
            }

            //if (first != null && first.Count > 0)
            //    rs.First = first;

            //if (previous != null && previous.Count > 0)
            //    rs.Previous = previous;

            //rs.Current = current;

            //if (next != null && next.Count > 0)
            //    rs.Next = next;

            //if (last != null && last.Count > 0)
            //    rs.Last = last;

            //rs.Count = globalIndex + 1;

            //return rs;

                //bool finished = false;
                //int globalIndex = -1;

                //if (currentPage > 0)
                //{
                //    List<T> first = new List<T>(pageSize);

                //    for (int i = 0; i < pageSize; i++, globalIndex++)
                //    {
                //        en.MoveNext();
                //        first.Add(en.Current);
                //    }

                //    List<T> previous = new List<T>(pageSize);

                //    // no need to skip items and get the previous items
                //    if (currentPage == 1)
                //    {
                //        previous = first;
                //    }
                //    else
                //    {
                //        // skip until previous start index
                //        int previousPageStartIndex = (currentPage * pageSize) - pageSize;

                //        for (; globalIndex < previousPageStartIndex; globalIndex++)
                //            en.MoveNext();

                //        for (int i = 0; i < pageSize; i++, globalIndex++)
                //        {
                //            en.MoveNext();
                //            previous.Add(en.Current);
                //        }
                //    }

                //    rs.First = first.AsEnumerable();
                //    rs.Previous = previous.AsEnumerable();
                //}

                //// stuff the current
                //List<T> current = new List<T>(pageSize);

                //for (int i = 0; i < pageSize; i++, globalIndex++)
                //{
                //    if (en.MoveNext())
                //        current.Add(en.Current);
                //    else
                //    {
                //        finished = true;
                //        break;
                //    }
                //}

                //rs.Current = current;

                //if (finished)
                //{
                //    rs.PageCount = (int)Math.Ceiling((double)globalIndex / pageSize);
                //    rs.Count = globalIndex + 1;
                //    return rs;
                //}

                //// populate next


                //List<T> next = new List<T>(pageSize);

                //for (int i = 0; i < pageSize; i++, globalIndex++)
                //{
                //    if (en.MoveNext())
                //        next.Add(en.Current);
                //    else
                //    {
                //        finished = true;
                //        break;
                //    }
                //}

                //if (next.Count > 0)
                //    rs.Next = next.AsEnumerable();

                //if (finished)
                //{
                //    // set the last - the next could already be the last
                //    rs.Last = rs.Next;
                //    rs.PageCount = (int)Math.Ceiling((double)globalIndex / pageSize);
                //    rs.Count = globalIndex + 1;

                //    return rs;
                //}

                //// create the last set - this is a tricky one - at least if you don't have a count or indexer
                //List<T> last = new List<T>(pageSize);

                //bool clearLast = false;
                //for (int i = 0; en.MoveNext(); i++)
                //{
                //    if (clearLast)
                //    {
                //        last.Clear();
                //        clearLast = false;
                //    }

                //    last.Add(en.Current);
                //    globalIndex++;

                //    // set a flag that states if the list needs to be cleared in the next loop
                //    if (i == pageSize)
                //    {
                //        clearLast = true;
                //        i = 0;
                //    }
                //}

            //    rs.PageCount = (int)Math.Ceiling((double)globalIndex / pageSize);
            //rs.Count = globalIndex + 1;

            //if (last.Count > 0)
            //    rs.Last = last.AsEnumerable();

            //return rs;
        }

        private PagedResultSet<T> Results => _results;

        public IListView<T> First => this.Results.First;
        public IListView<T> Previous => this.Results.Previous;
        public IListView<T> Current => this.Results.Current;
        public IListView<T> Next => this.Results.Next;
        public IListView<T> Last => this.Results.Last;

        public bool HasFirst => this.Results.HasFirst;
        public bool HasPrevious => this.Results.HasPrevious;
        public bool HasCurrent => this.Results.HasCurrent;
        public bool HasNext => this.Results.HasNext;
        public bool HasLast => this.Results.HasLast;

        public SortDirection SortDirection => this.Results.SortDirection;

        #endregion
    }
}
