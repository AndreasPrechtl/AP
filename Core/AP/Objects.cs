using System;
using System.Collections.Generic;
using AP.Collections.Specialized;
using AP.Reflection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AP;

/// <summary>
/// Contains helper methods for working with objects
/// </summary>
public static class Objects
{
    /// <summary>
    /// Wraps an object into an IDisposable context.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <param name="value">The value.</param>
    /// <returns>Returns a disposable object.</returns>
    [MethodImpl((MethodImplOptions)256)]
    public static AP.IContextDependentDisposable AsDisposable<T>(T value, object contextKey = null) where T : notnull => new AP.DisposableWrapper<T>(value, contextKey);

    [MethodImpl((MethodImplOptions)256)]
    public static ValueTask<AP.IContextDependentDisposable> AsAsyncDisposable<T>(T value) where T : notnull => ValueTask.FromResult((AP.IContextDependentDisposable)new AP.DisposableWrapper<T>(value));

    /// <summary>
    /// Disposes an object.
    /// </summary>
    /// <param name="value">The object.</param>
    public static async ValueTask DisposeAsync(this object value, object? contextKey = null)
    {
        if (contextKey is not null && value is AP.IContextDependentDisposable contextDependentDisposable)
        {
            await contextDependentDisposable.DisposeAsync(contextKey);
            return;
        }

        if (value is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync();
            return;
        }

        if (value is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    /// <summary>
    /// Disposes an object.
    /// </summary>
    /// <param name="value">The object.</param>
    public static void Dispose(this object value, object? contextKey = null)
    {
        if (contextKey is not null && value is AP.IContextDependentDisposable contextDependentDisposable)
        {
            contextDependentDisposable.Dispose(contextKey);
            return;
        }

        if (value is IDisposable disposable)
        {
            disposable.Dispose();            
        }
    }

    /// <summary>
    /// Tries to dispose an object.
    /// </summary>
    /// <param name="value">The object.</param>
    /// <returns></returns>
    public static async ValueTask<bool> TryDisposeAsync(this object value, object? contextKey = null)
    {
        try
        {
            if (contextKey is not null && value is AP.IContextDependentDisposable contextDependentDisposable)
            {
                await contextDependentDisposable.DisposeAsync(contextKey);
                return true;
            }

            if (value is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
                return true;
            }
        }
        catch { }

        return false;
    }

    /// <summary>
    /// Tries to dispose an object.
    /// </summary>
    /// <param name="value">The object.</param>
    /// <returns></returns>
    public static bool TryDispose(this object value, object? contextKey = null)
    {
        try
        {
            if (contextKey is not null && value is AP.IContextDependentDisposable contextDependentDisposable)
            {
                contextDependentDisposable.Dispose(contextKey);
                return true;
            }

            if (value is IDisposable disposable)
            {
                disposable.Dispose();
                return true;
            }
        }
        catch { }
        
        return false;
    }

    [MethodImpl((MethodImplOptions)256)]
    public static new bool ReferenceEquals(this object? value, object? other) => object.ReferenceEquals(value, other);

    [MethodImpl((MethodImplOptions)256)]
    public static bool IsNull(this object? value) => object.ReferenceEquals(value, null);

    [MethodImpl((MethodImplOptions)256)]
    public static bool IsDefault<T>(this T? value) => object.Equals(value, default(T));

    /// <summary>
    /// Merges two objects using Reflection and fields
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    /// <returns></returns>
    public static TInto Merge<TFrom, TInto>(TFrom from, TInto into, bool overwrite = false)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(into);

        foreach (var fromField in from.GetType().GetFields(Reflect.AllFields))
        {
            foreach (var intoField in into.GetType().GetFields(Reflect.AllFields))
            {
                if (fromField.Name == intoField.Name && !intoField.IsInitOnly) // && (overwrite || intoField.GetValue(into).IsNullOrDefault())))
                {
                    try
                    {
                        intoField.SetValue(into, fromField.GetValue(from));                            
                    }
                    catch
                    {
                        try
                        {
                            var f = fromField.GetValue(from);                            
                            var x = f != null ? TypeDescriptor.GetConverter(f)?.ConvertTo(f, intoField.FieldType) : null;

                            intoField.SetValue(into, x);
                        }
                        catch { }
                    }
                }
            }
        }
        return into;
    }

    /// <summary>
    /// Returns a NameValueDictionary,
    /// if an enumerable string, object was provided it will be transformed into a NameValueDictionary
    /// if a simple object was provided it will return a NameValueDictionary based on that object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static NameValueDictionary<object> ToDictionary(this object obj) => ToDictionary<object>(obj);

    /// <summary>
    /// Returns a NameValueDictionary,
    /// if an enumerable string, object was provided it will be transformed into a NameValueDictionary
    /// if a simple object was provided it will return a NameValueDictionary based on that object
    /// properties are filtered upon the TValue type;
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static NameValueDictionary<TValue> ToDictionary<TValue>(this object obj)
    {
        if (obj.IsDefault())
            return [];

        if (obj is IEnumerable<KeyValuePair<string, TValue>> enumerable)
            return new NameValueDictionary<TValue>(enumerable);

        return NameValueDictionary<TValue>.FromObject(obj);
    }

    /// <summary>
    /// Returns a ReadOnlyDictionary
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static ReadOnlyNameValueDictionary<object> ToReadOnlyDictionary(this object obj) => ToReadOnlyDictionary<object>(obj);

    /// <summary>
    /// Returns a ReadOnlyDictionary
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static ReadOnlyNameValueDictionary<TValue> ToReadOnlyDictionary<TValue>(this object obj)
    {
        if (obj.IsDefault())
            return ReadOnlyNameValueDictionary<TValue>.Empty;

        if (obj is IEnumerable<KeyValuePair<string, TValue>> enumerable)
            return new ReadOnlyNameValueDictionary<TValue>(enumerable);

        return ReadOnlyNameValueDictionary<TValue>.FromObject(obj);
    }
}

