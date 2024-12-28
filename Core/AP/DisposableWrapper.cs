using System;
using AP.ComponentModel;

namespace AP;

internal interface IDisposableWrapperInternal : AP.IDisposable
{
    object Value { get; }
}

/// <summary>
/// Wraps any object into a disposable context
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class DisposableWrapper<T> : DisposableObject, IWrapper<T>, IDisposableWrapperInternal
{
    private T _value;
    private readonly bool _canDisposeInstance;

    public DisposableWrapper(T value, object? contextKey = null, bool canDisposeValue = true)
        : base(contextKey)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        _value = value; 
        _canDisposeInstance = canDisposeValue;
        
        if (value is AP.IDisposable)
            ((AP.IDisposable)value).Disposing += this.OnValueDisposing;             
    }

    public bool CanDisposeInstance => _canDisposeInstance;

    public T Value
    {
        get
        {
            this.ThrowIfDisposed();
            return _value;
        }
    }

    private void OnValueDisposing(object sender, EventArgs e)
    {
        T value = _value;

        if (value != null)
            ((AP.IDisposable)value).Disposing -= this.OnValueDisposing;
        
        this.Dispose();
    }

    protected override void CleanUpResources()
    {
        if (base.IsDisposed)
            return;

        if (_canDisposeInstance)
        {
            T value = _value;
            if (value != null)
            {
                if (value is AP.IDisposable)
                    ((AP.IDisposable)value).Disposing -= this.OnValueDisposing;

                value.TryDispose();
            }                
        }
        _value = default;

        base.CleanUpResources();
    }

    public override bool Equals(object obj)
    {
        this.ThrowIfDisposed();

        if (obj == null)
            return false;

        if (this == obj)
            return true;

        if (obj is IDisposableWrapperInternal)
            return _value.Equals(((IDisposableWrapperInternal)obj).Value);

        return _value.Equals(obj);
    }

    public override int GetHashCode()
    {
        this.ThrowIfDisposed();
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        this.ThrowIfDisposed();
        return _value.ToString();
    }

    public static implicit operator T(DisposableWrapper<T> wrapper)
    {
        return wrapper.Value;
    }

    public static explicit operator DisposableWrapper<T>(T value)
    {
        return new DisposableWrapper<T>(value);
    }

    #region IDisposableWrapperInternal Members

    object IDisposableWrapperInternal.Value => _value;

    #endregion
}
