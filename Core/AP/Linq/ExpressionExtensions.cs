using System;
using System.Linq.Expressions;

namespace AP.Linq;

public static class ExpressionExtensions
{
    /// <summary>
    /// Casts an Expression<TDelegate> to one that's using a different signature
    /// </summary>
    /// <typeparam name="TDelegateOut"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static Expression<TDelegateOut> Cast<TDelegateOut>(this LambdaExpression expression)
        where TDelegateOut : class => Expressions.Lambda<TDelegateOut>(expression.Body, expression.Parameters);

    /// <summary>
    /// Tries to cast an Expression<TDelegate> to one that's using a different signature
    /// </summary>
    /// <typeparam name="TDelegateOut"></typeparam>
    /// <param name="expression"></param>
    /// <param name="output"></param>
    /// <returns></returns>
    public static bool TryCast<TDelegateOut>(this LambdaExpression expression, out Expression<TDelegateOut>? output)
       where TDelegateOut : class
    {
        if (expression == null)
        {
            output = null;
            return false;
        }
        try
        {
            output = Expression.Lambda<TDelegateOut>(expression.Body, expression.Parameters);
            return true;
        }
        catch (Exception)
        {
            output = null;
            return false;
        }
    }     
    
    /// <summary>
    /// Evaluates an expression tree
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static object Evaluate(this Expression expression, params object[] args)
    {
        switch (expression.NodeType)
        {
            case ExpressionType.Constant:
                return ((ConstantExpression)expression).Value;
            case ExpressionType.Lambda:
                return ((LambdaExpression)expression).Compile().DynamicInvoke(args);

            default:
                return Expression.Lambda(expression).Compile().DynamicInvoke(args);
        }
    }

    /// <summary>
    /// Evaluates an expression tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static T Evaluate<T>(this Expression expression, params object[] args) => (T)Evaluate(expression, args);
}
