using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Collections;
using AP.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Collections.ObjectModel;
using System.Reflection;
using AP.ComponentModel;

namespace AP.UI
{
    public sealed class QueryableViewModel<T, TNavigation, TKey> : IViewModel<T, TNavigation>
    {
        private readonly ResultSet<T, TNavigation, TKey> _results;

        private ResultSet<T, TNavigation, TKey> Results
        {
            get { return _results; }
        }

        public QueryableViewModel(IQueryable<T> source, LinkCreator<TKey, TNavigation> linkCreator, Expression<KeySelector<T, TKey>> keySelector, TKey currentKey, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey> keyComparer = null)
        {
            ArgumentNullException.ThrowIfNull(source);

            // this will only return true if the key is a ref type - otherwise - if I compare to IsDefault() it would result in an exception for a key that is int(0)
            if (currentKey == null)
                throw new ArgumentNullException("currentKey");

            ArgumentNullException.ThrowIfNull(keySelector);

            ArgumentNullException.ThrowIfNull(linkCreator);

            _results = CreateResultSet(source, linkCreator, keySelector.Cast<Func<T, TKey>>(), currentKey, sortDirection, keyComparer);
        }

        private static ResultSet<T, TNavigation, TKey> CreateResultSet(IQueryable<T> source, LinkCreator<TKey, TNavigation> linkCreator, Expression<Func<T, TKey>> keySelector, TKey currentKey, SortDirection sortDirection, IComparer<TKey> keyComparer)
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
                return new ResultSet<T, TNavigation, TKey>(default(TKey), default(TKey), default(T), default(TKey), default(TKey), linkCreator, 0, sortDirection);
            
            if (sortDirection == SortDirection.Ascending)
                return new ResultSet<T, TNavigation, TKey>(res.First, res.Previous, res.Current, res.Next, res.Last, linkCreator, res.Count, sortDirection);

            // descending sortdirection? switch the parameters
            return new ResultSet<T, TNavigation, TKey>(res.Last, res.Next, res.Current, res.Previous, res.First, linkCreator, res.Count, sortDirection);
        }

        public SortDirection SortDirection
        {
            get { return this.Results.SortDirection; }
        }


        #region IViewModel<T> Members

        public int Count { get { return this.Results.Count; } }

        public TNavigation First
        {
            get { return this.Results.First; }
        }

        public TNavigation Previous
        {
            get { return this.Results.Previous; }
        }

        public T Current
        {
            get { return this.Results.Current; }
        }

        public TNavigation Next
        {
            get { return this.Results.Next; }
        }

        public TNavigation Last
        {
            get { return this.Results.Last; }
        }

        public bool HasFirst { get { return this.Results.HasFirst; } }
        public bool HasPrevious { get { return this.Results.HasPrevious; } }
        public bool HasCurrent { get { return this.Results.HasCurrent; } }
        public bool HasNext { get { return this.Results.HasNext; } }
        public bool HasLast { get { return this.Results.HasLast; } }

        #endregion
    }
}
