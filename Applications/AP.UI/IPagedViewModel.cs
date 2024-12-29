using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Collections;
using AP.ComponentModel;
using AP.Linq;

namespace AP.UI
{
    public delegate TLink LinkCreator<out TLink>(int pageIndex = 0, int pageSize = 1);

    public interface IPagedViewModel<T>
    {
        int PageCount { get; }
        int PageSize { get; }
        int CurrentPage { get; }

        IListView<T> First { get; }
        IListView<T> Previous { get; }
        IListView<T> Current { get; }
        IListView<T> Next { get; }
        IListView<T> Last { get; }

        int Count { get; }

        bool HasFirst { get; }
        bool HasPrevious { get; }
        bool HasCurrent { get; }
        bool HasNext { get; }
        bool HasLast { get; }

        SortDirection SortDirection { get; }
    }

    public interface IPagedViewModel<T, out TLink>
    {
        int PageCount { get; }
        int PageSize { get; }
        int CurrentPage { get; }

        TLink First { get; }
        TLink Previous { get; }
        IListView<T> Current { get; }
        TLink Next { get; }
        TLink Last { get; }
        
        int Count { get; }

        bool HasFirst { get; }
        bool HasPrevious { get; }
        bool HasCurrent { get; }
        bool HasNext { get; }
        bool HasLast { get; }

        SortDirection SortDirection { get; }
    }
}
