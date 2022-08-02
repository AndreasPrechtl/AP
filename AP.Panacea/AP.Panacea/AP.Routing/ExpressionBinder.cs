using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AP.Routing
{
    public partial class ExpressionBinder<TContext, TParameters> : BinderBase<TContext, TParameters>
        where TContext : RoutingContext
    {
        private readonly ExpressionComparer<TParameters> _comparer;

        public ExpressionBinder(ExpressionComparer<TParameters> comparer = null)
        {
            _comparer = comparer ?? new ExpressionComparer<TParameters>();
        }

        public ExpressionComparer<TParameters> Comparer 
        { 
            get { return _comparer; } 
        }

        public override bool IsMatch(TContext context, Route<TContext, TParameters> route, out TParameters parameters)
        {
            if (context.HasExpression)
                return new ExpressionComparer<TParameters>().Compare(route.Expression, context.Expression, out parameters);
            
            parameters = default(TParameters);            
            return false;            
        }
    }
}
