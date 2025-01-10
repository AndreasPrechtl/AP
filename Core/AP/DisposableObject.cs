using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AP;

/// <summary>
/// Used as a base class for IDisposable implementations, does not contain a finalizer
/// </summary>
[SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "IDisposable is implemented correctly - yet simplified for less complexity on child classes")]
public abstract class DisposableObject : AP.IContextDependentDisposable
{
    private bool _isDisposed;
    private object? _contextKey;

    /// <summary>
    /// Fired when the object is about to be disposed.
    /// </summary>
    public event DisposingEventHandler? Disposing;

    /// <summary>
    /// Fired when the object has been disposed.
    /// </summary>
    public event DisposedEventHandler? Disposed;

    /// <summary>
    /// The context key for disposing the object.
    /// </summary>
    protected object? ContextKey => _contextKey;

    /// <summary>
    /// Creates a new inherited DisposableObject
    /// </summary>
    /// <param name="contextKey">The key that can be used to safely dispose the object</param>
    protected DisposableObject(object? contextKey = null)
    {
        _contextKey = contextKey;
    }

    internal virtual void SuppressFinalizeIfNeeded() { }
       
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
    protected abstract void CleanUpResources();

    /// <summary>
    /// Customizable cleanup code for async disposals.
    /// </summary>
    protected abstract ValueTask CleanUpResourcesAsync();

    /// <summary>
    /// Returns true when the object has been disposed.
    /// </summary>
    public bool IsDisposed => _isDisposed;

    /// <summary>
    /// Throws an exception if the object has already been disposed.
    /// </summary>
    protected void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_isDisposed, this);

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
        DisposeInternal(CleanUpResources, contextKey);
    }

    private void DisposeInternal(Action cleanup, object? contextKey = null)
    {
        if (!_isDisposed)
        {
            if (_contextKey?.Equals(contextKey) is true)
            {
                this.OnDisposing(EventArgs.Empty);

                try { cleanup(); }
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
    /// Disposes the instance asynchronously.
    /// </summary>
    /// <param name="contextKey">The contextKey for disposing the object.</param>        
    public ValueTask DisposeAsync(object? contextKey = null)
    {
        DisposeInternal(async () => await CleanUpResourcesAsync(), contextKey);
        return ValueTask.CompletedTask;
    }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    void IDisposable.Dispose() => this.Dispose();
    ValueTask IAsyncDisposable.DisposeAsync() => this.DisposeAsync();
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize

    #endregion
}