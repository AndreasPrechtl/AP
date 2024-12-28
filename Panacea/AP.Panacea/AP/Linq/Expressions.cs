using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.Reflection;
using System.Linq.Expressions;

namespace AP.Linq
{
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

        public static MethodCallExpression StaticCall(Type type, string methodName, Type[] typeArguments, params Expression[] arguments) { return Expression.Call(type, methodName, typeArguments, arguments); }
        public static MethodCallExpression StaticCall(MethodInfo method, IEnumerable<Expression> arguments) { return Expression.Call(method, arguments); }
        public static MethodCallExpression StaticCall(MethodInfo method, params Expression[] arguments) { return Expression.Call(method, arguments); }
        public static MethodCallExpression StaticCall(MethodInfo method, Expression arg0) { return Expression.Call(method, arg0); }
        public static MethodCallExpression StaticCall(MethodInfo method, Expression arg0, Expression arg1) { return Expression.Call(method, arg0, arg1); }
        public static MethodCallExpression StaticCall(MethodInfo method, Expression arg0, Expression arg1, Expression arg2) { return Expression.Call(method, arg0, arg1, arg2); }
        public static MethodCallExpression StaticCall(MethodInfo method, Expression arg0, Expression arg1, Expression arg2, Expression arg3) { return Expression.Call(method, arg0, arg1, arg2, arg3); }
        public static MethodCallExpression StaticCall(MethodInfo method, Expression arg0, Expression arg1, Expression arg2, Expression arg3, Expression arg4) { return Expression.Call(method, arg0, arg1, arg2, arg3, arg4); }

        public static MethodCallExpression InstanceCall(Expression instance, string methodName, Type[] typeArguments, params Expression[] arguments) { return Expression.Call(instance, methodName, typeArguments, arguments); }
        public static MethodCallExpression InstanceCall(Expression instance, MethodInfo method, params Expression[] arguments) { return Expression.Call(instance, method, arguments); }
        public static MethodCallExpression InstanceCall(Expression instance, MethodInfo method, IEnumerable<Expression> arguments) { return Expression.Call(instance, method, arguments); }
        public static MethodCallExpression InstanceCall(Expression instance, MethodInfo method) { return Expression.Call(instance, method); }
        public static MethodCallExpression InstanceCall(Expression instance, MethodInfo method, Expression arg0, Expression arg1) { return Expression.Call(instance, method, arg0, arg1); }
        public static MethodCallExpression InstanceCall(Expression instance, MethodInfo method, Expression arg0, Expression arg1, Expression arg2) { return Expression.Call(instance, method, arg0, arg1, arg2); }
        
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
            Delegate m = func as Delegate;

            if (m == null)
                throw new ArgumentException("func must be of type System.Delegate");

            ParameterInfo[] parameters = m.Method.GetParameters();
            ParameterExpression[] pes = new ParameterExpression[parameters.Length];

            int i = 0;
            foreach (ParameterInfo p in parameters)
            {
                pes[i] = Expressions.Parameter(p.ParameterType, p.Name);
                i++;
            }

            MethodCallExpression c = null;
            if (m.Target == null)
                c = Expressions.StaticCall(m.Method, pes);
            else
                c = Expressions.InstanceCall(Expression.Constant(m.Target), m.Method, pes);

            return Expression.Lambda<TDelegate>(c, pes);
        }
    }
}
