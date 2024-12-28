using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using AP.Linq;
using AP.Reflection;

namespace AP.Routing
{    
    public class ExpressionComparer<TParameters>
    {
        /// <summary>
        /// Compares the route's and the routing context's expressions.
        /// </summary>
        /// <param name="routeExpression">The route expression.</param>
        /// <param name="contextExpression">The context expression.</param>
        /// <returns>Returns true if both expressions are invoking the same targets, using the same parameters.</returns>
        public virtual bool Compare(Expression<ResultCreator<TParameters>> routeExpression, Expression<ResultCreator> contextExpression, out TParameters parameters)
        {
            ExpressionComparerInternal<TParameters> comparer = new ExpressionComparerInternal<TParameters>();
                
            bool b = comparer.Compare(routeExpression, contextExpression);

            if (b)
                parameters = comparer.Parameters;
            else
                parameters = default(TParameters);

            return b;
        }
    }           
}
