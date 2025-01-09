using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Panacea
{
    public class NavigatingEventArgs<TRequest, TResponse> : NavigationEventArgsBase<TRequest, TResponse>
        where TRequest : Request
        where TResponse : Response
    {
        public TRequest Request { get; private set; }

        public NavigatingEventArgs(INavigator<TRequest, TResponse> navigator, TRequest request, bool updateUri = true)
            : base(navigator, updateUri)
        {
            ArgumentNullException.ThrowIfNull(request);

            this.Request = request;
        }

        public bool Cancel { get; set; }
    }
}
