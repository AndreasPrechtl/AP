﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AP.Collections.Specialized;
using AP.Reflection;
using System.Reflection;
using AP.ComponentModel.Conversion;
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
    public static AP.IDisposable AsDisposable<T>(T value) where T : notnull => new AP.DisposableWrapper<T>(value);

    [MethodImpl((MethodImplOptions)256)]
    public static ValueTask<AP.IDisposable> AsAsyncDisposable<T>(T value) where T : notnull => ValueTask.FromResult((AP.IDisposable)new AP.DisposableWrapper<T>(value));

    /// <summary>
    /// Short method for System.Activator.CreateInstance&lt;T&gt;
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <param name="args">The constructor arguments.</param>
    /// <returns>Returns a the instance.</returns>
    [MethodImpl((MethodImplOptions)256)]
    public static T New<T>(params object[] args)
    {
        if (args == null || args.Length == 0)
            return System.Activator.CreateInstance<T>();

        return (T)System.Activator.CreateInstance(typeof(T), args)!;
    }

    /// <summary>
    /// Short method for System.Activator.CreateInstance&lt;T&gt;
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    [MethodImpl((MethodImplOptions)256)]
    public static object New(Type type, params object[] args) => System.Activator.CreateInstance(type, args)!;


    /// <summary>
    /// Short method that returns an uninitialized object - use with caution as no constructors are used - might break some lazy/deferred loading scenarios
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl((MethodImplOptions)256)]
    public static T Uninitialized<T>() => (T)Uninitialized(typeof(T));

    /// <summary>
    /// Short method that returns an uninitialized object - use with caution as no constructors are used - might break some lazy/deferred loading scenarios
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    [MethodImpl((MethodImplOptions)256)]
    public static object Uninitialized(Type type) => FormatterServices.GetUninitializedObject(type);

    /// <summary>
    /// Creates either a new or uninitialized object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T NewOrUninitialized<T>()
    {
        T instance;
        Type type = typeof(T);

        if (typeof(T).IsAnonymous())
            instance = (T)Objects.Uninitialized(type);
        else
        {
            try { instance = Objects.New<T>(); }
            catch { instance = (T)Objects.Uninitialized(type); }
        }
        
        return instance;
    }

    /// <summary>
    /// Creates either a new or uninitialized object.
    /// </summary>
    /// <param name="type">The type of the object to create.</param>
    /// <returns>The object.</returns>
    public static object NewOrUnintialized(Type type)
    {
        object instance;

        if (type.IsAnonymous())
            instance = Uninitialized(type);
        else
        {
            try { instance = Objects.New(type); }
            catch { instance = Objects.Uninitialized(type); }
        }

        return instance;
    }

    /// <summary>
    /// Disposes an object.
    /// </summary>
    /// <param name="value">The object.</param>
    public static void Dispose(this object value)
    {
        if (value is System.IDisposable disposable)
            disposable.Dispose();
    }
    
    /// <summary>
    /// Tries to dispose an object.
    /// </summary>
    /// <param name="value">The object.</param>
    /// <returns></returns>
    public static bool TryDispose(this object value)
    {
        if (value is System.IDisposable disposable)
        {
            try
            {
                disposable.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        return true;
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
                if ((fromField.Name == intoField.Name && !intoField.IsInitOnly)) // && (overwrite || intoField.GetValue(into).IsNullOrDefault())))
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
                            var x = TypeDescriptor.GetConverter(f).ConvertTo(f, intoField.FieldType);

                            intoField.SetValue(into, x);
                        }
                        catch { }
                    }
                }
            }
        }
        return into;
    }
     // move to conversion or leave it here ?
    public static TOutput Convert<TInput, TOutput>(this TInput input)
    {
        var c = Converters.GetConverter<TInput, TOutput>();

        if (c != null)
            return c.Convert(input);
        else
            return (TOutput)TypeDescriptor.GetConverter(input!).ConvertTo(input, typeof(TOutput))!;
        //else
        //    return Objects.Merge(input, New.Object<TOutput>(), true);
    }

    //public static object Convert(this object input, Type outputType)
    //{
    //    var c = Converters.GetConverter(input.GetType(), outputType);

    //    if (c != null)
    //        return c.Convert(input);
    //    else
    //        return Objects.Merge(input, Activator.CreateInstance(outputType), true);
    //}

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

