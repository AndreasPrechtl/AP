using AP.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Panacea
{
    public class NavigatedEventArgs<TRequest, TResponse> : NavigationEventArgsBase<TRequest, TResponse>
        where TRequest : Request
        where TResponse : Response
    {
        public TResponse Response { get; private set; }

        public NavigatedEventArgs(INavigator<TRequest, TResponse> navigator, TResponse response, bool updateCurrentUri = true)
            : base(navigator, updateCurrentUri)
        {
            ArgumentNullException.ThrowIfNull(response);

            this.Response = response;
        }
    }
}
