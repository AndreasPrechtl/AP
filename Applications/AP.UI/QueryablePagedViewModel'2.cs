using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AP.Collections;
using AP.ComponentModel;
using AP.Linq;
using AP.Reflection;

namespace AP.UI
{
    public sealed class QueryablePagedViewModel<T, TKey> : IPagedViewModel<T>
    {
        private PagedResultSet<T> Results
        {
            get { return _results; }
        }

        private static PagedResultSet<T> CreateResultSet(IQueryable<T> source, Expression<Func<T, TKey>> keySelector, Linq.SortDirection sortDirection, int currentPage, int pageSize, IComparer<TKey> keyComparer)
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
                return new PagedResultSet<T>(null, null, null, null, null, pageSize, currentPage, count, sortDirection);

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

        public QueryablePagedViewModel(IQueryable<T> source, Expression<KeySelector<T, TKey>> keySelector, int currentPage = 0, int pageSize = 1, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey> keyComparer = null)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            
            if (keyComparer == null && !keySelector.ReturnType.Is(typeof(IComparable<TKey>)))
                keyComparer = Comparer<TKey>.Default;

            _results = CreateResultSet(source, keySelector.Cast<Func<T, TKey>>(), sortDirection, currentPage, pageSize, keyComparer);
        }

        #region IPagedViewModel<T,IEnumerable<T>> Members

        public int PageCount
        {
            get { return this.Results.PageCount; }
        }

        public int PageSize
        {
            get { return this.Results.PageSize; }
        }

        public int CurrentPage
        {
            get { return this.Results.CurrentPage; }
        }

        public int Count { get { return this.Results.Count; } }

        public IListView<T> First
        {
            get { return this.Results.First; }
        }

        public IListView<T> Previous
        {
            get { return this.Results.Previous; }
        }

        public IListView<T> Current
        {
            get { return this.Results.Current; }
        }

        public IListView<T> Next
        {
            get
            {
                return this.Results.Next;
            }
        }

        public IListView<T> Last
        {
            get
            {
                return this.Results.Last;
            }
        }

        public bool HasFirst { get { return this.Results.HasFirst; } }
        public bool HasPrevious { get { return this.Results.HasPrevious; } }

        public bool HasCurrent { get { return this.Results.HasCurrent; } }

        public bool HasNext { get { return this.Results.HasNext; } }
        public bool HasLast { get { return this.Results.HasLast; } }

        public SortDirection SortDirection { get { return this.Results.SortDirection; } }

        #endregion
    }
}
