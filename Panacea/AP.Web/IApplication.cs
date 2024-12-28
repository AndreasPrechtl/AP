using System;
using AP.ComponentModel.ObjectManagement;

namespace AP.Web
{
    public interface IApplication
    {
        event ApplicationEventHandler RequestEnded;
        event ApplicationEventHandler RequestStarted;

        event ApplicationEventHandler SessionStarted;
        event ApplicationEventHandler SessionEnded;

        event ApplicationEventHandler ApplicationStarted;
        event ApplicationEventHandler ApplicationEnded;

        event ApplicationErrorEventHandler Error;

        IObjectManager ObjectManager { get; }

        HttpApplicationState State { get; }
    }
}
