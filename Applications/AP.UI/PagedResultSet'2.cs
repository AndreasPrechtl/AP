using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP.Collections;
using AP.Collections.ReadOnly;
using AP.Linq;

namespace AP.UI
{
    internal sealed class PagedResultSet<T> : IPagedViewModel<T>
    {
        private readonly IListView<T> _first = ReadOnlyList<T>.Empty;
        private readonly IListView<T> _previous = ReadOnlyList<T>.Empty;
        private readonly IListView<T> _current = ReadOnlyList<T>.Empty;
        private readonly IListView<T> _next = ReadOnlyList<T>.Empty;
        private readonly IListView<T> _last = ReadOnlyList<T>.Empty;

        public SortDirection SortDirection { get; }
        public int Count { get; }
        public int CurrentPage { get; }
        public int PageCount { get; }
        public int PageSize { get; }

        public bool HasFirst => !_first.IsEmpty();
        public bool HasPrevious => !_previous.IsEmpty();
        public bool HasCurrent => !_current.IsEmpty();
        public bool HasNext => !_next.IsEmpty();
        public bool HasLast => !_last.IsEmpty();

        public PagedResultSet(IListView<T> first, IListView<T> previous, IListView<T> current, IListView<T> next, IListView<T> last, int pageSize, int currentPage, int count, SortDirection sortDirection)
        {
            PageSize = pageSize;
            CurrentPage = currentPage;
            Count = count;
            SortDirection = sortDirection;
            PageCount = (int)Math.Ceiling((double)count / pageSize);

            if (PageCount > 0)
            {
                _first = first ?? ReadOnlyList<T>.Empty;
                _previous = previous ?? ReadOnlyList<T>.Empty;
                _current = current ?? ReadOnlyList<T>.Empty;
                _next = next ?? ReadOnlyList<T>.Empty;
                _last = last ?? ReadOnlyList<T>.Empty;        
            }
        }
        
        public IListView<T> First
        {
            get
            {
                if (!HasFirst)
                    throw new Exception("First");

                return _first!;
            }
        }
        public IListView<T> Previous
        {
            get
            {
                if (!this.HasPrevious)
                    throw new Exception("Previous");

                return _previous!;
            }
        }

        public IListView<T> Current
        {
            get
            {
                if (!this.HasCurrent)
                    throw new Exception("Current");

                return _current!;
            }
        }

        public IListView<T> Next
        {
            get
            {
                if (!this.HasNext)
                    throw new Exception("Next");

                return _next!;
            }
        }

        public IListView<T> Last
        {
            get
            {
                if (!this.HasLast)
                    throw new Exception("Last");

                return _last!;
            }
        }
    }
}
