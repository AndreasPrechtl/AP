using System;

namespace AP.ComponentModel.ObjectManagement;

internal interface IManagedInstanceInternal : System.IDisposable
{
    object Value { get; }
    bool IsDisposed { get; }
}

public sealed class ManagedInstance<TBase> : System.IDisposable, IWrapper<TBase>, IManagedInstanceInternal
    where TBase : notnull
{
    private TBase _value;
    private readonly bool _canDisposeInstance;
    private bool _isDisposed;
    private Action? _callback;

    /// <summary>
    /// Creates a new ManagedInstance.
    /// </summary>
    /// <param name="instance">The instance to manage.</param>
    /// <param name="canDisposeInstance">When true, the instance will disposed alongside.</param>
    public ManagedInstance(TBase instance, bool canDisposeInstance = false, Action? disposedCallback = null)            
    {
        ArgumentNullException.ThrowIfNull(instance);

        _value = instance;
        _canDisposeInstance = canDisposeInstance;
        _callback = disposedCallback;

        if (instance is AP.IContextDependentDisposable)
            ((AP.IContextDependentDisposable)instance).Disposing += this.OnValueDisposing;   
    }

    public static ManagedInstance<TBase> Create(TBase instance, bool canDisposeInstance = false) => new(instance, canDisposeInstance);

    public bool IsDisposed => _isDisposed;

    public bool CanDisposeInstance => _canDisposeInstance;

    private void OnValueDisposing(object sender, EventArgs e)
    {
        TBase value = _value;

        if (value != null)
            ((AP.IContextDependentDisposable)value).Disposing -= this.OnValueDisposing;
        
        this.Dispose();
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_isDisposed, this);

    #region IWrapper<TBase> Members

    /// <summary>
    /// Gets the actual instance.
    /// </summary>
    public TBase Value
    {
        get
        {
            this.ThrowIfDisposed();
            return _value; 
        }
    }

    #endregion

    public void Dispose()
    {
        TBase value = _value;

        if (_isDisposed)
            return;

        if (_canDisposeInstance && value != null)
        {
            if (value is AP.IContextDependentDisposable)
                ((AP.IContextDependentDisposable)value).Disposing -= this.OnValueDisposing;

            value.TryDispose();
        }

        _isDisposed = true;
        
        _callback?.Invoke();

        _value = default!;
    }

    public override bool Equals(object? obj)
    {
        this.ThrowIfDisposed();

        if (obj == null)
            return false;

        if (this == obj)
            return true;

        if (obj is IManagedInstanceInternal)
            return _value!.Equals(((IManagedInstanceInternal)obj).Value);

        return _value!.Equals(obj);
    }

    public override int GetHashCode()
    {
        this.ThrowIfDisposed();
        return _value!.GetHashCode();
    }

    public override string ToString()
    {
        this.ThrowIfDisposed();
        return _value!.ToString()!;
    }

    public static implicit operator TBase(ManagedInstance<TBase> instance)
    {
        return instance.Value;
    }

    #region IManagedInstanceInternal Members

    object IManagedInstanceInternal.Value => _value;

    #endregion
}
