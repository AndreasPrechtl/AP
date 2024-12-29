using AP.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Panacea
{    
    public abstract class NavigationEventArgsBase<TRequest, TResponse> : EventArgs
        where TRequest : Request
        where TResponse : Response
    {
        public INavigator<TRequest, TResponse> Navigator { get; private set; }
        public bool UpdateCurrentUri { get; private set; }

        internal NavigationEventArgsBase(INavigator<TRequest, TResponse> navigator, bool updateCurrentUri = true)
        {
            if (navigator == null)
                throw new ArgumentNullException("navigator");

            this.Navigator = navigator;
            this.UpdateCurrentUri = updateCurrentUri;
        }
    }
}
