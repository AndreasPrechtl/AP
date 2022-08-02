using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.ComponentModel;
using AP.Reflection;
using System.Reflection;
using AP.Linq.Expressions;

namespace AP.ComponentModel
{
    /// obsolete for .net 4.5 and higher 
    //public static class NotifierExtensions
    //{
    //    private struct NotifierExpressionInfo
    //    {
    //        public string PropertyName;
    //        public object Sender;
    //    }

    //    private const string _propertyChanged = "PropertyChanged";
    //    private const string _propertyChanging = "PropertyChanging";

    //    private static NotifierExpressionInfo ExtractInfo(LambdaExpression property)
    //    {
    //        MemberExpression mex = (MemberExpression)property.Body;

    //        NotifierExpressionInfo expressionInfo = new NotifierExpressionInfo { PropertyName = mex.Member.Name };
    //        expressionInfo.Sender = ((ConstantExpression)mex.Expression).Value;

    //        return expressionInfo;
    //    }

    //    /// <summary>
    //    /// Used to fire the changing event by using a lambda expression containing info about the property.
    //    /// Adds a bit of an overhead but removes renaming problems.
    //    /// Usage:
    //    /// 
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="eventHandler"></param>
    //    /// <param name="property"></param>
    //    public static void Notify<TInstance, T>(this PropertyChangingEventHandler eventHandler, Expression<Invoke<TInstance, T>> property)                        
    //        where TInstance : INotifyPropertyChanging
    //    {            
    //        NotifierExpressionInfo expressionInfo = ExtractInfo(property);
    //        eventHandler.TryInvoke(expressionInfo.Sender, new PropertyChangingEventArgs(expressionInfo.PropertyName));
    //    }

    //    /// <summary>
    //    /// Used to fire the changed event by using a lambda expression containing info about the property.
    //    /// Adds a bit of an overhead but removes renaming problems. 
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="eventHandler"></param>
    //    /// <param name="property"></param>
    //    public static void Notify<TInstance, T>(this PropertyChangedEventHandler eventHandler, Expression<Action<TInstance, T>> property)
    //        where TInstance : INotifyPropertyChanging
    //    {
    //        NotifierExpressionInfo expressionInfo = ExtractInfo(property);
    //        eventHandler.TryInvoke(expressionInfo.Sender, new PropertyChangedEventArgs(expressionInfo.PropertyName));
    //    }
    //}
}
