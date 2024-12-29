using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Panacea
{
    public interface IBasicNavigator<TRequest, TResponse>
        where TRequest : Request
        where TResponse : Response<TRequest>
    {
        void Navigate(TRequest request, bool updateUri = true);
    }
}
