using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using AP.Linq;
using AP.Reflection;


namespace AP.Routing
{
    internal sealed class ExpressionComparerInternal<TParameters>
    {
        private TParameters _parameters;
        private ParameterExpression _parameterExpression;

        public TParameters Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Compares the route's and the routing context's expressions.
        /// </summary>
        /// <param name="routeExpression">The route expression.</param>
        /// <param name="contextExpression">The context expression.</param>
        /// <returns>Returns true if both expressions are invoking the same targets, using the same parameters.</returns>
        public bool Compare(Expression<ResultCreator<TParameters>> routeExpression, Expression<ResultCreator> contextExpression)
        {
            _parameters = New.OrUninitialized<TParameters>();
            _parameterExpression = routeExpression.Parameters[0];

            return this.Visit(routeExpression.Body, contextExpression.Body);
        }

        /// <summary>
        /// Removes a possible type cast.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The unpacked expression.</returns>
        private static Expression RemoveTypeCast(Expression expression)
        {
            ExpressionType nodeType = expression.NodeType;

            if (nodeType == ExpressionType.Convert || nodeType == ExpressionType.ConvertChecked)
                return ((UnaryExpression)expression).Operand;

            return expression;
        }

        private bool Visit(Expression routeExpression, Expression contextExpression)
        {
            routeExpression = RemoveTypeCast(routeExpression);
            contextExpression = RemoveTypeCast(contextExpression);

            ExpressionType nodeType = routeExpression.NodeType;

            if (nodeType == contextExpression.NodeType)
            {
                switch (nodeType)
                {
                    case ExpressionType.New:
                        return this.VisitNew((NewExpression)routeExpression, (NewExpression)contextExpression);
                    case ExpressionType.MemberInit:
                        return this.VisitMemberInit((MemberInitExpression)routeExpression, (MemberInitExpression)contextExpression);
                    case ExpressionType.Call:
                        return this.VisitCall((MethodCallExpression)routeExpression, (MethodCallExpression)contextExpression);
                    case ExpressionType.ListInit:
                        return this.VisitListInit((ListInitExpression)routeExpression, (ListInitExpression)contextExpression);
                    case ExpressionType.ArrayIndex:
                        return this.VisitArrayIndex((BinaryExpression)routeExpression, (BinaryExpression)contextExpression);
                    case ExpressionType.Constant:
                        return this.VisitConstant((ConstantExpression)routeExpression, (ConstantExpression)contextExpression);
                    default:
                        throw new Exception("Expression could not be used for routing - NodeType not supported.");
                }
            }
            
            // maybe that's a direct match
            return this.AnalyzeParameters(routeExpression, contextExpression);
        }

        /// <summary>
        /// Checks if expression1 contains indexer access for ResultCreatorParameters
        /// </summary>
        /// <param name="expression1"></param>
        /// <param name="expression2"></param>
        private bool AnalyzeParameters(Expression expression1, Expression expression2)
        {
            ExpressionType nodeType = expression1.NodeType;

            if (nodeType == expression2.NodeType)
            {
                return object.Equals(expression1.Evaluate(), expression2.Evaluate());
            }
            else if (nodeType == ExpressionType.Parameter)   // the special case: p => p
            {
                if (expression1 == _parameterExpression)
                {
                    // evaluate to object and when the thing actually is TParameters, use it - that way there's no problem with type casts or methods returning object instead of TParameter.
                    object result = expression2.Evaluate();

                    if (result is TParameters)
                    {
                        _parameters = (TParameters)expression2.Evaluate();
                        return true;
                    }
                }
            }
            else if (nodeType == ExpressionType.MemberAccess)    // the regular usage: p => p.Id = 123414
            {
                MemberExpression memberExpression1 = (MemberExpression)expression1;

                if (memberExpression1.Expression == _parameterExpression)
                {
                    MemberInfo member = memberExpression1.Member;

                    if (member.IsField())
                        ((FieldInfo)member).CreateSetterDelegate().DynamicInvoke(_parameters, expression2.Evaluate());
                    else if (member.IsProperty())
                        ((PropertyInfo)member).CreateSetterDelegate().DynamicInvoke(_parameters, expression2.Evaluate());
                    else
                        throw new ArgumentException(string.Format("Member {0} is not a field or property.", member.Name));

                    return true;
                }
            }
            return false;
        }

        private bool VisitArguments(System.Collections.ObjectModel.ReadOnlyCollection<Expression> arguments1, System.Collections.ObjectModel.ReadOnlyCollection<Expression> arguments2)
        {

            int count = arguments1.Count;

            for (int i = 0; i < count; ++i)
                if (!this.Visit(arguments1[i], arguments2[i]))
                    return false;

            return true;
        }

        private bool VisitNew(NewExpression newExpression1, NewExpression newExpression2)
        {
            // extract the parameters
            if (newExpression1.Constructor != newExpression2.Constructor)
                return false;

            return this.VisitArguments(newExpression1.Arguments, newExpression2.Arguments);
        }

        private bool VisitCall(MethodCallExpression methodCallExpression1, MethodCallExpression methodCallExpression2)
        {
            // argument malfunction - should throw an exception
            if (methodCallExpression1.Method != methodCallExpression2.Method)
                return false;

            // visit the contextual object and the arguments 
            return this.Visit(methodCallExpression1.Object, methodCallExpression2.Object) && this.VisitArguments(methodCallExpression1.Arguments, methodCallExpression2.Arguments);
        }

        private bool VisitArrayIndex(BinaryExpression binaryExpression1, BinaryExpression binaryExpression2)
        {
            MemberExpression me1 = (MemberExpression)binaryExpression1.Left;
            MemberExpression me2 = (MemberExpression)binaryExpression2.Left;

            if (me1.Member != me2.Member)
                return false;

            return this.Visit(me1.Expression, me2.Expression) && this.AnalyzeParameters(binaryExpression1.Right, binaryExpression2.Right);
        }

        private bool VisitMemberInit(MemberInitExpression initExpression1, MemberInitExpression initExpression2)
        {
            // test the ctors first - if they don't match - there's no need to continue.
            if (!this.VisitNew(initExpression1.NewExpression, initExpression2.NewExpression))
                return false;

            var bindings1 = initExpression1.Bindings;
            var bindings2 = initExpression2.Bindings;

            int count = bindings1.Count;
            if (count != bindings2.Count)
                return false;

            int matchingMembers = 0;
            for (int i = 0; i < count; ++i)
            {
                MemberAssignment assignment1 = (MemberAssignment)bindings1[i];
                ExpressionType nodeType = assignment1.Expression.NodeType;

                for (int j = 0; j < count; ++j)
                {
                    MemberAssignment assignment2 = (MemberAssignment)bindings2[j];

                    if (!this.Visit(assignment1.Expression, assignment2.Expression))
                        return false;

                    matchingMembers++;
                }
            }

            // returns true if every single piece could be matched
            return matchingMembers == count;
        }

        private bool VisitListInit(ListInitExpression listInitExpression1, ListInitExpression listInitExpression2)
        {
            // this is a shorthand check if everything is alright
            if (!this.VisitNew(listInitExpression1.NewExpression, listInitExpression2.NewExpression))
                return false;

            // distinguish between unsorted lists, sorted lists/sorted/unsorted collections - or just leave it to chance?

            var inits1 = listInitExpression1.Initializers;
            var inits2 = listInitExpression2.Initializers;

            int count = inits1.Count;

            if (count != inits2.Count)
                return false;

            for (int i = 0; i < count; ++i)
            {
                ElementInit ei1 = inits1[i];
                ElementInit ei2 = inits2[i];

                var arguments1 = ei1.Arguments;
                var arguments2 = ei2.Arguments;

                int argumentsCount = arguments1.Count;

                if (argumentsCount != arguments2.Count)
                    break;

                for (int j = 0; j < argumentsCount; ++j)
                    if (!this.Visit(arguments1[i], arguments2[j]))
                        return false;
            }

            // no errors 
            return true;
        }

        private bool VisitConstant(ConstantExpression constantExpression1, ConstantExpression constantExpression2)
        {
            return object.Equals(constantExpression1.Value, constantExpression2.Value);
        }
    }
}
