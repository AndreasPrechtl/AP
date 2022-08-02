using System;
using System.Collections.Generic;
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
    public sealed class QueryablePagedViewModel<T, TNavigation, TKey> : IPagedViewModel<T, TNavigation>
    {
        private PagedResultSet<T, TNavigation> Results
        {
            get { return _results; }
        }

        private static PagedResultSet<T, TNavigation> CreateResultSet(IQueryable<T> source, LinkCreator<TNavigation> linkCreator, Expression<Func<T, TKey>> keySelector, Linq.SortDirection sortDirection, int currentPage, int pageSize, IComparer<TKey> keyComparer)
        {
            int currentPageStartIndex = currentPage * pageSize;

            IQueryable<T> sorted = sortDirection == SortDirection.Unsorted ? source : source.Sort(keySelector, sortDirection, keyComparer);

            var q = from c in sorted
                    select new
                    {
                        Count = source.Count(),
                        Current = sorted.Skip(currentPageStartIndex).Take(pageSize)
                    };

            var res = q.FirstOrDefault();

            if (res == null)
                return new PagedResultSet<T, TNavigation>(null, linkCreator, -1, 0, 0, sortDirection);

            return new PagedResultSet<T, TNavigation>
            (
                new AP.Collections.List<T>(res.Current),
                linkCreator,
                currentPage,
                pageSize,
                res.Count,
                sortDirection
            );
        }

        private readonly PagedResultSet<T, TNavigation> _results;

        public QueryablePagedViewModel(IQueryable<T> source, LinkCreator<TNavigation> toNavigation, Expression<KeySelector<T, TKey>> keySelector, int currentPage = 0, int pageSize = 1, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey> keyComparer = null)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            if (toNavigation == null)
                throw new ArgumentNullException("toNavigation");

            if (keyComparer == null && !keySelector.ReturnType.Is(typeof(IComparable<TKey>)))
                keyComparer = Comparer<TKey>.Default;

            _results = CreateResultSet(source, toNavigation, keySelector.Cast<Func<T, TKey>>(), sortDirection, currentPage, pageSize, keyComparer);
        }

        #region IPagedViewModel<T,TNavigation> Members

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

        public TNavigation First
        {
            get { return this.Results.First; }
        }

        public TNavigation Previous
        {
            get { return this.Results.Previous; }
        }

        public IListView<T> Current
        {
            get { return this.Results.Current; }
        }

        public TNavigation Next
        {
            get
            {
                return this.Results.Next;
            }
        }

        public TNavigation Last
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
