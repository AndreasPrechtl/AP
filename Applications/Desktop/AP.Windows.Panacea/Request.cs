using AP.Routing;
using AP.Security;
using AP.UniformIdentifiers;
using System;
using System.Linq.Expressions;

namespace AP.Panacea.Windows
{
    public class Request : AP.Panacea.Request
    {
        /// <summary>
        /// Creates a new Request instance.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="sender"></param>
        /// <param name="referrer"></param>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <param name="clientAgent"></param>
        public Request(Expression<ResultCreator> expression, object sender = null, IUri referrer = null, User user = null, string message = null, ClientAgent clientAgent = null)
            : base(expression, sender, referrer, user, message, clientAgent)
        { }

        /// <summary>
        /// Creates a new Request instance.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="sender"></param>
        /// <param name="referrer"></param>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <param name="clientAgent"></param>
        public Request(IUri uri, object sender = null, IUri referrer = null, User user = null, string message = null, ClientAgent clientAgent = null)
            : base(uri, sender, referrer, user, message, clientAgent)
        { }

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
        protected Request(Guid id, IUri uri, Expression<ResultCreator> expression, object sender, IUri referrer, User user, string message, ClientAgent clientAgent)
            : base(id, uri, expression, sender, referrer, user, message, clientAgent)
        { }
    }
}
