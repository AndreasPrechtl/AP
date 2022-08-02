using AP.Collections;
using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AP.Linq;

namespace AP.Routing
{       
    /// <summary>
    /// Helper class for creating Routes.
    /// </summary>
    public abstract class Route : StaticType
    {
        public static Route<TContext, TParameters> Create<TContext, TParameters>
        (
            Expression<ResultCreator<TParameters>> expression, 
            UriSegmentList uriSegments,
            ContextValidator<TContext> contextConstraint = null, 
            BinderBase<TContext, TParameters> binder = null, 
            ParametersValidator<TParameters> parametersConstraint = null,
            RoutingAction action = RoutingAction.Allow
        ) 
            where TContext : RoutingContext
        {
            return new Route<TContext, TParameters>(expression, uriSegments, contextConstraint, binder, parametersConstraint, action);
        }

        public static Route<TContext, object> Create<TContext>
        (
            Expression<ResultCreator<object>> expression,
            UriSegmentList uriSegments,
            ContextValidator<TContext> contextConstraint = null,
            BinderBase<TContext, object> binder = null,
            RoutingAction action = RoutingAction.Allow
        )
            where TContext : RoutingContext
        {
            return Create<TContext, object>(expression, uriSegments, contextConstraint, binder, null, action);
        }
        
        public static Route<TContext, TParameters> Allowed<TContext, TParameters>
        (
            Expression<ResultCreator<TParameters>> expression,
            UriSegmentList uriSegments,
            ContextValidator<TContext> contextConstraint = null,
            BinderBase<TContext, TParameters> binder = null,
            ParametersValidator<TParameters> parametersConstraint = null            
        )
            where TContext : RoutingContext
        {
            return new Route<TContext, TParameters>(expression, uriSegments, contextConstraint, binder, parametersConstraint, RoutingAction.Allow);
        }
        
        public static Route<TContext, object> Allowed<TContext>
        (
            Expression<ResultCreator<object>> expression,
            UriSegmentList uriSegments,
            ContextValidator<TContext> contextConstraint = null,
            BinderBase<TContext, object> binder = null,
            ParametersValidator<object> parametersConstraint = null
        )
            where TContext : RoutingContext
        {
            return Allowed<TContext, object>(expression, uriSegments, contextConstraint, binder, parametersConstraint);
        }

        public static Route<TContext, TParameters> Denied<TContext, TParameters>
        (
            Expression<ResultCreator<TParameters>> expression,
            UriSegmentList uriSegments,
            ContextValidator<TContext> contextConstraint = null,
            BinderBase<TContext, TParameters> binder = null,
            ParametersValidator<TParameters> parametersConstraint = null
        )
            where TContext : RoutingContext
        {
            return new Route<TContext, TParameters>(expression, uriSegments, contextConstraint, binder, parametersConstraint, RoutingAction.Deny);
        }

        public static Route<TContext, object> Denied<TContext>
        (
            Expression<ResultCreator<object>> expression,
            UriSegmentList uriSegments,
            ContextValidator<TContext> contextConstraint = null,
            BinderBase<TContext, object> binder = null,
            ParametersValidator<object> parametersConstraint = null
        )
            where TContext : RoutingContext
        {
            return Denied<TContext, object>(expression, uriSegments, contextConstraint, binder, parametersConstraint);
        }
        
        public static Route<TContext, TParameters> Denied<TContext, TParameters>
        (
            Expression<ResultCreator<TParameters>> expression,            
            ContextValidator<TContext> contextConstraint = null,
            BinderBase<TContext, TParameters> binder = null,
            ParametersValidator<TParameters> parametersConstraint = null            
        )
            where TContext : RoutingContext
        {
            return new Route<TContext, TParameters>(expression, UriSegmentList.Empty, contextConstraint, binder, parametersConstraint, RoutingAction.Deny);
        }

        public static Route<TContext, object> Denied<TContext>
        (
            Expression<ResultCreator<object>> expression,
            ContextValidator<TContext> contextConstraint = null,
            BinderBase<TContext, object> binder = null,
            ParametersValidator<object> parametersConstraint = null
        )
            where TContext : RoutingContext
        {
            return Denied<TContext, object>(expression, UriSegmentList.Empty, contextConstraint, binder, parametersConstraint);
        }

        public static Route<TContext, TParameters> Denied<TContext, TParameters>
        (
            UriSegmentList uriSegments,
            ContextValidator<TContext> contextConstraint = null,
            BinderBase<TContext, TParameters> binder = null,
            ParametersValidator<TParameters> parametersConstraint = null            
        )
            where TContext : RoutingContext
        {
            return new Route<TContext, TParameters>(null, uriSegments, contextConstraint, binder, parametersConstraint, RoutingAction.Deny);
        }

        public static Route<TContext, object> Denied<TContext>
        (
            UriSegmentList uriSegments,
            ContextValidator<TContext> contextConstraint = null,
            BinderBase<TContext, object> binder = null,
            ParametersValidator<object> parametersConstraint = null
        )
            where TContext : RoutingContext
        {
            return Denied<TContext, object>(null, uriSegments, contextConstraint, binder, parametersConstraint);
        }
    }    
}
