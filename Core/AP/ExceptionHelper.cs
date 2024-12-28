using System;
using System.Linq.Expressions;
using System.Reflection;

namespace AP;

/// <summary>
/// Used for generic exception handling
/// </summary>
public abstract class ExceptionHelper : StaticType
{
    /// <summary>
    /// Validates the expression tree used to get the argument in question
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="argument"></param>
    /// <param name="memberExpression"></param>
    /// <param name="constantExpression"></param>
    private static void ValidateArgumentExpression<T>(Expression<Func<T>> argument, out MemberExpression memberExpression, out ConstantExpression constantExpression)
    {
        ArgumentNullException.ThrowIfNull(argument);

        if (argument.Body is not MemberExpression member)
            throw new InvalidOperationException("Body must be a MemberExpression");

        if (member.Member.MemberType != MemberTypes.Field)
            throw new InvalidOperationException("MemberType must be Field");

        if (member.Expression is not ConstantExpression constant)
            throw new InvalidOperationException("Expression must be a ConstantExpression");

        memberExpression = member;
        constantExpression = constant;
    }

    public static void ThrowArgumentException<T>(Expression<Func<T>> argument, string? message = null, Exception? innerException = null) => ThrowArgumentException<ArgumentException, T>(argument, message, innerException);

    public static void ThrowArgumentNullException<T>(Expression<Func<T>> argument, string? message = null, Exception? innerException = null) => ThrowArgumentException<ArgumentNullException, T>(argument, message, innerException);

    public static void ThrowArgumentOutOfRangeException<T>(Expression<Func<T>> argument, string? message = null, Exception? innerException = null) => ThrowArgumentException<ArgumentOutOfRangeException, T>(argument, message, innerException);

    /// <summary>
    /// Throws any ArgumentException
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="argument"></param>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    private static void ThrowArgumentException<TException, T>(Expression<Func<T>> argument, string? message = null, Exception? innerException = null)
        where TException : ArgumentException
    {

        ValidateArgumentExpression<T>(argument, out MemberExpression me, out ConstantExpression ce);

        if (message.IsNullOrWhiteSpace())
            message = me.Member.Name;
        else
            message = me.Member.Name + ": " + message;

        throw New.Exception<TException>(message, innerException);
    }

    public static void ThrowOnInvalidCastException<TFrom, TTo>() => ThrowOnInvalidCastException(typeof(TFrom), typeof(TTo));

    public static void ThrowOnInvalidCastException(Type from, Type to)
    {
        // if that doesn't sound weird - it is working that way - otherwise you'd have to use to == from || to.IsSubclassOf(from)
        if (!from.IsAssignableFrom(to))
            throw new InvalidCastException("Cannot cast {0} to {1}".Format(from, to));
    }
}
