using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace AP.ComponentModel.ObjectManagement
{    
    internal interface IObjectLifetimeInternal : AP.IDisposable
    {
        object Key { get; }
        //object Instance { get; }
    }
}
