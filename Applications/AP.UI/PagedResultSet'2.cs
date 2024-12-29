using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP.Collections;
using AP.Linq;

namespace AP.UI
{
    internal sealed class PagedResultSet<T> : IPagedViewModel<T>
    {
        public IListView<T> Current
        {
            get
            {
                if (!this.HasCurrent)
                    throw new Exception("Current");

                return _current;
            }
        }
        
        private readonly SortDirection _sortDirection;
        public SortDirection SortDirection { get { return _sortDirection; } }
        
        public bool HasFirst { get { return _first != null; } }
        public bool HasPrevious { get { return _previous != null; } }
        public bool HasCurrent { get { return _current != null; } }
        public bool HasNext { get { return _next != null; } }
        public bool HasLast { get { return _last != null; } }

        private readonly int _count;
        private readonly int _currentPage;
        private readonly int _pageSize;
        private readonly int _pageCount;

        private readonly IListView<T> _first;
        private readonly IListView<T> _previous;

        private readonly IListView<T> _current;

        private readonly IListView<T> _next;
        private readonly IListView<T> _last;

        public int Count { get { return _count; } }
        public int CurrentPage { get { return _currentPage; } }
        public int PageCount { get { return _pageCount; } }
        public int PageSize { get { return _pageSize; } }

        public PagedResultSet(IListView<T> first, IListView<T> previous, IListView<T> current, IListView<T> next, IListView<T> last, int pageSize, int currentPage, int count, SortDirection sortDirection)
        {
            _pageSize = pageSize;
            _currentPage = currentPage;
            _count = count;
            _sortDirection = sortDirection;
            _pageCount = (int)Math.Ceiling((double)count / pageSize);

            if (_pageCount > 0)
            {
                _first = first.IsDefaultOrEmpty() ? null : first;
                _previous = previous.IsDefaultOrEmpty() ? null : previous;
                _current = current.IsDefaultOrEmpty() ? null : current;
                _next = next.IsDefaultOrEmpty() ? null : next;
                _last = last.IsDefaultOrEmpty() ? null : last;
            }
        }
        
        public IListView<T> First
        {
            get
            {
                if (!HasFirst)
                    throw new Exception("First");

                return _first;
            }
        }
        public IListView<T> Previous
        {
            get
            {
                if (!this.HasPrevious)
                    throw new Exception("Previous");

                return _previous;
            }
        }

        public IListView<T> Next
        {
            get
            {
                if (!this.HasNext)
                    throw new Exception("Next");

                return _next;
            }
        }

        public IListView<T> Last
        {
            get
            {
                if (!this.HasLast)
                    throw new Exception("Last");

                return _last;
            }
        }
    }
}
