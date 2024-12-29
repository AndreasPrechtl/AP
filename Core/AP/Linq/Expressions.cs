using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;

namespace AP.Linq;

/// <summary>
/// Expression shortcut
/// </summary>
public abstract class Expressions : System.Linq.Expressions.Expression
{
    protected Expressions()
        : base()
    {
        StaticType.ThrowTypeInitializationException(this.GetType());
    }

    public static MethodCallExpression StaticCall(Type type, string methodName, Type[] typeArguments, params Expression[] arguments) => Expression.Call(type, methodName, typeArguments, arguments);
    public static MethodCallExpression StaticCall(MethodInfo method, params IEnumerable<Expression> arguments) => Expression.Call(method, arguments);
    
    public static MethodCallExpression InstanceCall(Expression instance, string methodName, Type[] typeArguments, params IEnumerable<Expression> arguments) => Expression.Call(instance, methodName, typeArguments, [..arguments]);
    public static MethodCallExpression InstanceCall(Expression instance, MethodInfo method, params IEnumerable<Expression> arguments) => Expression.Call(instance, method, arguments);    

    /// <summary>
    /// Converts a delegate to an Expression
    /// Note: Might not be working with Linq to Entities/Sql
    /// </summary>
    /// <typeparam name="TDelegate"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Expression<TDelegate> FromDelegate<TDelegate>(TDelegate func)
        where TDelegate : class
    {
        if (func is not Delegate m)
            throw new ArgumentException("func must be of type System.Delegate");

        ParameterInfo[] parameters = m.Method.GetParameters();
        ParameterExpression[] pes = new ParameterExpression[parameters.Length];

        int i = 0;
        foreach (ParameterInfo p in parameters)
        {
            pes[i] = Expressions.Parameter(p.ParameterType, p.Name);
            i++;
        }

        MethodCallExpression? c = null;
        if (m.Target == null)
            c = Expressions.StaticCall(m.Method, pes);
        else
            c = Expressions.InstanceCall(Expression.Constant(m.Target), m.Method, pes);

        if (c is null)
            throw new InvalidOperationException("not a MethodCallExpression");

        return Expression.Lambda<TDelegate>(c, pes);
    }
}
