using System;
using System.Linq.Expressions;
using AP.ComponentModel;
using AP.Routing;
using AP.Security;
using AP.UniformIdentifiers;

namespace AP.Panacea
{
    public class Response
    {
        private readonly Guid _id;
        private readonly IUri _uri;
        private readonly Request _request;
        private readonly string _message;
        private readonly object _result;
        private readonly object _parameters;
        private readonly ResponseType _type;
        
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
            : this(New.Guid(), type, result, uri, request, parameters, message)
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
        {
            if (request == null)            
                throw new ArgumentNullException("request");

            if (uri == null && type != ResponseType.RouteNotFound)
                throw new ArgumentNullException("uri");

            _id = id;
            _type = type;
            _result = result;
            _request = request;
            _uri = uri;
            _parameters = parameters;
            _message = message;
        }

        /// <summary>
        /// Gets the response type.
        /// </summary>
        public ResponseType Type
        {
            get { return _type; }
        }
        
        /// <summary>
        /// Gets the sender.
        /// </summary>
        public object Sender
        {
            get { return _request.Sender; }
        }

        /// <summary>
        /// Gets the request that originated the response.
        /// </summary>
        public Request Request
        {
            get { return _request; }
        }

        /// <summary>
        /// Gets the uri.
        /// </summary>
        public IUri Uri
        {
            get { return _uri; }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        public User User
        {
            get { return _request.User; }
        }

        /// <summary>
        /// Gets the client agent.
        /// </summary>
        public ClientAgent ClientAgent
        {
            get { return _request.ClientAgent; }
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        public object Result
        {
            get { return _result; }
        }
        
        /// <summary>
        /// Gets the response id.
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }
    }
}