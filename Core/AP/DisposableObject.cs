using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace AP
{
    /// <summary>
    /// Used as a base class for IDisposable implementations, does not contain a finalizer
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "IDisposable is implemented correctly - yet simplified for less complexity on child classes")]
    public abstract class DisposableObject : IDisposable
    {
        private bool _isDisposed;

        /// <summary>
        /// Fired when the object is about to be disposed.
        /// </summary>
        public event DisposingEventHandler Disposing;

        /// <summary>
        /// Fired when the object has been disposed.
        /// </summary>
        public event DisposedEventHandler Disposed;

        private object _contextKey;

        /// <summary>
        /// Creates a new inherited DisposableObject
        /// </summary>
        /// <param name="contextKey">The key that can be used to safely dispose the object</param>
        protected DisposableObject(object contextKey = null)
        {
            _contextKey = contextKey;
        }
        
        /// <summary>
        /// Disposes the object when the ContextKey is null.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]        
        public void Dispose()
        {
            this.Dispose(null);
        }

        internal virtual void SuppressFinalizeIfNeeded()
        { }
           
        /// <summary>
        /// Raises the Disposing event
        /// </summary>
        /// <param name="e">The eventargs that should be used.</param>
        protected void OnDisposing(EventArgs e = null)
        {
            DisposingEventHandler handler = this.Disposing;
            if (handler != null)
                handler(this, e ?? EventArgs.Empty);
        }

        /// <summary>
        /// Raises the Disposed event.
        /// </summary>
        /// <param name="e">The eventargs that should be used.</param>
        protected void OnDisposed(EventArgs e = null)
        {            
            DisposedEventHandler handler = this.Disposed;
            if (handler != null)
                handler(this, e ?? EventArgs.Empty);
        }

        /// <summary>
        /// Customizable cleanup code.
        /// </summary>
        protected virtual void CleanUpResources()
        { }

        /// <summary>
        /// Returns true when the object has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }
        
        /// <summary>
        /// Throws an exception if the object has already been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException("Object is already disposed", (Exception)null);
        }

        public override bool Equals(object obj)
        {
            this.ThrowIfDisposed();
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            this.ThrowIfDisposed();
            return base.GetHashCode();
        }

        public override string ToString()
        {
            this.ThrowIfDisposed();
            return base.ToString();
        }

        #region IContextualDisposable Members

        /// <summary>
        /// Disposes the instance.
        /// </summary>
        /// <param name="contextKey">The contextKey for disposing the object.</param>        
        public void Dispose(object contextKey)
        {            
            if (!_isDisposed)
            {
                // use equals or referencial equality?
                if (_contextKey == null || object.Equals(_contextKey, contextKey))
                {
                    this.OnDisposing(EventArgs.Empty);

                    try { this.CleanUpResources(); }
                    catch (Exception) { }

                    _isDisposed = true;
                    this.OnDisposed(EventArgs.Empty);

                    // remove the event listeners
                    this.Disposing = null;
                    this.Disposed = null;

                    _contextKey = null;

                    this.SuppressFinalizeIfNeeded();
                }// throw or not?
                else
                    throw new InvalidOperationException("Cannot dispose without the proper contextKey");
            }
        }

        /// <summary>
        /// The context key for disposing the object.
        /// </summary>
        protected object ContextKey { get { return _contextKey; } }
        
        #endregion
    }
}