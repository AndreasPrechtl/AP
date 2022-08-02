﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using AP.Linq;
using AP.ComponentModel;

namespace AP.UI
{
    public static class EnumerableExtensions
    {
        public static IViewModel<T> ToViewModel<T, TKey>(this IEnumerable<T> source, KeySelector<T, TKey> keySelector, TKey currentKey, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey> keyComparer = null)
        {
            return new EnumerableViewModel<T, TKey>(source, keySelector, currentKey, sortDirection, keyComparer);
        }

        public static IViewModel<T, TNavigation> ToViewModel<T, TNavigation, TKey>(this IEnumerable<T> source, LinkCreator<TKey, TNavigation> linkCreator, KeySelector<T, TKey> keySelector, TKey currentKey, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey> keyComparer = null)
        {
            return new EnumerableViewModel<T, TNavigation, TKey>(source, linkCreator, keySelector, currentKey, sortDirection, keyComparer);
        }

        public static IPagedViewModel<T> ToPagedViewModel<T, TKey>(this IEnumerable<T> source, KeySelector<T, TKey> keySelector, int currentPage = 0, int pageSize = 1, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey> keyComparer = null)
        {
            return new EnumerablePagedViewModel<T, TKey>(source, keySelector, currentPage, pageSize, sortDirection, keyComparer);
        }

        public static IPagedViewModel<T, TNavigation> ToPagedViewModel<T, TNavigation, TKey>(this IEnumerable<T> source, LinkCreator<TNavigation> linkCreator, KeySelector<T, TKey> keySelector, int currentPage = 0, int pageSize = 1, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey> keyComparer = null)
        {
            return new EnumerablePagedViewModel<T, TNavigation, TKey>(source, linkCreator, keySelector, currentPage, pageSize, sortDirection, keyComparer);
        }
    }
}
