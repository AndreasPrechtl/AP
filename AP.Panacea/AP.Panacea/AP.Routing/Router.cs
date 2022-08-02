using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.ComponentModel;
using AP.UniformIdentifiers;
using AP;
using AP.Reflection;
using System.Reflection;
using AP.Collections;
using System.Linq.Expressions;
using AP.Linq;
using System.ComponentModel;
using AP.Collections.Specialized;

namespace AP.Routing
{
    public class Router<TContext>
        where TContext : RoutingContext
    {
        private readonly RouteTable<TContext> _routes;
        
        /// <summary>
        /// Creates a new router instance.
        /// <param name="routes">The existing routes.</param>
        /// </summary>        
        public Router(IEnumerable<IRoute<TContext>> routes = null)
        {
            _routes = new RouteTable<TContext>(routes);
        }

        /// <summary>
        /// Returns the route table.
        /// </summary>
        public RouteTable<TContext> Routes
        {
            get { return _routes; }
        }

        /// <summary>
        /// Creates the RoutingResult.
        /// </summary>
        /// <param name="context">The routing context.</param>
        /// <returns>A routing result - might be Allowed, Blocked or Invalid.</returns>
        public RoutingResult<TContext> GetResult(TContext context)
        {            
            ResultType resultType;
            object parameters;

            IRoute<TContext> route = this.FindRoute(context, out resultType, out parameters);
            
            if (resultType == ResultType.NoMatch)
                return this.HandleRouteNotFound(context);
            
            IUri resultUri = this.CreateResultUri(parameters, route);

            if (resultType == ResultType.Denied)
                return this.HandleBlockedRoute(context, route, resultUri);

            Expression<ResultCreator> expression = Expression.Lambda<ResultCreator>(Expression.Invoke(route.Expression, Expressions.Constant(parameters, parameters.GetType())));

            return this.HandleAllowedRoute(context, route, resultUri, expression.Compile());            
        }

        /// <summary>
        /// Gets the Uri for a RoutingContext, returns null if no direct and allowed match was found.
        /// </summary>
        /// <param name="context">The routing context.</param>
        /// <returns>The uri.</returns>
        public IUri GetUri(TContext context)
        {
            ResultType resultType;
            object parameters;

            IRoute<TContext> route = this.FindRoute(context, out resultType, out parameters);

            if (resultType != ResultType.NoMatch)
                return this.CreateResultUri(parameters, route);

            return null;
        }

        /// <summary>
        /// Finds the route.
        /// </summary>
        /// <param name="context">The routing context.</param>
        /// <param name="resultType">The resultType enum.</param>
        /// <param name="parameters">The parameters for creating the result and resultUri.</param>
        /// <returns>The matching route or null.</returns>
        private IRoute<TContext> FindRoute(TContext context, out ResultType resultType, out object parameters)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            foreach (IRoute<TContext> route in _routes)
            {              
                resultType = ResultType.NoMatch;
                parameters = null;

                IBinder<TContext> b = route.HasBinder ? route.Binder : this.GetDefaultBinder(route.ParametersType);
                
                if (b.IsMatch(context, route, out parameters))
                {
                    if (route.HasContextValidator && !route.ContextValidator(context))
                        continue;
                    
                    // the binder needs to be tried for a match, if it doesn't exist, get the default binder
                    IBinder<TContext> binder = route.HasBinder ? route.Binder : this.GetDefaultBinder(route.ParametersType);

                    if (!binder.IsMatch(context, route, out parameters))
                        continue;

                    if (route.HasParametersValidator && !route.ParametersValidator(parameters))
                        continue;

                    // simply convert the routeType to the result type
                    resultType = (ResultType)route.Action;

                    return route;
                }
            }

            resultType = ResultType.NoMatch;
            parameters = null;

            return null;
        }

        /// <summary>
        /// Creates the result uri for the given parameters and route.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="route">The route.</param>
        /// <returns>Returns the result uri.</returns>
        protected virtual IUri CreateResultUri(object parameters, IRoute<TContext> route)
        {
            UriSegmentList segments = route.UriSegments;
            
            AP.Collections.List<string> parts = new AP.Collections.List<string>(segments.Count);

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty;;
            
            foreach (UriSegment segment in segments)
            {
                if (segment.Type == UriSegmentType.Fixed)                    
                    parts.Add(((FixedUriSegment)segment).Value);
                else if (parameters != null)
                {
                    string name = ((TemplateUriSegment)segment).Name;
                 
                    PropertyInfo property = parameters.GetType().GetProperty(name, flags);

                    if (property != null)
                    {
                        Delegate getter = property.CreateGetterDelegate();

                        Converter<object, string> converter = this.GetPropertyConverter(property);

                        parts.Add(converter(getter.DynamicInvoke()));
                    }
                }                
            }

            return new HttpUrl(parts);
        }

        /// <summary>
        /// Returns a converter that is used for a parameters property when the uri gets built.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The converter. Currently returns a delegate wrapper for the TypeConverter.ConvertToString method.</returns>
        protected virtual Converter<object, string> GetPropertyConverter(PropertyInfo property)
        {
            return new Converter<object, string>(TypeDescriptor.GetConverter(property.PropertyType).ConvertToString);
        }

        /// <summary>
        /// Handles an allowed route.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="route">The route.</param>
        /// <param name="resultUri">The result uri.</param>
        /// <param name="resultCreator">The result creating activator.</param>
        /// <returns>A valid RoutingResult.</returns>
        protected virtual RoutingResult<TContext> HandleAllowedRoute(TContext context, IRoute<TContext> route, IUri resultUri, ResultCreator resultCreator)
        {
            return RoutingResult<TContext>.Allowed(context, route, resultUri, resultCreator);
        }
        
        /// <summary>
        /// Gets an existing binder or creates a new binder for the specified parameters type.
        /// </summary>
        /// <param name="parametersType">The parameters type.</param>
        /// <returns>A new or existing binder.</returns>
        protected virtual IBinder<TContext> GetDefaultBinder(Type parametersType)
        {
            return (IBinder<TContext>)New.Instance(typeof(DefaultBinder<,>).MakeGenericType(typeof(TContext), parametersType), new object[] { null, null });
        }
        
        /// <summary>
        /// Can be used to further determine what to do with a blocked route - redirect to an error page for example.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="route">The blocked route.</param>
        /// <param name="result">The original result.</param>
        /// <param name="uri">The suspected result uri.</param>
        /// <returns>A fallback RoutingResult.</returns>
        protected virtual RoutingResult<TContext> HandleBlockedRoute(TContext context, IRoute<TContext> route, IUri uri = null)
        {
            return RoutingResult<TContext>.Denied(context, route, uri);
        }

        /// <summary>
        /// Can be used to alter the result into something more fitting - like a result for an error page.
        /// </summary>
        /// <param name="context">The routing context.</param>
        /// <returns>A fallback RoutingResult.</returns>
        protected virtual RoutingResult<TContext> HandleRouteNotFound(TContext context)
        {
            return RoutingResult<TContext>.Invalid(context);
        }
    }
}
