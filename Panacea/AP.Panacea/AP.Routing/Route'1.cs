using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AP.Linq;

namespace AP.Routing
{
    public class Route<TContext> : IRoute<TContext>
        where TContext : RoutingContext
    {
        private readonly Expression<ResultCreator<TParameters>> _expression;
        private readonly UriSegmentList _uriSegments;
        private readonly ContextValidator<TContext> _contextConstraint;
        private readonly IBinder<TContext> _binder;
        private readonly RoutingAction _action;

        // cached
        private static readonly Type _parametersType = typeof(TParameters);

        /// <summary>
        /// Creates a new Route.
        /// </summary>
        /// <param name="expression">The result creator expression.</param>
        /// <param name="uriSegments">The search pattern template.</param>
        /// <param name="contextConstraint">The context validator.</param>
        /// <param name="binder">The Binder.</param>
        /// <param name="uriBuilder">The UriBuilder.</param>
        /// <param name="parametersConstraint">The parameters validator.</param>
        /// <param name="action">The action.</param>
        public Route
        (
            Expression<ResultCreator> expression,
            UriSegmentList uriSegments,
            ContextValidator<TContext> contextConstraint = null,
            BinderBase<TContext> binder = null,
            ParametersValidator<TParameters> parametersConstraint = null,
            RoutingAction action = RoutingAction.Allow
        )
        {
            if (action == RoutingAction.Allow)
            {
                if (expression == null)
                    throw new ArgumentNullException("expression");

                if (uriSegments == null)
                    throw new ArgumentNullException("uriSegments");

                if (uriSegments.Count == 0)
                    throw new ArgumentOutOfRangeException("uriSegments");
            }
            else if (expression == null && uriSegments.IsDefaultOrEmpty())
                throw new ArgumentException("expression and uriSegments cannot be null or empty at the same time.");

            _expression = expression;
            _uriSegments = uriSegments;
            _contextConstraint = contextConstraint;

            _parametersConstraint = parametersConstraint;

            _binder = binder;
            _action = action;
        }

        public Expression<ResultCreator<TParameters>> Expression
        {
            get { return _expression; }
        }

        public UriSegmentList UriSegments
        {
            get { return _uriSegments; }
        }

        public ContextValidator<TContext> ContextValidator
        {
            get { return _contextConstraint; }
        }

        public ParametersValidator<TParameters> ParametersValidator
        {
            get { return _parametersConstraint; }
        }

        public BinderBase<TContext, TParameters> Binder
        {
            get { return _binder; }
        }

        public RoutingAction Action { get { return _action; } }

        public bool HasBinder { get { return _binder != null; } }
        public bool HasContextValidator { get { return _contextConstraint != null; } }
        public bool HasParametersValidator { get { return _parametersConstraint != null; } }

        #region IRoute<TContext> Members

        IBinder<TContext> IRoute<TContext>.Binder
        {
            get { return _binder; }
        }

        ParametersValidator IRoute<TContext>.ParametersValidator
        {
            get { return _parametersConstraint != null ? new ParametersValidator(p => _parametersConstraint((TParameters)p)) : null; }
        }

        LambdaExpression IRoute<TContext>.Expression
        {
            get { return _expression; }
        }

        Type IRoute<TContext>.ParametersType
        {
            get { return _parametersType; }
        }

        #endregion
    }
}
