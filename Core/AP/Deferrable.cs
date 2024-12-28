using AP.ComponentModel;
using System;

namespace AP;

/// <summary>
/// Provides lazy loading and freezing capabilities.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class Deferrable<T> : FreezableObject, IWrapper<T>
{
    private object _value;
    private object _defaultValue;
    private Activator<T> _activator;

    /// <summary>
    /// Creates a new Deferrable.
    /// </summary>
    /// <param name="activator">The ValueCreator</param>
    /// <param name="isFrozen">Indicates if the object should be frozen.</param>
    public Deferrable(Activator<T>? activator = null, bool isFrozen = false)
        : base(isFrozen)
    {       
        _activator = activator ?? System.Activator.CreateInstance<T>;
    }

    /// <summary>
    /// Creates a new Deferrable.
    /// </summary>
    /// <param name="activator">The Value</param>
    /// <param name="isFrozen">Indicates if the object should be frozen.</param>
    public Deferrable(T value, bool isFrozen = false)
        : this(() => value, isFrozen)
    {
        _value = value;
    }

    /// <summary>
    /// Indicates if the value has been loaded.
    /// </summary>
    public bool IsValueActive => _value != null;

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <exception cref="System.InvalidOperationException" />
    public T Value
    {
        get
        {
            object v = _value;
            if (v == null)
                _value = v = this.DefaultValue;

            return (T)v;
        }
        set
        {
            AssertCanWrite();
            _value = value;
        }
    }

    /// <summary>
    /// Gets the activator.
    /// </summary>
    public Activator<T> Activator => _activator;

    /// <summary>
    /// Removes the loaded value.
    /// </summary>
    /// <exception cref="System.InvalidOperationException" />
    public void Reset()
    {
        base.AssertCanWrite();
        _value.TryDispose();
        _value = null;
    }

    /// <summary>
    /// Gets the default value.
    /// </summary>
    public T DefaultValue
    {
        get
        {
            object dv = _defaultValue;
            
            if (dv == null)
                _defaultValue = dv = _activator();
            
            return (T)dv;                
        }
    }

    public static implicit operator T(Deferrable<T> deferrable)
    {
        return deferrable.Value;
    }

    public static implicit operator Deferrable<T>(Activator<T> activator)
    {
        return new Deferrable<T>(activator);
    }

    public static implicit operator Deferrable<T>(Func<T> activator)
    {
        return new Deferrable<T>(new Activator<T>(activator));
    }    
}