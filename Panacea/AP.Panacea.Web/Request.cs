using AP.Routing;
using AP.Security;
using AP.UniformIdentifiers;
using AP.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AP.Panacea.Web
{
    public class Request : AP.Panacea.Request
    {
        private readonly HttpContext _httpContext;

        /// <summary>
        /// Creates a new Request instance.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="sender"></param>
        /// <param name="referrer"></param>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <param name="clientAgent"></param>
        public Request(Expression<ResultCreator> expression, object sender = null, IUri referrer = null, User user = null, string message = null, ClientAgent clientAgent = null, HttpContext httpContext = null)
            : base(expression, sender, referrer, user, message, clientAgent)
        {
            _httpContext = httpContext ?? HttpContext.Current;
        }

        /// <summary>
        /// Creates a new Request instance.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="sender"></param>
        /// <param name="referrer"></param>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <param name="clientAgent"></param>
        public Request(IUri uri, object sender = null, IUri referrer = null, User user = null, string message = null, ClientAgent clientAgent = null, HttpContext httpContext = null)
            : base(uri, sender, referrer, user, message, clientAgent)
        {
            _httpContext = httpContext ?? HttpContext.Current;
        }

        /// <summary>
        /// cctor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uri"></param>
        /// <param name="expression"></param>        
        /// <param name="referrer"></param>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <param name="clientAgent"></param>
        protected Request(Guid id, IUri uri, Expression<ResultCreator> expression, object sender, IUri referrer, User user, string message, ClientAgent clientAgent, HttpContext httpContext)
            : base(id, uri, expression, sender, referrer, user, message, clientAgent)
        {
            _httpContext = httpContext;
        }
        
        public HttpContext HttpContext
        {
            get { return _httpContext; }
        }
    }
}
