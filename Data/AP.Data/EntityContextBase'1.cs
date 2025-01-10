using System;

namespace AP.Data;

/// <summary>
/// Base class for EntityContexts with an external DataProvider.
/// </summary>
/// <typeparam name="TDataProvider">The DataProvider.</typeparam>
public abstract class EntityContextBase<TDataProvider> : EntityContextBase
    where TDataProvider : class
{
    private TDataProvider _inner;
    private readonly bool _canDisposeContext;

    /// <summary>
    /// Gets the DataProvider.
    /// </summary>
    public TDataProvider Provider
    {
        get 
        { 
            this.ThrowIfDisposed();
            return _inner;
        }            
    }

    protected EntityContextBase(TDataProvider dataContext, bool ownsProvider = true, object? contextKey = null)
        : base(contextKey)
    {
        ArgumentNullException.ThrowIfNull(dataContext);

        _inner = dataContext;
        _canDisposeContext = ownsProvider;

        if (!ownsProvider && dataContext is AP.IContextDependentDisposable)
        {
            ((AP.IContextDependentDisposable)dataContext).Disposed += DataContextDisposed;
        }
    }

    private void DataContextDisposed(object sender, EventArgs e)
    {
        if (_inner != null)
        {
            ((AP.IContextDependentDisposable)_inner).Disposing -= this.DataContextDisposed;
            this.CleanUpResources();
        }
    }
    
    protected override void CleanUpResources()
    {
        base.CleanUpResources();
     
        if (_canDisposeContext && _inner != null)
            _inner.TryDispose();

        _inner = null!;
    }
}
