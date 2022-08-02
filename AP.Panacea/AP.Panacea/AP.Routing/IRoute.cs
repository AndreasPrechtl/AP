using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AP.Routing
{
    /// <summary>
    /// Used when the Route is evaluated, it will only be used when all (the contextConstraint, the binder and the parametersConstraint) return true.
    /// </summary>
    /// <remarks>Out of convinience, the values match RoutingResult's Allowed, Denied.</remarks>
    public enum RoutingAction
    {
        Allow = 1,
        Deny = 2
    }

    public interface IRoute<TContext>
        where TContext : RoutingContext
    {
        IBinder<TContext> Binder { get; }
        ContextValidator<TContext> ContextValidator { get; }
        ParametersValidator ParametersValidator { get; }

        Type ParametersType { get; }

        UriSegmentList UriSegments { get; }

        LambdaExpression Expression { get; }

        bool HasContextValidator { get; }
        bool HasBinder { get; }
        bool HasParametersValidator { get; }

        RoutingAction Action { get; }
    }
}
