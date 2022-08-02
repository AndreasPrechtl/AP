using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AP.Routing
{
    public class RoutingException : Exception
    {
        public RoutingException()
            : base()
        { }

        public RoutingException(string message)
            : base(message)
        { }
        
        public RoutingException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected RoutingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
