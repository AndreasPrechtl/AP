using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP.Collections;
using AP.Linq;

namespace AP.UI
{
    internal sealed class PagedResultSet<T, TLink> : IPagedViewModel<T, TLink>
    {
        private readonly IListView<T> _current;

        private readonly SortDirection _sortDirection;
        public SortDirection SortDirection { get { return _sortDirection; } }

        public IListView<T> Current 
        {
            get
            {
                if (_current == null)
                    throw new Exception("Current");
    
                return _current;
            }
        }

        private readonly LinkCreator<TLink> _linkCreator;

        public bool HasFirst { get { return this.HasCurrent && _currentPage > 0; } }
        public bool HasPrevious { get { return this.HasFirst; } }
        
        public bool HasCurrent { get { return _current != null; } }

        public bool HasNext { get { return this.HasCurrent && _pageCount > _currentPage + 1; } }
        public bool HasLast { get { return this.HasNext; } }

        private readonly int _pageCount;

        public int PageCount
        {
            get { return _pageCount; }
        }

        public int Count
        {
            get { return _count; }
        }

        public int PageSize
        {
            get { return _pageSize; }
        }

        public int CurrentPage
        {
            get { return _currentPage; }
        }

        private readonly int _count;
        private readonly int _currentPage;
        private readonly int _pageSize;

        public PagedResultSet(IListView<T> current, LinkCreator<TLink> linkCreator, int currentPage, int pageSize, int count, SortDirection sortDirection)
        { 
            _current = current.IsDefaultOrEmpty() ? null : current;
            _currentPage = currentPage;
            _pageSize = pageSize;
            _count = count;
            _pageCount = (int)Math.Ceiling((double)count / pageSize);
            _linkCreator = linkCreator;
            _sortDirection = sortDirection;
        }

        public TLink First
        {
            get
            {
                if (!HasFirst)
                    throw new Exception("First");

                return _linkCreator(0, _pageSize);
            }
        }
        public TLink Previous
        {
            get 
            { 
                if (!this.HasPrevious)
                    throw new Exception("Previous");

                return _linkCreator(_currentPage - 1, _pageSize);
            }
        }

        public TLink Next
        {
            get
            {
                if (!this.HasNext)
                    throw new Exception("Next");

                return _linkCreator(_currentPage + 1, _pageSize);
            }
        }

        public TLink Last
        {
            get
            {
                if (!this.HasLast)
                    throw new Exception("Last");

                return _linkCreator(_pageCount - 1, _pageSize);
            }
        }
    }
}
