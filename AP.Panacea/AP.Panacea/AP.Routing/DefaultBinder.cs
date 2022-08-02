using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Routing
{
    public class DefaultBinder<TContext, TParameters> : BinderBase<TContext, TParameters>
          where TContext : RoutingContext
    {
        private readonly ExpressionBinder<TContext, TParameters> _expressionBinder;
        private readonly UriBinder<TContext, TParameters> _uriBinder;
        
        public DefaultBinder(ExpressionBinder<TContext, TParameters> expressionBinder = null, UriBinder<TContext, TParameters> uriBinder = null)
        {
            _expressionBinder = expressionBinder ?? new ExpressionBinder<TContext, TParameters>();
            _uriBinder = uriBinder ?? new UriBinder<TContext, TParameters>();
        }

        public ExpressionBinder<TContext, TParameters> ExpressionBinder
        {
            get { return _expressionBinder; }
        }

        public UriBinder<TContext, TParameters> UriBinder
        {
            get { return _uriBinder; }
        }

        public override bool IsMatch(TContext context, Route<TContext, TParameters> route, out TParameters parameters)
        {
            return _expressionBinder.IsMatch(context, route, out parameters) || _uriBinder.IsMatch(context, route, out parameters);
        }
    }
}
