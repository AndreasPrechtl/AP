namespace AP.ComponentModel.ObjectManagement;

internal interface IObjectLifetimeInternal : AP.IDisposable
{
    object Key { get; }
    //object Instance { get; }
}
