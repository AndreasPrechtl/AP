using System;


namespace AP.ComponentModel;

/// <summary>
/// Delegate used for the ErrorOccured event
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public delegate void ErrorLogger(object sender, ErrorLoggerEventArgs e);

/// <summary>
/// Provides event based ErrorLogging capabilities
/// </summary>
public class Sandbox : DisposableObject
{
    /// <summary>
    /// Event used for external error logging
    /// </summary>
    public event ErrorLogger? ErrorOccured;
    
    /// <summary>
    /// Todo: add extensions for funcs, actions (along with their hilariously many parameters)
    /// returns true if no error occurred
    /// <param name="action"></param>
    /// <param name="notify"></param>
    /// <returns></returns>
    public bool Try(object context, Delegate action, out object? result, bool notify = true, params object[] args)
    {
        result = null;

        try
        {
            result = action.DynamicInvoke(args);
            return true;
        }
        catch (Exception exception)
        {
            if (notify)
                this.Notify(context, exception);
        }
        return false;           
    }

    /// <summary>
    /// Notifies all loggers that an error has occured
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="exception"></param>
    private void Notify(object sender, Exception exception) => this.OnErrorOccurred(new ErrorLoggerEventArgs(new ExceptionInfo() { Context = sender, Exception = exception }));

    /// <summary>
    /// Notifies the event listeners
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnErrorOccurred(ErrorLoggerEventArgs e)
    {
        this.ErrorOccured?.Invoke(this, e);
    }

    protected override void CleanUpResources()
    {
        base.CleanUpResources();

        this.ErrorOccured = null;
    }
}
