using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Navigation;
using AP.ComponentModel.ObjectManagement;
using AP.Routing;
using AP.Security;
using AP.UniformIdentifiers;
using AP.UI;
using AP.UI.SiteMapping;
using AP.Configuration;
using System.Windows.Controls;
using AP.Reflection;
using AP.Windows.Controls;
using AP.Linq;
using System.Reflection;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AP.Panacea.Windows
{
    public abstract class ApplicationBase : System.Windows.Application, 
        IApplication<Request, Response>, 
        IResponseRenderer<Response, ContentControl>, 
        IResponseRenderer<Response, ContentPresenter>,
        INotifyPropertyChanged
    {
        private ContentControl _renderTarget;
        
        private readonly Deferrable<ApplicationCore> _core;
        private readonly Deferrable<Navigator<Request, Response>> _navigator;
        private IUri _currentUri;
        private bool _integratedNavigation;
                       
        protected ApplicationBase()
        {
            _core = new Deferrable<ApplicationCore>(this.CreateCore);
            _navigator = new Deferrable<Navigator<Request, Response>>(this.CreateNavigator);
        }

        protected virtual Navigator<Request, Response> CreateNavigator()
        {
            return new Navigator<Request, Response>(this, true);
        }

        protected virtual ApplicationCore CreateCore()
        {
            return new ApplicationCore(this);
        }

        protected ApplicationCore Core { get { return _core.Value; } }
        protected Navigator<Request, Response> Navigator { get { return _navigator.Value; } }
        
        public bool IntegratedNavigation
        {
            get { return _integratedNavigation; }
            set { _integratedNavigation = value; }
        }
        
        /// <summary>
        /// Loads the rendering target, can be either a Frame or a NavigationWindow.
        /// </summary>
        /// <returns>The rendering target.</returns>
        /// <remarks>Should return null when the application or main window has not been loaded yet.</remarks>
        protected abstract ContentControl LoadRenderingTarget();
        
        /// <summary>
        /// Gets a reference to the Frame or NavigationWindow.
        /// </summary>
        protected ContentControl RenderTarget { get { return _renderTarget; } }
        
        /// <summary>
        /// Gets the NavigationService.
        /// </summary>
        /// <remarks>Property added because Navigation events were already available for the Application - but not the NavigationService!?</remarks>
        public NavigationService NavigationService
        {
            get 
            {
                if (_renderTarget is Frame)
                    return ((Frame)_renderTarget).NavigationService;
                else if (_renderTarget is NavigationWindow)
                    return ((NavigationWindow)_renderTarget).NavigationService;

                return null;
            }
        }

        private bool PrepareNavigation()
        {
            if (this.MainWindow == null)
                return false;

            ContentControl renderTarget = _renderTarget;
            
            if (renderTarget == null)
            {
                renderTarget = this.LoadRenderingTarget();

                if (renderTarget != null)
                {
                    if (renderTarget is Frame)
                    {
                        Frame f = (Frame)renderTarget;
                        f.UseJournalEntryName(true);
                        f.CanInfuseContent(true);
                        _renderTarget = renderTarget;
                    }                    
                    else if (renderTarget is NavigationWindow)
                    {
                        NavigationWindow w = (NavigationWindow)renderTarget;
                        w.UseJournalEntryName(true);
                        w.CanInfuseContent(true);                       
                        _renderTarget = renderTarget;
                    }
                    else
                        throw new InvalidOperationException("RenderTarget has to be a Frame or NavigationWindow.");
                }                    
            }

            return renderTarget != null;
        }

        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            if (e.Content is Response)
                this.CurrentUri = ((Response)e.Content).Uri;

            base.OnLoadCompleted(e);
        }
        
        protected override void OnNavigating(NavigatingCancelEventArgs e)
        {
            Response response = e.Content as Response;

            // if there's a response, just use it
            if (response != null)
            {
                base.OnNavigating(e);
                return;
            }

            // intercept all other possibilities
            if (e.NavigationMode != NavigationMode.New || e.Cancel || _integratedNavigation || !this.PrepareNavigation() || e.Content is CustomContentState)
            {                
                base.OnNavigating(e);
                return;
            }            
            
            // create a response, cancel and restart the navigation with the proper response
            // 1st the request
            object target = e.Content ?? e.Uri;
            target = target ?? e.WebRequest;

            Request request = e.Content as Request ?? this.CreateRequest(target);
                       
            response = this.Core.GetResponse(request);
                      
            // cancel the current navigation attempt and restart it with the response for e.Content
            e.Cancel = true;
            base.OnNavigating(e);
                        
            // just the problem - how do I generate the history entry's title?
            this.NavigationService.Navigate(response);
        }

        #region IApplication Members
        
        /// <summary>
        /// Gets the application instance.
        /// </summary>
        private static new ApplicationBase Current { get { return (AP.Panacea.Windows.ApplicationBase)System.Windows.Application.Current; } }

        /// <summary>
        /// Gets the application instance.
        /// </summary>
        public static ApplicationBase Instance { get { return Current; } }
        
        /// <summary>
        /// Creates a Request instance from an object.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>The request.</returns>
        public virtual Request CreateRequest(object target)
        {
            Request request;

            if (target is Expression<ResultCreator>)
                request = new Request((Expression<ResultCreator>)target, this);
            else if (target is Expression<Func<object>>)
                request = new Request(((Expression<Func<object>>)target).Cast<ResultCreator>(), this);
            else if (target is IUri)
                request = new Request((IUri)target, this);
            else if (target is string)
                request = new Request(new HttpUrl((string)target), this);
            else if (target is System.Uri)
                request = new Request(new HttpUrl(target.ToString()), this);
            else if (target is System.Net.WebRequest)
                request = new Request(new HttpUrl(((System.Net.WebRequest)target).RequestUri.ToString()), this);
            else
                throw new ArgumentException("target");

            return request;
        }

        public SiteMap<Request> SiteMap { get { return this.Core.SiteMap; } }

        public IUri GetUri(Request request, bool testAuthorization)
        {
            return this.Core.GetUri(request, testAuthorization);
        }

        public IObjectManager ObjectManager
        {
            get { return this.Core.ObjectManager; }
        }

        public MembershipContextBase Membership
        {
            get { return this.Core.Membership; }
        }

        public Response GetResponse(Request requestContext)
        {
            return this.Core.GetResponse(requestContext);
        }

        public bool IsAuthorized(Request requestContext)
        {
            return this.Core.IsAuthorized(requestContext);
        }

        public RouteTable<Request> RouteTable
        {
            get { return this.Core.RouteTable; }
        }

        #endregion

        #region INavigator Members

        public void Navigate(Request request, bool updateCurrentUri = true)
        {
            if (updateCurrentUri)
                this.NavigationService.Navigate(request);
            else
                this.Render(this.Core.GetResponse(request));                
        }

        #endregion

        #region IDisposable Members

        public event DisposingEventHandler Disposing
        {
            add { this.Core.Disposing += value; }
            remove { this.Core.Disposing -= value; }
        }

        public event DisposedEventHandler Disposed
        {
            add { this.Core.Disposed += value; }
            remove { this.Core.Disposed -= value; }
        }

        public void Dispose(object contextKey)
        {
            if (_core != null)
                this.Core.Dispose(contextKey);
        }

        public bool IsDisposed
        {
            get { return this.Core.IsDisposed; }
        }

        #endregion

        #region System.IDisposable Members

        public void Dispose()
        {
            if (_core.IsValueActive)
                _core.TryDispose();
        }

        #endregion


        #region IResponseRenderer<Response, Control> Members

        public void Render(Response response, ContentControl renderTarget = null)
        {            
            if (renderTarget != null)
                renderTarget.Content = response.Result;
            else
                this.Render(response);
        }

        public void Render(Response response, ContentPresenter renderTarget = null)
        {
            if (renderTarget != null)
                renderTarget.Content = response.Result;
            else
                this.Render(response);
        }

        #endregion

        #region IResponseRenderer<Response> Members

        public void Render(Response response)
        {
            ContentControl renderTarget = this.RenderTarget;

            if (renderTarget is Frame)
                ((Frame)renderTarget).Infuse(response);
            else
                ((NavigationWindow)renderTarget).Infuse(response);
        }

        #endregion

        ApplicationCore<Request, Response> IApplication<Request, Response>.Core
        {
            get { return _core; }
        }

        public virtual User CurrentUser
        {
            get 
            {
                if (this.Core.Membership != null)
                {
                    User user = new User(System.Security.Principal.WindowsIdentity.GetCurrent().Name);

                    //if (user.Exists)
                    return user;
                }

                return null; 
            }
        }

        public ClientAgent CurrentClientAgent
        {
            get { return new ClientAgent(Environment.MachineName, Environment.OSVersion.VersionString); }
        }

        public new event NavigatingEventHandler<Request, Response> Navigating 
        { 
            add { this.Navigator.Navigating += value; }
            remove { this.Navigator.Navigating -= value; }
        }
        
        public new event NavigatedEventHandler<Request, Response> Navigated
        {
            add { this.Navigator.Navigated += value; }
            remove { this.Navigator.Navigated -= value; }
        }

        /// <summary>
        /// Gets the current Uri.
        /// </summary>
        /// <remarks>Enabled one way binding via INotifyPropertyChanged.</remarks>
        public IUri CurrentUri
        {
            get { return _currentUri; }
            protected set 
            {
                if (!object.Equals(_currentUri, value))
                {
                    _currentUri = value;
                    this.OnPropertyChanged();
                }
            }
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        { 
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
