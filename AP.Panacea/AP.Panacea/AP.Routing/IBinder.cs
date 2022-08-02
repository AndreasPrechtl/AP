using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Routing
{
    public interface IBinder<TContext>
        where TContext : RoutingContext
    {        
        bool IsMatch(TContext context, IRoute<TContext> route, out object parameters);
    }
}
