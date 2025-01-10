namespace AP.ComponentModel.ObjectManagement;

internal interface IObjectLifetimeInternal : AP.IContextDependentDisposable
{
    object Key { get; }
}
