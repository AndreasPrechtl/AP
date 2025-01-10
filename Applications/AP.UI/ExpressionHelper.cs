using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AP.Linq;

namespace AP.UI
{
    internal static class ExpressionHelper
    {
        private const string _compare = "Compare";
        private const string _compareTo = "CompareTo";

        public static Expression<Func<T, bool>> CreateComparisonExpression<T, TKey>(TKey currentKey, Expression<Func<T, TKey>> keySelector, ComparisonOperator op, IComparer<TKey>? keyComparer)
        {
            ConstantExpression zeroExpression = Expression.Constant(0);
            ConstantExpression currentKeyExpression = Expression.Constant(currentKey);

            Expression compare = null!;

            if (keyComparer != null)
            {
                compare = Expressions.InstanceCall
                (
                    Expression.Constant(keyComparer),
                    _compare,
                    [],
                    keySelector.Body, currentKeyExpression
                );
            }
            else
            {
                // hopefully IComparable is implemented directly - or else.
                compare = Expression.Call    // calls the CompareTo method
                (
                    keySelector.Body,
                    _compareTo,
                    null, // non-generic method -> null                               
                    currentKeyExpression  // the key of the current item that's already been retrieved                         
                );
            }

            BinaryExpression binary = op switch
            {
                ComparisonOperator.Greater => Expression.GreaterThan(compare, zeroExpression),
                ComparisonOperator.Less => Expression.LessThan(compare, zeroExpression),
                _ => Expression.Equal(compare, zeroExpression),
            };
            
            return Expression.Lambda<Func<T, bool>>(binary, keySelector.Parameters);
        }
    }
}
