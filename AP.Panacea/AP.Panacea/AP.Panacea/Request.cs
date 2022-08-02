using System;
using System.Linq.Expressions;
using AP.ComponentModel;
using AP.Routing;
using AP.Security;
using AP.UniformIdentifiers;

namespace AP.Panacea
{
    public class Request : RoutingContext
    {        
        private Guid _id;
        private User _user;
        private ClientAgent _clientAgent;
        private IUri _referrer;

        private readonly string _message;        
                
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
            : this(New.Guid(), null, expression, sender, referrer, user, message, clientAgent)
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
            : this(New.Guid(), uri, null, sender, referrer, user, message, clientAgent)
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
            : base(uri, expression, sender)
        {
            _id = id;
            _referrer = referrer;
            _user = user;
            _message = message;
            _clientAgent = clientAgent;
        }
        
        public IUri Referrer
        {
            get { return _referrer; }
            internal set { _referrer = value; }
        }

        public string Message
        {
            get { return _message; }
        }

        public User User
        {
            get { return _user; }
            internal set { _user = value; }
        }

        public ClientAgent ClientAgent
        {
            get { return _clientAgent; }
            internal set { _clientAgent = value; }
        }
        
        /// <summary>
        /// Gets the request id.
        /// </summary>
        public Guid Id
        {
            get { return _id; }
            internal set { _id = value; }
        }
    }
}