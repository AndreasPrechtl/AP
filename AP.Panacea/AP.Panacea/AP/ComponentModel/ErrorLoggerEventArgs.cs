using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.ComponentModel
{
    /// <summary>
    /// Provides info about the error for logging purposes
    /// </summary>
    public class ErrorLoggerEventArgs
    {
        public ExceptionInfo ErrorInfo { get; private set; }

        public ErrorLoggerEventArgs(ExceptionInfo error)
        {
            this.ErrorInfo = error;
        }     
    }
}
