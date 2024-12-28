using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Routing
{
    public delegate object ResultCreator<TParameters>(TParameters parameters);
    public delegate object ResultCreator();
    
    public delegate bool ContextValidator<TContext>(TContext context) where TContext : RoutingContext;
    
    public delegate bool ParametersValidator<in TParameters>(TParameters parameters);
    public delegate bool ParametersValidator(object parameters);

    public delegate IUri UriBuilder<TParameters>(TParameters parameters);
}
