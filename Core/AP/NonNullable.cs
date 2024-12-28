using System.Runtime;

namespace System;

/// <summary>
/// Represents an object whose underlying type is a class type that can not be null.
/// </summary>
/// <typeparam name="T">The underlying class type of the <see cref="System.NonNullable{T}"/> generic type.</typeparam>
/// <remarks>Do NOT use the parameterless constructor, it would result in an invalid state.</remarks>
[Serializable]
public struct NonNullable<T>
    where T : class
{
    private readonly T _inner;

    ///// <summary>
    ///// Do not use this ctor. It is only meant for being used by array initializers or default values.
    ///// </summary>
    //public NonNullable()
    //    : base()
    //{ }

    /// <summary>
    /// Initializes a new instance of the <see cref="System.NonNullable{T}"/> structure to the specified value.        
    /// </summary>
    /// <param name="value">A class type.</param>
    /// <exception cref="System.ArgumentNullException">Throws a System.ArgumentNullException if the value is null.</exception>        
    /// <remarks>Do NOT use the parameterless constructor, it would result in an invalid state.</remarks>
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]        
    public NonNullable(T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        _inner = value;
    }
    
    /// <summary>
    /// Gets the value of the current <see cref="System.NonNullable{T}"/> value.
    /// </summary>
    /// <returns>The value of the current <see cref="System.NonNullable{T}"/> object.</returns>
    public T Value 
    { 
        get 
        { 
            T v = _inner;
            if (v == null)
                throw new InvalidOperationException("Value has not been assigned. Do not use the default contstructor, for this will result in an invalid state.");
            
            return v; 
        } 
    }

    public static explicit operator T(NonNullable<T> value)
    {
        return value.Value;
    }

    public static implicit operator NonNullable<T>(T value)
    {
        return new NonNullable<T>(value);
    }

    /// <summary>
    /// Returns the text representation of the value of the current <see cref="System.NonNullable{T}"/> object.
    /// </summary>
    /// <returns>The text representation of the value of the current <see cref="System.NonNullable{T}"/> object.</returns>
    public override string ToString()
    {
        T v = _inner;
        if (v == null)
            return base.ToString();

        return v.ToString();
    }

    /// <summary>
    /// Indicates whether the current <see cref="System.NonNullable{T}"/> object is equal to a specified object.
    /// </summary>
    /// <param name="other">An object.</param>
    /// <returns>true if the other parameter is equal to the current <see cref="System.NonNullable{T}"/> object; otherwise, false.</returns>
    public override bool Equals(object other)
    {
        T v = _inner;
        if (v == null)
            return base.Equals(other);

        return v.Equals(other);
    }
            
    /// <summary>
    /// Retrieves the hash code of the object returned by the <see cref="System.NonNullable{T}.Value"/> property.
    /// </summary>
    /// <returns>The hash code of the object returned by the <see cref="System.NonNullable{T}.Value"/> property.</returns>
    public override int GetHashCode()
    {
        T v = _inner;
        if (v == null)
            return base.GetHashCode();

        return v.GetHashCode();
    }
}
