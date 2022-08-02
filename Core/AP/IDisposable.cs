using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP
{
    /// <summary>
    /// Delegate used when an AP.IDisposable is about to be disposed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DisposingEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Delegate used when an AP.IDisposable has been disposed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DisposedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Extends the System.IDisposable by exposing events; listening objects can run cleanup code without implementing IDisposable on their own.
    /// </summary>
    public interface IDisposable : System.IDisposable
    {
        /// <summary>
        /// Event occurs when the object is about to be disposed
        /// </summary>
        event DisposingEventHandler Disposing;

        /// <summary>
        /// Event occurs when the object has been disposed
        /// </summary>
        event DisposedEventHandler Disposed;

        /// <summary>
        /// Disposes the object by utilizing a key
        /// </summary>
        /// <param name="contextKey">The key for disposing the object</param>
        void Dispose(object contextKey);

        /// <summary>
        /// Indicates if the object has been disposed
        /// </summary>
        bool IsDisposed { get; }        
    }
}
