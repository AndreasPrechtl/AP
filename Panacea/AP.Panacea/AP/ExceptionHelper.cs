using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AP.Reflection;
using AP.Linq;
using System.Dynamic;
using System.Reflection;

namespace AP
{
    /// <summary>
    /// Used for generic exception handling
    /// </summary>
    public abstract class ExceptionHelper : StaticType
    {
        /// <summary>
        /// Throws an ArgumentNullException when the value is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="argumentName"></param>
        public static void AssertNotNull<T>(T value, string argumentName)
        {
            if (value.IsNull())
                throw new ArgumentNullException(argumentName);
        }

        /// <summary>
        /// Throws an ArgumentNullException when the value is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        public static void AssertNotNull<T>(Expression<Func<T>> argument)
        {
            ConstantExpression ce = null;
            MemberExpression me = null;
            ValidateArgumentExpression<T>(argument, out me, out ce);

            // ce.FullName holds the scope rather than the actual value - to get it - use reflection on the member ... this blows.
            AssertNotNull<T>((T)((FieldInfo)me.Member).GetValue(ce.Value), me.Member.Name);
        }
        
        /// <summary>
        /// Validates the expression tree used to get the argument in question
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="memberExpression"></param>
        /// <param name="constantExpression"></param>
        private static void ValidateArgumentExpression<T>(Expression<Func<T>> argument, out MemberExpression memberExpression, out ConstantExpression constantExpression)
        {
            memberExpression = argument.Body as MemberExpression;

            if (memberExpression == null)
                throw new InvalidOperationException("Body must be a MemberExpression");

            if (memberExpression.Member.MemberType != MemberTypes.Field)
                throw new InvalidOperationException("MemberType must be Field");

            constantExpression = memberExpression.Expression as ConstantExpression;

            if (constantExpression == null)
                throw new InvalidOperationException("Expression must be a ConstantExpression");
        }

        public static void ThrowArgumentException<T>(Expression<Func<T>> argument, string message = null, Exception innerException = null)
        {
            ThrowArgumentException<ArgumentException, T>(argument, message, innerException);
        }

        public static void ThrowArgumentNullException<T>(Expression<Func<T>> argument, string message = null, Exception innerException = null)
        {
            ThrowArgumentException<ArgumentNullException, T>(argument, message, innerException);
        }
                
        public static void ThrowArgumentOutOfRangeException<T>(Expression<Func<T>> argument, string message = null, Exception innerException = null)
        {
            ThrowArgumentException<ArgumentOutOfRangeException, T>(argument, message, innerException);
        }

        /// <summary>
        /// Throws any ArgumentException
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        private static void ThrowArgumentException<TException, T>(Expression<Func<T>> argument, string message = null, Exception innerException = null)
            where TException : ArgumentException
        {
            MemberExpression me;
            ConstantExpression ce;

            ValidateArgumentExpression<T>(argument, out me, out ce);        
            
            if (message.IsNullOrWhiteSpace())
                message = me.Member.Name;
            else
                message = me.Member.Name + ": " + message;

            throw New.Exception<TException>(message, innerException);
        }

        public static void ThrowOnInvalidCastException<TFrom, TTo>()
        {
            ThrowOnInvalidCastException(typeof(TFrom), typeof(TTo));
        }
           
        public static void ThrowOnInvalidCastException(Type from, Type to)
        {
            // if that doesn't sound weird - it is working that way - otherwise you'd have to use to == from || to.IsSubclassOf(from)
            if (!from.IsAssignableFrom(to))
                throw new InvalidCastException("Cannot cast {0} to {1}".Format(from, to));
        }
    }
}
