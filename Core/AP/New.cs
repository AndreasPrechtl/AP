using AP.Linq;
using AP.Reflection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AP;

/// <summary>
/// Provides a set of shorthand methods for creating objects - mostly collection types.
/// </summary>
/// <remarks>ToDo: add any subclass to source generator and merge everything below to this class.</remarks>
public abstract class New : StaticType
{
    /// <summary>
    /// Short method for System.Activator.CreateInstance&lt;T&gt;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args"></param>
    /// <returns></returns>
    [MethodImpl((MethodImplOptions)256)]
    public static T Instance<T>(params IEnumerable<object?> args)
        where T : notnull
        => args.IsEmpty() ? System.Activator.CreateInstance<T>() : (T)System.Activator.CreateInstance(typeof(T), args)!;

    /// <summary>
    /// Short method that returns an uninitialized object - use with caution as no constructors are used - might break some lazy/deferred loading scenarios
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl((MethodImplOptions)256)]
    public static T Uninitialized<T>()
        where T : notnull 
        => (T)Uninitialized(typeof(T));

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
        where T : notnull
        => (T)NewOrUninitialized(typeof(T));

    /// <summary>
    /// Creates either a new or uninitialized object.
    /// </summary>
    /// <param name="type">The type of the object to create.</param>
    /// <returns>The object.</returns>
    public static object NewOrUninitialized(Type type)
    {
        object instance;

        if (type.IsAnonymous())
            instance = Uninitialized(type);
        else
        {
            try { instance = Instance(type); }
            catch { instance = Uninitialized(type); }
        }

        return instance;
    }

    /// <summary>
    /// Short method for System.Activator.CreateInstance
    /// Warning, if there are default values in the constructor it WILL throw a MissingMethodException (setting the values manually helps)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    [MethodImpl((MethodImplOptions)256)]
    public static object Instance(Type type, params IEnumerable<object> args) => New.Instance(type, args);

    /// <summary>
    /// Short method for System.Activator.CreateInstance
    /// </summary>
    /// <param name="type"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    [MethodImpl((MethodImplOptions)256)]
    public static object Object(Type type, params IEnumerable<object> args) => New.Instance(type, args);

    /// <summary>
    /// Creates a new System.Guid
    /// </summary>
    /// <returns>The Guid</returns>
    [MethodImpl((MethodImplOptions)256)]
    public static Guid Guid() => System.Guid.NewGuid();

    [MethodImpl((MethodImplOptions)256)]
    public static AP.Collections.List<T> List<T>(params IEnumerable<T> items) => new(items);

    [MethodImpl((MethodImplOptions)256)]
    public static AP.Collections.Set<T> Set<T>(params IEnumerable<T> items) => new(items);

    [MethodImpl((MethodImplOptions)256)]
    public static AP.Collections.Queue<T> Queue<T>(params IEnumerable<T> items) => new(items);

    [MethodImpl((MethodImplOptions)256)]
    public static AP.Collections.Stack<T> Stack<T>(params IEnumerable<T> items) => new(items);

    [MethodImpl((MethodImplOptions)256)]
    public static AP.Collections.Dictionary<TKey, TValue> Dictionary<TKey, TValue>(params KeyValuePair<TKey, TValue>[] dictionary)
        where TKey : notnull
        => new(dictionary);
}
