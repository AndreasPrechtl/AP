using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Routing
{
    [Serializable]
    public enum ResultType
    {
        NoMatch = 0,
        Allowed = 1,
        Denied = 2        
    }

    public class RoutingResult<TContext>
        where TContext : RoutingContext
    {      
        private readonly Deferrable<object> _value;
        private readonly IUri _resultUri;
        private readonly ResultType _type;
        private readonly TContext _context;
        private readonly IRoute<TContext> _route;

        /// <summary>
        /// cctor - Creates a RoutingResult.
        /// </summary>
        /// <param name="type">The ResultType.</param>
        /// <param name="context">The RoutingContext.</param>
        /// <param name="route">The route.</param>
        /// <param name="resultUri">The result uri.</param>
        /// <param name="value">The wrapped activator.</param>        
        protected RoutingResult(ResultType type, TContext context, IRoute<TContext> route, IUri resultUri, Deferrable<object> value)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _type = type;
            _context = context;
            _resultUri = resultUri;
            _value = value;
            _route = route;
        }

        public static RoutingResult<TContext> Allowed(TContext context, IRoute<TContext> route, IUri resultUri, ResultCreator resultCreator)
        {
            if (resultUri == null)
                throw new ArgumentNullException("resultUri");

            if (resultCreator == null)
                throw new ArgumentNullException("resultCreator");

            if (route == null)
                throw new ArgumentNullException("route");

            return new RoutingResult<TContext>(ResultType.Allowed, context, route, resultUri, new Deferrable<object>(new Activator<object>(resultCreator)));
        }

        public static RoutingResult<TContext> Denied(TContext context, IRoute<TContext> route, IUri uri = null)
        {
            if (route == null)
                throw new ArgumentNullException("route");
            
            return new RoutingResult<TContext>(ResultType.Denied, context, route, uri, null);
        }

        public static RoutingResult<TContext> Invalid(TContext context)
        {
            return new RoutingResult<TContext>(ResultType.NoMatch, context, null, null, null);
        }

        public object Value
        {
            get
            {
                if (_value != null)
                    return _value.Value;

                return null;
            }
        }

        public ResultType Type
        {
            get { return _type; }
        }

        public TContext Context
        {
            get { return _context; }
        }

        public IUri Uri
        {
            get { return _resultUri; }
        }
    }
}
