using AP.Routing;
using AP.Security;
using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AP.Panacea
{
    public class Navigator<TRequest, TResponse> : INavigator<TRequest, TResponse>
        where TRequest : Request
        where TResponse : Response
    {
        private IApplication<TRequest, TResponse> _application;
        private IUri _currentUri;
        private readonly bool _applicationIsNavigator;

        protected IApplication<TRequest, TResponse> Application { get { return _application; } }

        /// <summary>
        /// Creates a new Navigator.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="applicationIsNavigator">Determines if the application should be used as the Navigator reference for the event args, default is false.</param>
        public Navigator(IApplication<TRequest, TResponse> application, bool applicationIsNavigator = false)
        {
            ArgumentNullException.ThrowIfNull(application);

            application.Disposed += Application_Disposed;
            _application = application;
            _applicationIsNavigator = applicationIsNavigator;
        }
        
        private void Application_Disposed(object sender, EventArgs e)
        {
            _application.Disposed -= Application_Disposed;
            _application = null;
        }

        public bool ApplicationIsNavigator
        {
            get { return _applicationIsNavigator; }
        }

        #region INavigator<TRequest, TResponse> Members

        public virtual void Navigate(TRequest request, bool updateCurrentUri = true)
        {
            bool cancel;
            this.OnNavigating(request, out cancel, updateCurrentUri);

            if (cancel)
                return;

            TResponse response = _application.GetResponse(request);

            this.OnNavigated(response, updateCurrentUri);
        }

        protected virtual void OnNavigating(TRequest request, out bool cancel, bool updateCurrentUri = true)
        {
            // set a default value.
            cancel = false;

            NavigatingEventHandler<TRequest, TResponse> handler = this.Navigating;

            if (handler != null)
            {
                NavigatingEventArgs<TRequest, TResponse> e = new NavigatingEventArgs<TRequest, TResponse>(_applicationIsNavigator ? _application : (INavigator<TRequest, TResponse>)this, request, updateCurrentUri);

                foreach (NavigatingEventHandler<TRequest, TResponse> current in handler.GetInvocationList())
                {
                    // invoke the handler
                    current(this, e);

                    // stop further handlers from receiving a canceled request.
                    if (cancel = e.Cancel)
                        break;
                }
            }
        }

        protected virtual void OnNavigated(TResponse response, bool updateCurrentUri = true)
        {
            NavigatedEventHandler<TRequest, TResponse> handler = this.Navigated;

            this.UpdateCurrentUri(response.Uri, updateCurrentUri);
            this.Render(response);

            if (handler != null)
                handler(this, new NavigatedEventArgs<TRequest, TResponse>(_applicationIsNavigator ? _application : (INavigator<TRequest, TResponse>)this, response, updateCurrentUri));
        }

        protected virtual void UpdateCurrentUri(IUri uri, bool updateCurrentUri = true)
        {
            if (updateCurrentUri)
                _currentUri = uri;
        }

        public event NavigatingEventHandler<TRequest, TResponse> Navigating;
        public event NavigatedEventHandler<TRequest, TResponse> Navigated;

        public TResponse GetResponse(TRequest request)
        {
            return _application.GetResponse(request);
        }

        public IUri GetUri(TRequest request, bool testAuthorization = true)
        {
            return _application.GetUri(request);
        }

        /// <summary>
        /// Gets the current Uri; If the Application is the prime navigator, the Application's current Uri is used instead.
        /// </summary>
        public IUri CurrentUri
        {
            get { return _applicationIsNavigator ? _application.CurrentUri : _currentUri; }
        }

        public User CurrentUser
        {
            get { return _application.CurrentUser; }
        }

        public ClientAgent CurrentClientAgent
        {
            get { return _application.CurrentClientAgent; }
        }

        #endregion

        #region IResponseRenderer<TResponse> Members

        public virtual void Render(TResponse response)
        {
            _application.Render(response);
        }

        #endregion
    }
}
