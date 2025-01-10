using System;
using System.Collections.Generic;
using System.Linq;
using AP.Linq;
using System.Linq.Expressions;

namespace AP.UI
{
    public sealed class QueryableViewModel<T, TNavigation, TKey> : IViewModel<T, TNavigation>
        where TKey : notnull
    {
        private readonly ResultSet<T, TNavigation, TKey> _results;

        public QueryableViewModel(IQueryable<T> source, LinkCreator<TKey, TNavigation> linkCreator, Expression<KeySelector<T, TKey>> keySelector, TKey currentKey, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey>? keyComparer = null)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(currentKey);
            ArgumentNullException.ThrowIfNull(keySelector);
            ArgumentNullException.ThrowIfNull(linkCreator);

            _results = CreateResultSet(source, linkCreator, keySelector.Cast<Func<T, TKey>>(), currentKey, sortDirection, keyComparer);
        }

        private static ResultSet<T, TNavigation, TKey> CreateResultSet(IQueryable<T> source, LinkCreator<TKey, TNavigation> linkCreator, Expression<Func<T, TKey>> keySelector, TKey currentKey, SortDirection sortDirection, IComparer<TKey>? keyComparer)
        {
            // create the required expressions
            var lessExpression = ExpressionHelper.CreateComparisonExpression(currentKey, keySelector, ComparisonOperator.Less, keyComparer);
            var equalsExpression = ExpressionHelper.CreateComparisonExpression(currentKey, keySelector, ComparisonOperator.Equal, keyComparer);
            var greaterExpression = ExpressionHelper.CreateComparisonExpression(currentKey, keySelector, ComparisonOperator.Greater, keyComparer);

            // sort it, and build the queries
            var asc = source.Sort(keySelector, SortDirection.Ascending, keyComparer);
            var desc = source.Sort(keySelector, SortDirection.Descending, keyComparer);

            // try and use these queries inline... and u'd be surprised. this stuff will be optimized in a funny way -> NOT WORKING.
            var firstQuery = asc.Where(lessExpression).Select(keySelector);
            var previousQuery = desc.Where(lessExpression).Select(keySelector);
            var currentQuery = source.Where(equalsExpression);
            var nextQuery = asc.Where(greaterExpression).Select(keySelector);
            var lastQuery = desc.Where(greaterExpression).Select(keySelector);

            var q = from c in source
                    select new
                    {
                        Count = source.Count(),
                        First = firstQuery.FirstOrDefault(),
                        Previous = previousQuery.FirstOrDefault(),
                        Current = currentQuery.FirstOrDefault(),
                        Next = nextQuery.FirstOrDefault(),
                        Last = lastQuery.FirstOrDefault()
                    };

            var res = q.FirstOrDefault();

            if (res == null)
                return new ResultSet<T, TNavigation, TKey>(default!, default!, default!, default!, default!, linkCreator, 0, sortDirection);
            
            if (sortDirection == SortDirection.Ascending)
                return new ResultSet<T, TNavigation, TKey>(res.First, res.Previous, res.Current, res.Next, res.Last, linkCreator, res.Count, sortDirection);

            // descending sortDirection? switch the parameters
            return new ResultSet<T, TNavigation, TKey>(res.Last, res.Next, res.Current, res.Previous, res.First, linkCreator, res.Count, sortDirection);
        }

        public SortDirection SortDirection => _results.SortDirection;


        #region IViewModel<T> Members

        public int Count => _results.Count;

        public TNavigation First => _results.First;
        public TNavigation Previous => _results.Previous;
        public T Current => _results.Current;
        public TNavigation Next => _results.Next;
        public TNavigation Last => _results.Last;

        public bool HasFirst => _results.HasFirst;
        public bool HasPrevious => _results.HasPrevious;
        public bool HasCurrent => _results.HasCurrent;
        public bool HasNext => _results.HasNext;
        public bool HasLast => _results.HasLast;

        #endregion
    }
}
