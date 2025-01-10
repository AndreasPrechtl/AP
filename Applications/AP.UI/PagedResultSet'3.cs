using System;
using AP.Collections;
using AP.Collections.ReadOnly;
using AP.Linq;

namespace AP.UI
{
    internal sealed class PagedResultSet<T, TLink> : IPagedViewModel<T, TLink>
    {
        private readonly IListView<T> _current;
        private readonly LinkCreator<TLink> _linkCreator;

        public SortDirection SortDirection { get; }
        public int PageCount { get; }
        public int Count { get; }
        public int PageSize { get; }
        public int CurrentPage { get; }

        public bool HasFirst => this.HasCurrent && CurrentPage > 0;
        public bool HasPrevious => this.HasFirst;
        public bool HasCurrent => !_current.IsEmpty();
        public bool HasNext => this.HasCurrent && PageCount > CurrentPage + 1;
        public bool HasLast => this.HasNext;

        public PagedResultSet(IListView<T> current, LinkCreator<TLink> linkCreator, int currentPage, int pageSize, int count, SortDirection sortDirection)
        { 
            _current = current ?? ReadOnlyList<T>.Empty;
            CurrentPage = currentPage;
            PageSize = pageSize;
            Count = count;
            PageCount = (int)Math.Ceiling((double)count / pageSize);
            _linkCreator = linkCreator;
            SortDirection = sortDirection;
        }

        public TLink First
        {
            get
            {
                if (!HasFirst)
                    throw new Exception("First");

                return _linkCreator(0, PageSize);
            }
        }
        public TLink Previous
        {
            get 
            { 
                if (!this.HasPrevious)
                    throw new Exception("Previous");

                return _linkCreator(CurrentPage - 1, PageSize);
            }
        }

        public IListView<T> Current
        {
            get
            {
                if (_current == null)
                    throw new Exception("Current");

                return _current;
            }
        }

        public TLink Next
        {
            get
            {
                if (!this.HasNext)
                    throw new Exception("Next");

                return _linkCreator(CurrentPage + 1, PageSize);
            }
        }

        public TLink Last
        {
            get
            {
                if (!this.HasLast)
                    throw new Exception("Last");

                return _linkCreator(PageCount - 1, PageSize);
            }
        }
    }
}
