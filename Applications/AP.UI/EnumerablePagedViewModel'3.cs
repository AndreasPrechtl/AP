﻿using System;
using System.Collections.Generic;
using AP.Collections;
using AP.Linq;

namespace AP.UI
{
    public sealed class EnumerablePagedViewModel<T, TLink, TKey> : IPagedViewModel<T, TLink>
    {
        private PagedResultSet<T, TLink> Results
        {
            get { return _results; }
        }

        private static PagedResultSet<T, TLink> CreateResultSet(IEnumerable<T> source, LinkCreator<TLink> linkCreator, KeySelector<T, TKey> keySelector, SortDirection sortDirection, int currentPage, int pageSize, IComparer<TKey>? keyComparer)
        {
            source = sortDirection == SortDirection.Unsorted ? source : source.Sort(keySelector, sortDirection, keyComparer);

            using (IEnumerator<T> en = source.GetEnumerator())
            {
                int currentStartIndex = currentPage * pageSize;
                int nextStartIndex = currentStartIndex + pageSize;

                int count = 0;

                AP.Collections.List<T> current = new AP.Collections.List<T>(pageSize);

                for (; en.MoveNext(); count++)
                {
                    if (count >= currentStartIndex && count < nextStartIndex)
                        current.Add(en.Current);
                }

                return new PagedResultSet<T, TLink>(current, linkCreator, currentPage, pageSize, count, sortDirection);
            }
        }

        private readonly PagedResultSet<T, TLink> _results;
        
        public EnumerablePagedViewModel(IEnumerable<T> source, LinkCreator<TLink> linkCreator, KeySelector<T, TKey> keySelector, int currentPage = 0, int pageSize = 1, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey>? keyComparer = null)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(linkCreator);

            _results = CreateResultSet(source, linkCreator, keySelector, sortDirection, currentPage, pageSize, keyComparer);
        }

        #region IPagedViewModel<T,TNavigation> Members

        public int Count { get { return this.Results.Count; } }
        public int PageSize { get { return this.Results.PageSize; } }
        public int CurrentPage { get { return this.Results.CurrentPage; } }
        public int PageCount { get { return this.Results.PageCount; } }

        public TLink First => this.Results.First;
        public TLink Previous => this.Results.Previous;
        public IListView<T> Current => this.Results.Current;
        public TLink Next => this.Results.Next;
        public TLink Last => this.Results.Last;

        public bool HasFirst => this.Results.HasFirst;
        public bool HasPrevious => this.Results.HasPrevious;
        public bool HasCurrent => this.Results.HasCurrent;
        public bool HasNext => this.Results.HasNext;
        public bool HasLast => this.Results.HasLast;

        public SortDirection SortDirection => this.Results.SortDirection;

        #endregion
    }
}
