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
    public delegate void NavigatedEventHandler<TRequest, TResponse>(object sender, NavigatedEventArgs<TRequest, TResponse> e)
        where TRequest : Request
        where TResponse : Response;

    public delegate void NavigatingEventHandler<TRequest, TResponse>(object sender, NavigatingEventArgs<TRequest, TResponse> e)
        where TRequest : Request
        where TResponse : Response;
    
    public interface INavigator<TRequest, TResponse> : IResponseRenderer<TResponse>
        where TRequest : Request
        where TResponse : Response
    {
        void Navigate(TRequest request, bool updateCurrentUri = true);
        
        event NavigatingEventHandler<TRequest, TResponse> Navigating;
        event NavigatedEventHandler<TRequest, TResponse> Navigated;        

        IUri CurrentUri { get; }
    }
}
