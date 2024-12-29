using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AP.Linq;

namespace AP.Linq;

public static class QueryableExtensions
{
    private const string OrderBy = "OrderBy";
    private const string OrderByDescending = "OrderByDescending";
    private const string ThenBy = "ThenBy";
    private const string ThenByDescending = "ThenByDescending";

    // alternate code if some conflicts will pop up
    //public static IQueryable<T> Reverse<T>(this IQueryable<T> source, bool reverseThenByExpressions = true)
    //{
    //    OrderExpressionVisitor visitor = new OrderExpressionVisitor { _reverseThenByExpressions = reverseThenByExpressions };

    //    Expression e = visitor.Visit(source.Expression);

    //    // if the expression couldn't be modified - use the enumerable as source
    //    if (!visitor._hasModifiedExpression)
    //        return source.AsEnumerable().Reverse().AsQueryable();

    //    return (IQueryable<T>)source.Provider.CreateQuery<T>(e);
    //}
    public static IQueryable<T> Reverse<T>(this IQueryable<T> source, bool reverseThenByExpressions = true)
    {
        // that check shouldn't be necessary
        if (source is IOrderedQueryable<T>)
            return Reverse((IOrderedQueryable<T>)source);

        return source.AsEnumerable().Reverse().AsQueryable();
    }
    public static IOrderedQueryable<T> Reverse<T>(this IOrderedQueryable<T> source, bool reverseThenByExpressions = true)
    {
        ArgumentNullException.ThrowIfNull(source);
        OrderExpressionVisitor visitor = new() { _reverseThenByExpressions = reverseThenByExpressions };

        System.Linq.Expressions.Expression e = visitor.Visit(source.Expression);

        return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(e);
    }

    /// <summary>
    /// An ExpressionVisitor that's used to reverse OrderBy and ThenBy statements
    /// </summary>
    private sealed class OrderExpressionVisitor : ExpressionVisitor
    {
        internal bool _reverseThenByExpressions = true;
        internal bool _hasModifiedExpression = false;

        protected sealed override System.Linq.Expressions.Expression VisitMethodCall(MethodCallExpression node)
        {
            MethodInfo mi = node.Method;
            Type qt = typeof(Queryable);

            switch (mi.Name)
            {
                case OrderBy:
                    node = Expressions.StaticCall(qt, OrderByDescending, mi.GetGenericArguments(), this.Visit(node.Arguments).ToArray());
                    _hasModifiedExpression = true;
                    break;
                case OrderByDescending:
                    node = Expressions.StaticCall(qt, OrderBy, mi.GetGenericArguments(), this.Visit(node.Arguments).ToArray());
                    _hasModifiedExpression = true;
                    break;
                case ThenBy:
                    if (_reverseThenByExpressions)
                    {
                        node = Expressions.StaticCall(qt, ThenByDescending, mi.GetGenericArguments(), this.Visit(node.Arguments).ToArray());
                        _hasModifiedExpression = true;
                    }
                    break;
                case ThenByDescending:
                    if (_reverseThenByExpressions)
                    {
                        node = Expressions.StaticCall(qt, ThenBy, mi.GetGenericArguments(), this.Visit(node.Arguments).ToArray());
                        _hasModifiedExpression = true;
                    }
                    break;
            }
            return base.VisitMethodCall(node);
        }
    }

    public static T Last<T>(this IOrderedQueryable<T> source, Expression<Func<T, bool>>? predicate = null)
    {
        if (predicate != null)
            return Reverse(source).First(predicate);
        else
            return Reverse(source).First();
    }

    public static T? LastOrDefault<T>(this IOrderedQueryable<T> source, Expression<Func<T, bool>>? predicate = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (predicate != null)
            return Reverse(source).FirstOrDefault(predicate);
        else
            return Reverse(source).FirstOrDefault();
    }
    public static IOrderedQueryable<T> Sort<T, TKey>(this IQueryable<T> source, Expression<KeySelector<T, TKey>> keySelector, SortDirection sortDirection, IComparer<TKey>? keyComparer = null) => Sort(source, keySelector.Cast<Func<T, TKey>>(), sortDirection, keyComparer);

    public static IOrderedQueryable<T> Sort<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> keySelector, SortDirection sortDirection, IComparer<TKey>? keyComparer = null)
    {
        ArgumentOutOfRangeException.ThrowIfEqual((sbyte)sortDirection, (sbyte)SortDirection.Unsorted, nameof(sortDirection));

        if (keyComparer == null)
        {
            if (sortDirection == SortDirection.Ascending)
                return source.OrderBy(keySelector);
            
            return source.OrderByDescending(keySelector);
        }

        if (sortDirection == SortDirection.Ascending)
            return source.OrderBy(keySelector, keyComparer);
            
        return source.OrderByDescending(keySelector, keyComparer);
    }

    public static SortDirection GetSortDirection<T>(this IQueryable<T> source)
    {
        if (source is IOrderedQueryable<T>)
        {
            if (source.Expression is not MethodCallExpression e)
                return source.AsEnumerable().GetSortDirection();

            MethodInfo mi = e.Method;

            if (mi.Name == OrderBy)
                return SortDirection.Ascending;
            
            if (mi.Name == OrderByDescending)
                return SortDirection.Descending;
        }
        return SortDirection.Unsorted;
    }
    public static bool IsEmpty<T>(this IQueryable<T> source) => !source.Any();

    public static bool IsNullOrEmpty<T>(this IQueryable<T> source) => source.IsNull() || source.IsEmpty();
}
