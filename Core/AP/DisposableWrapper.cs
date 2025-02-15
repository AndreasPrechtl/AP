﻿using System;
using System.Threading.Tasks;

namespace AP;

internal interface IDisposableWrapperInternal : AP.IContextDependentDisposable
{
    object Value { get; }
}

/// <summary>
/// Wraps any object into a disposable context
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class DisposableWrapper<T> : DisposableObject, IWrapper<T>, IDisposableWrapperInternal
    where T : notnull
{
    private T _value;
    private readonly bool _canDisposeInstance;

    public DisposableWrapper(T value, object? contextKey = null, bool canDisposeValue = true)
        : base(contextKey)
    {
        ArgumentNullException.ThrowIfNull(value);

        _value = value; 
        _canDisposeInstance = canDisposeValue;
        
        if (value is AP.IContextDependentDisposable disposable)
            disposable.Disposing += this.OnValueDisposing;
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

        if (value is AP.IContextDependentDisposable disposable)
            disposable.Disposing -= this.OnValueDisposing;
        
        this.Dispose();
    }

    protected override void CleanUpResources()
    {
        CleanUpResourcesInternal(() => _value?.TryDispose(this.ContextKey));
    }

    protected override ValueTask CleanUpResourcesAsync()
    {
        CleanUpResourcesInternal(async () => await _value.TryDisposeAsync(this.ContextKey));
        return ValueTask.CompletedTask;
    }

    private void CleanUpResourcesInternal(Action cleanup)
    {
        T? value = _value;
        if (value is AP.IContextDependentDisposable disposable)
            disposable.Disposing -= this.OnValueDisposing;

        if (_canDisposeInstance && value is not null)
            cleanup();

        _value = default!;
    }


    public override bool Equals(object? obj)
    {
        this.ThrowIfDisposed();

        if (obj is null)
            return false;

        if (this == obj)
            return true;

        if (obj is IDisposableWrapperInternal wrapper)
            return _value.Equals(wrapper.Value);

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
        return _value.ToString()!;
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
