using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AP.Collections;
using AP.Collections.ReadOnly;
using AP.Linq;
using AP.Reflection;

namespace AP.UI
{
    public sealed class QueryablePagedViewModel<T, TKey> : IPagedViewModel<T>
        where TKey : notnull
    {
        private PagedResultSet<T> Results => _results;

        private static PagedResultSet<T> CreateResultSet(IQueryable<T> source, Expression<Func<T, TKey>> keySelector, SortDirection sortDirection, int currentPage, int pageSize, IComparer<TKey>? keyComparer)
        {
            //if (sortDirection == SortDirection.Unsorted)
            //    throw new ArgumentOutOfRangeException("sortDirection");

            int currentPageStartIndex = currentPage * pageSize;
            int previousPageStartIndex = currentPageStartIndex - pageSize;
            int nextPageStartIndex = currentPageStartIndex + pageSize;


            var sorted = sortDirection == SortDirection.Unsorted ? source : source.Sort(keySelector, sortDirection, keyComparer);

            int count = source.Count();

            // truncates the int (-> no need for ceiling and subtracting 1)
            int lastPageStartIndex = (count / pageSize) * pageSize;

            var q = from c in source
                    select new
                    {
                        First = sorted.Take(pageSize),
                        Previous = sorted.Skip(previousPageStartIndex).Take(pageSize),
                        Current = sorted.Skip(currentPageStartIndex).Take(pageSize),
                        Next = sorted.Skip(nextPageStartIndex).Take(pageSize),
                        Last = sorted.Skip(lastPageStartIndex).Take(pageSize)
                    };

            //var asc = source.Sort(keySelector, SortDirection.Ascending, keyComparer);
            //var desc = source.Sort(keySelector, SortDirection.Descending, keyComparer);

            //var q = from c in source
            //        let count = source.Count()
            //        let skip = count - pageSize 
            //        select new
            //        {
            //            Count = count,
            //            First = asc.Take(pageSize),
            //            Previous = asc.Skip(previousPageStartIndex).Take(pageSize),
            //            Current = asc.Skip(currentPageStartIndex).Take(pageSize),
            //            Next = asc.Skip(nextPageStartIndex).Take(pageSize),
            //            Last = asc.Skip(skip).Take(pageSize) //desc.Take(pageSize).OrderBy(keySelector)
            //        };

            var res = q.FirstOrDefault();

            if (res == null)
                return new PagedResultSet<T>(ReadOnlyList<T>.Empty, ReadOnlyList<T>.Empty, ReadOnlyList<T>.Empty, ReadOnlyList<T>.Empty, ReadOnlyList<T>.Empty, pageSize, currentPage, count, sortDirection);

            return new PagedResultSet<T>
            (   
                new AP.Collections.List<T>(pageSize) { res.First }, 
                new AP.Collections.List<T>(pageSize) { res.Previous }, 
                new AP.Collections.List<T>(pageSize) { res.Current }, 
                new AP.Collections.List<T>(pageSize) { res.Next }, 
                new AP.Collections.List<T>(pageSize) { res.Last }, 
                pageSize, currentPage, count, sortDirection
            );
            
        }

        private readonly PagedResultSet<T> _results;

        public QueryablePagedViewModel(IQueryable<T> source, Expression<KeySelector<T, TKey>> keySelector, int currentPage = 0, int pageSize = 1, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey>? keyComparer = null)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(keySelector);

            if (keyComparer == null && !keySelector.ReturnType.Is(typeof(IComparable<TKey>)))
                keyComparer = Comparer<TKey>.Default;

            _results = CreateResultSet(source, keySelector.Cast<Func<T, TKey>>(), sortDirection, currentPage, pageSize, keyComparer);
        }

        #region IPagedViewModel<T,IEnumerable<T>> Members

        public int PageCount => this.Results.PageCount;
        public int PageSize => this.Results.PageSize;
        public int CurrentPage => this.Results.CurrentPage;
        public int Count => this.Results.Count;

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
