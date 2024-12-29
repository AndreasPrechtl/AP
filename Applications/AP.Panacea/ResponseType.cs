using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Panacea
{
    public enum ResponseType
    {
        Authorized = 0,
        Unauthorized = 1,
        RouteNotFound = 3,
        RouteBlocked = 5
    }
}
