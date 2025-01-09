using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AP.Panacea.Web
{
    public class ForwardToHttpHandler
    {
        private readonly Type _handlerType;

        public Type HandlerType { get { return _handlerType; } }

        public ForwardToHttpHandler(Type httpHandlerType)
        {
            ArgumentNullException.ThrowIfNull(httpHandlerType);

            if (!typeof(IHttpHandler).IsAssignableFrom(httpHandlerType))
                throw new ArgumentException("httpHandlerType does not implement IHttpHandler");

            _handlerType = httpHandlerType;
        }
    }
}
