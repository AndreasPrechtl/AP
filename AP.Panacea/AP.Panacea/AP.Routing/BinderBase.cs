using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Routing
{
    /// <summary>
    /// The binder is being used to extract parameters, uri and activator from the routing context.
    /// </summary>
    /// <typeparam name="TContext">The routing context.</typeparam>
    /// <typeparam name="TParameters">The parameters.</typeparam>
    public abstract class BinderBase<TContext, TParameters> : IBinder<TContext>
        where TContext : RoutingContext
    {
        protected BinderBase()
        { }

        public abstract bool IsMatch(TContext context, Route<TContext, TParameters> route, out TParameters parameters);

        #region IBinder<TContext> Members

        bool IBinder<TContext>.IsMatch(TContext context, IRoute<TContext> route, out object parameters)
        {
            if (route is Route<TContext, TParameters>)
            {
                TParameters p;

                if (this.IsMatch(context, (Route<TContext, TParameters>)route, out p))
                {
                    parameters = p;
                    return true;
                }
            }

            parameters = null;
            
            return false;
        }

        #endregion
    }
}
