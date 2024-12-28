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
    public event ErrorLogger ErrorOccured;
    
    /// <summary>
    /// Todo: add extensions for funcs, actions (along with their hilariously many parameters)
    /// returns true if no error occured
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
    private void Notify(object sender, Exception exception) => this.OnErrorOccured(this.CreateErrorLoggerEventArgs(this.CreateExceptionInfo(sender, exception)));

    /// <summary>
    /// Creates the ErrorLoggerEventArgs
    /// </summary>
    /// <param name="errorInfo"></param>
    /// <returns></returns>
    protected virtual ErrorLoggerEventArgs CreateErrorLoggerEventArgs(ExceptionInfo errorInfo) => new(errorInfo);

    /// <summary>
    /// Creates an ErrorInfo object
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    protected virtual ExceptionInfo CreateExceptionInfo(object sender, Exception exception) => new(sender, exception);

    /// <summary>
    /// Notifies the event listeners
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnErrorOccured(ErrorLoggerEventArgs e)
    {
        ErrorLogger handler = this.ErrorOccured;

        if (handler != null)
            handler(this, e);
    }

    protected override void CleanUpResources()
    {
        base.CleanUpResources();

        this.ErrorOccured = null;
    }
}
