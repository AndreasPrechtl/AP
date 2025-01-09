using System;
using System.Linq.Expressions;
using AP.UniformIdentifiers;

namespace AP.Routing
{
    public class RoutingContext
    {
        private readonly IUri _uri;
        private readonly Expression<ResultCreator> _expression;
        private readonly object _sender;

        /// <summary>
        /// Creates a new RoutingContext instance.
        /// </summary>
        /// <param name="uri">The uri to the requested item.</param>
        /// <param name="sender">The sender, can be null.</param>
        public RoutingContext(IUri uri, object sender = null)
            : this(uri, null, sender)
        {
            ArgumentNullException.ThrowIfNull(uri);
        }

        /// <summary>
        /// Creates a new RoutingContext instance.
        /// </summary>
        /// <param name="expression">The expression for creating the requested item.</param>
        /// <param name="sender">The sender.</param>
        public RoutingContext(Expression<ResultCreator> expression, object sender = null)
            : this(null, expression, sender)
        {
            ArgumentNullException.ThrowIfNull(expression);
        }

        /// <summary>
        /// cctor.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <param name="expression">The expression for generating the requested item.</param>
        /// <param name="sender">The sender.</param>
        /// <remarks>In order to allow custom routing, uri and expression may be null.</remarks>
        protected RoutingContext(IUri uri, Expression<ResultCreator> expression, object sender)
        {
            _uri = uri;
            _expression = expression;
            _sender = sender;
        }

        /// <summary>
        /// Gets the Uri (might be null).
        /// </summary>
        public IUri Uri
        {
            get { return _uri; }
        }

        /// <summary>
        /// Gets the Expression (might be null).
        /// </summary>
        public Expression<ResultCreator> Expression
        {
            get { return _expression; }
        }

        /// <summary>
        /// Gets the sender (might be null).
        /// </summary>
        public object Sender
        {
            get { return _sender; }
        }

        /// <summary>
        /// Indicates if an uri is used.
        /// </summary>
        public bool HasUri
        {
            get { return _uri != null; }
        }

        /// <summary>
        /// Indicates if an expression is used.
        /// </summary>
        public bool HasExpression
        {
            get { return _expression != null; }
        }
    }
}