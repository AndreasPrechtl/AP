using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Panacea.Windows
{
    public class Response : AP.Panacea.Response
    {
        /// <summary>
        /// Creates a new Response instance.
        /// </summary>
        /// <param name="type">The ResponseType.</param>
        /// <param name="result">The result.</param>
        /// <param name="uri">The response uri.</param>
        /// <param name="request">The request originating this response.</param>        
        /// <param name="parameters">The response parameters, can be null.</param>
        /// <param name="message">The response message, can be null.</param>
        public Response(ResponseType type, object result, IUri uri, Request request, object parameters = null, string message = null)
            : base(type, result, uri, request, parameters, message)
        { }

        /// <summary>
        /// cctor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="result"></param>
        /// <param name="uri"></param>
        /// <param name="request"></param>
        /// <param name="parameters"></param>
        /// <param name="message"></param>
        protected Response(Guid id, ResponseType type, object result, IUri uri, Request request, object parameters, string message)
            : base(id, type, result, uri, request, parameters, message)
        { }

        public new Request Request
        {
            get { return (Request)base.Request; }
        }
    }
}
