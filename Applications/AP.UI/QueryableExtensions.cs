using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AP.Linq;

namespace AP.UI
{
    public static class QueryableExtensions
    {
        public static IViewModel<T> ToViewModel<T, TKey>(this IQueryable<T> source, Expression<KeySelector<T, TKey>> keySelector, TKey currentKey, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey>? keyComparer = null) 
            => new QueryableViewModel<T, TKey>(source, keySelector, currentKey, sortDirection, keyComparer);

        public static IViewModel<T, TNavigation> ToViewModel<T, TNavigation, TKey>(this IQueryable<T> source, LinkCreator<TKey, TNavigation> linkCreator, Expression<KeySelector<T, TKey>> keySelector, TKey currentKey, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey>? keyComparer = null) 
            => new QueryableViewModel<T, TNavigation, TKey>(source, linkCreator, keySelector, currentKey, sortDirection, keyComparer);

        public static IPagedViewModel<T> ToPagedViewModel<T, TKey>(this IQueryable<T> source, Expression<KeySelector<T, TKey>> keySelector, int currentPage = 0, int pageSize = 1, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey>? keyComparer = null)
            => new QueryablePagedViewModel<T, TKey>(source, keySelector, currentPage, pageSize, sortDirection, keyComparer);

        public static IPagedViewModel<T, TNavigation> ToPagedViewModel<T, TNavigation, TKey>(this IQueryable<T> source, LinkCreator<TNavigation> linkCreator, Expression<KeySelector<T, TKey>> keySelector, int currentPage = 0, int pageSize = 1, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey>? keyComparer = null) 
            => new QueryablePagedViewModel<T, TNavigation, TKey>(source, linkCreator, keySelector, currentPage, pageSize, sortDirection, keyComparer);
    }
}
