using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using AP.Collections;
using AP.ComponentModel;
using AP.ComponentModel.Conversion;
using AP.Linq;
using AP.UniformIdentifiers;

namespace AP.Routing
{   
    // note: lacks public syncroot - might add it.

    /// <summary>
    /// The route table.
    /// </summary>
    /// <typeparam name="TContext">The routing context.</typeparam>
    public class RouteTable<TContext> : AP.Collections.List<IRoute<TContext>>
        where TContext : RoutingContext
    {
        /// <summary>
        /// Creates a new RouteTable.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public RouteTable(int capacity)
            : base(capacity)
        { }

        /// <summary>
        /// Creates a new RouteTable.
        /// </summary>
        /// <param name="routes">optional routes.</param>
        public RouteTable(IEnumerable<IRoute<TContext>> routes = null)
            : base(routes)
        { }
        

        /// <summary>
        /// Clones the route table.
        /// </summary>
        /// <returns></returns>
        public new RouteTable<TContext> Clone()
        {
            return (RouteTable<TContext>)this.OnClone();
        }

        /// <summary>
        /// Clones the route table.
        /// </summary>
        /// <returns>A new route table.</returns>
        protected override Collections.List<IRoute<TContext>> OnClone()
        {
            return new RouteTable<TContext>(this);
        }
    }
}
