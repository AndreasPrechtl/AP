using System;
using AP.Reflection;
using System.Runtime.CompilerServices;

namespace AP;

public delegate bool Change<T>(out T obj);    
public delegate T Activator<out T>();

/// <summary>
/// A delegate that's used to invoke a static member
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <returns></returns>
public delegate TResult Invoke<TResult>();

/// <summary>
/// A delegate that's used to invoke an instance member
/// </summary>
/// <typeparam name="TInstance"></typeparam>
/// <typeparam name="TResult"></typeparam>
/// <param name="instance"></param>
/// <returns></returns>
public delegate TResult Invoke<TInstance, TResult>(TInstance instance);

/// <summary>
/// Delegate extensions
/// </summary>
public static class Delegates
{
    /// <summary>
    /// Compares the MethodBody's byte array for equality
    /// </summary>
    /// <param name="delegate"></param>
    /// <param name="other"></param>
    /// <returns></returns>                
    public static bool MethodBodyEqual(this Delegate method, Delegate other)
    {
        if (method == other)
            return true;

        return method.Method.MethodBodyEqual(other.Method);
    }

    /// <summary>
    /// Tries to invoke a constructor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="activator"></param>
    /// <param name="instance"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T>(this Activator<T> activator, out T instance) => TryInvokeFuncInternal(activator, (Func<T>)(object)activator, out instance);

    /// <summary>
    /// Tries to invoke a static method
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="invoker"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<TInstance, TResult>(this Invoke<TResult> invoker, out TResult result) => TryInvokeFuncInternal(invoker, (Func<TResult>)(object)invoker, out result);

    /// <summary>
    /// Tries to invoke an instance method
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="invoker"></param>
    /// <param name="instance"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<TInstance, TResult>(this Invoke<TInstance, TResult> invoker, TInstance instance, out TResult result) => TryInvokeFuncInternal(invoker, () => invoker(instance), out result);

    #region funcs

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<TResult>(this Func<TResult> func, out TResult result) => TryInvokeFuncInternal(func, func, out result);

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, TResult>
        (this Func<T1, TResult> func, T1 arg1, out TResult result) => TryInvokeFuncInternal(func, () => func(arg1), out result);

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, TResult>
        (this Func<T1, T2, TResult> func, T1 arg1, T2 arg2, out TResult result) => TryInvokeFuncInternal(func, () => func(arg1, arg2), out result);

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, TResult>
        (this Func<T1, T2, T3, TResult> func, T1 arg1, T2 arg2, T3 arg3, out TResult result) => TryInvokeFuncInternal(func, () => func(arg1, arg2, arg3), out result);

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, TResult>
        (this Func<T1, T2, T3, T4, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, out TResult result) => TryInvokeFuncInternal(func, () => func(arg1, arg2, arg3, arg4), out result);

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, TResult>
        (this Func<T1, T2, T3, T4, T5, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out TResult result) => TryInvokeFuncInternal(func, () => func(arg1, arg2, arg3, arg4, arg5), out result);

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, TResult>
        (this Func<T1, T2, T3, T4, T5, T6, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, out TResult result) => TryInvokeFuncInternal(func, () => func(arg1, arg2, arg3, arg4, arg5, arg6), out result);

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, TResult>
        (this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, out TResult result) => TryInvokeFuncInternal(func, () => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7), out result);

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, out TResult result) => TryInvokeFuncInternal(func, () => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), out result);

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, out TResult result) => TryInvokeFuncInternal(func, () => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), out result);

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, out TResult result) => TryInvokeFuncInternal(func, () => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), out result);

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, out TResult result) => TryInvokeFuncInternal(func, () => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), out result);

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, out TResult result) => TryInvokeFuncInternal(func, () => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), out result);

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, out TResult result) => TryInvokeFuncInternal(func, () => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), out result);

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, out TResult result) => TryInvokeFuncInternal(func, () => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14), out result);

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, out TResult result) => TryInvokeFuncInternal(func, () => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15), out result);

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, out TResult result) => TryInvokeFuncInternal(func, () => func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16), out result);

    #endregion

    #region actions

    /// <summary>
    /// Tries to invoke an action.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action"></param>
    /// <returns></returns>
    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke(this Action action) =>
        // use the same action as a wrapper - would only generate the same result anyway.
        TryInvokeActionInternal(action, action);

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1>
        (this Action<T1> action, T1 arg1) => TryInvokeActionInternal(action, () => action(arg1));

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2>
        (this Action<T1, T2> action, T1 arg1, T2 arg2) => TryInvokeActionInternal(action, () => action(arg1, arg2));

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3>
        (this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3) => TryInvokeActionInternal(action, () => action(arg1, arg2, arg3));

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4>
        (this Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => TryInvokeActionInternal(action, () => action(arg1, arg2, arg3, arg4));

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5>
        (this Action<T1, T2, T3, T4, T5> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => TryInvokeActionInternal(action, () => action(arg1, arg2, arg3, arg4, arg5));

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6>
        (this Action<T1, T2, T3, T4, T5, T6> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => TryInvokeActionInternal(action, () => action(arg1, arg2, arg3, arg4, arg5, arg6));

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7>
        (this Action<T1, T2, T3, T4, T5, T6, T7> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => TryInvokeActionInternal(action, () => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7));

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8>
        (this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) => TryInvokeActionInternal(action, () => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        (this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) => TryInvokeActionInternal(action, () => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        (this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) => TryInvokeActionInternal(action, () => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        (this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) => TryInvokeActionInternal(action, () => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        (this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) => TryInvokeActionInternal(action, () => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        (this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) => TryInvokeActionInternal(action, () => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13));

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        (this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) => TryInvokeActionInternal(action, () => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14));

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        (this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) => TryInvokeActionInternal(action, () => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15));

    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        (this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) => TryInvokeActionInternal(action, () => action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16));

    #endregion

    /// <summary>
    /// Tries to invoke a delegate with a returning a result.
    /// </summary>
    /// <param name="method"></param>
    /// <param name="output"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke(this Delegate method, out object? output, params object[] args)
    {
        if (method != null)
        {
            try
            {
                output = method.DynamicInvoke(args);
                return true;
            }
            catch { }
        }
        output = null;
        
        return false;
    }

    /// <summary>
    /// Tries to invoke a delegate without returning a result
    /// </summary>
    /// <param name="method"></param>
    /// <param name="output"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    [MethodImpl((MethodImplOptions)256)]
    public static bool TryInvoke(this Delegate method, params object[] args)
    {
        if (method != null)
        {
            try
            {
                method.DynamicInvoke(args);
                return true;
            }
            catch { }
        }

        return false;
    }

    [MethodImpl((MethodImplOptions)256)]
    private static bool TryInvokeActionInternal(this Delegate method, Action wrapper)
    {
        if (method != null)
        {
            try
            {
                wrapper();
                return true;
            }
            catch { }
        }            
        return false;
    }

    [MethodImpl((MethodImplOptions)256)]
    private static bool TryInvokeFuncInternal<TResult>(this Delegate method, Func<TResult> wrapper, out TResult result)            
    {
        if (method != null)
        {
            try
            {
                result = wrapper();
                return true;
            }
            catch { }
        }
        result = default;

        return false;
    }

    ///// <summary>
    ///// Tries to invoke a delegate - removed - possibly redundant code 
    ///// </summary>
    ///// <param name="method"></param>
    ///// <param name="output"></param>
    ///// <param name="args"></param>
    ///// <returns></returns>
    //[MethodImpl((MethodImplOptions)256)]
    //public static bool TryInvoke(this MulticastDelegate method, out object output, params object[] args)
    //{
    //    if (method != null)
    //    {
    //        output = method.DynamicInvoke(args);
    //        return true;
    //    }
    //    else
    //    {
    //        output = null;
    //        return false;
    //    }
    //}
}
