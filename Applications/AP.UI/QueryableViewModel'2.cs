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
    public sealed class QueryableViewModel<T, TKey> : IViewModel<T>
    {
        private readonly ResultSet<T> _results;

        private ResultSet<T> Results
        {
            get { return _results; }
        }

        public QueryableViewModel(IQueryable<T> source, Expression<KeySelector<T, TKey>> keySelector, TKey currentKey, SortDirection sortDirection = SortDirection.Ascending, IComparer<TKey> keyComparer = null)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            
            if (currentKey == null)
                throw new ArgumentNullException("currentKey");

            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            
            if (sortDirection == SortDirection.Unsorted)
                throw new ArgumentOutOfRangeException("sortDirection");
         
            _results = CreateResultSet(source, keySelector.Cast<Func<T, TKey>>(), currentKey, sortDirection, keyComparer);
        }

        private static ResultSet<T> CreateResultSet(IQueryable<T> source, Expression<Func<T,TKey>> keySelector, TKey currentKey, SortDirection sortDirection, IComparer<TKey> keyComparer)
        {
            // create the required expressions
            var lessExpression = ExpressionHelper.CreateComparisonExpression(currentKey, keySelector, ComparisonOperator.Less, keyComparer);
            var equalsExpression = ExpressionHelper.CreateComparisonExpression(currentKey, keySelector, ComparisonOperator.Equal, keyComparer);
            var greaterExpression = ExpressionHelper.CreateComparisonExpression(currentKey, keySelector, ComparisonOperator.Greater, keyComparer);
            
            // pre-build some sorted queries
            var asc = source.Sort(keySelector, SortDirection.Ascending, keyComparer);
            var desc = source.Sort(keySelector, SortDirection.Descending, keyComparer);

            // try and use these queries inline... and u'd be surprised. this stuff will be optimized in a funny way -> NOT WORKING.
            var firstQuery = asc.Where(lessExpression);
            var previousQuery = desc.Where(lessExpression);
            var currentQuery = source.Where(equalsExpression);
            var nextQuery = asc.Where(greaterExpression);
            var lastQuery = desc.Where(greaterExpression);
            
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
                return new ResultSet<T>(default(T), default(T), default(T), default(T), default(T), 0, sortDirection);

            if (sortDirection == SortDirection.Ascending)
                return new ResultSet<T>(res.First, res.Previous, res.Current, res.Next, res.Last, res.Count, sortDirection);

            // descending sortdirection? switch the parameters
            return new ResultSet<T>(res.Last, res.Next, res.Current, res.Previous, res.First, res.Count, sortDirection);
        }

        public SortDirection SortDirection
        {
            get { return this.Results.SortDirection; }
        }

        
        #region IViewModel<T> Members

        public int Count { get { return this.Results.Count; } }

        public T First
        {
            get { return this.Results.First; }
        }

        public T Previous
        {
            get { return this.Results.Previous; }
        }

        public T Current
        {
            get { return this.Results.Current; }
        }

        public T Next
        {
            get { return this.Results.Next; }
        }

        public T Last
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
