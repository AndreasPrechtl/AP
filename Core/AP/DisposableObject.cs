using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AP;

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
    public event DisposingEventHandler? Disposing;

    /// <summary>
    /// Fired when the object has been disposed.
    /// </summary>
    public event DisposedEventHandler? Disposed;

    private object? _contextKey;

    /// <summary>
    /// Creates a new inherited DisposableObject
    /// </summary>
    /// <param name="contextKey">The key that can be used to safely dispose the object</param>
    protected DisposableObject(object? contextKey = null)
    {
        _contextKey = contextKey;
    }

    void System.IDisposable.Dispose() => this.Dispose();

    internal virtual void SuppressFinalizeIfNeeded()
    { }
       
    /// <summary>
    /// Raises the Disposing event
    /// </summary>
    /// <param name="e">The EventArgs that should be used.</param>
    protected void OnDisposing(EventArgs? e = null) => this.Disposing?.Invoke(this, e ?? EventArgs.Empty);        
    
    /// <summary>
    /// Raises the Disposed event.
    /// </summary>
    /// <param name="e">The EventArgs that should be used.</param>
    protected void OnDisposed(EventArgs? e = null) => this.Disposed?.Invoke(this, e ?? EventArgs.Empty);

    /// <summary>
    /// Customizable cleanup code.
    /// </summary>
    protected virtual void CleanUpResources()
    { }

    /// <summary>
    /// Returns true when the object has been disposed.
    /// </summary>
    public bool IsDisposed => _isDisposed;

    /// <summary>
    /// Throws an exception if the object has already been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_isDisposed)
            throw new ObjectDisposedException("Object is already disposed", default(Exception));
    }

    public override bool Equals(object? obj)
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
        return base.ToString()!;
    }

    #region IContextualDisposable Members

    /// <summary>
    /// Disposes the instance.
    /// </summary>
    /// <param name="contextKey">The contextKey for disposing the object.</param>        
    public void Dispose(object? contextKey = null)
    {            
        if (!_isDisposed)
        {
            if (_contextKey?.Equals(contextKey) is true)
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

    ValueTask IAsyncDisposable.DisposeAsync() => this.DisposeAsync();

    public ValueTask DisposeAsync(object? contextKey = null)
    {
        this.Dispose(contextKey);
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// The context key for disposing the object.
    /// </summary>
    protected object? ContextKey => _contextKey;

    #endregion
}