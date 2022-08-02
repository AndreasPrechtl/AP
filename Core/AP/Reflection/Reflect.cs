using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Diagnostics;
using AP.Linq;
using System.Runtime.CompilerServices;

namespace AP.Reflection
{
    /// <summary>
    /// Provides additional reflection possibilities.
    /// </summary>
    public static class Reflect
    {
        private static readonly DelegateCache<FieldInfo> _fieldSettersCache;
        private static readonly DelegateCache<FieldInfo> _fieldGettersCache;
        private static readonly DelegateCache<PropertyInfo> _propertyGettersCache;
        private static readonly DelegateCache<PropertyInfo> _propertySettersCache;
        private static readonly DelegateCache<MethodInfo> _methodCache;
        private static readonly DelegateCache<ConstructorInfo> _constructorCache;

        private sealed class DelegateCache<TMemberInfo>
            where TMemberInfo : MemberInfo
        {
            private readonly System.Collections.Generic.Dictionary<TMemberInfo, Delegate> _inner;
            public readonly object SyncRoot = new object();

            public DelegateCache(int capacity)
            {
                _inner = new System.Collections.Generic.Dictionary<TMemberInfo, Delegate>(capacity);
            }

            public bool TryGetDelegate(TMemberInfo member, out Delegate value)
            {
                return _inner.TryGetValue(member, out value);
            }
            public void Add(TMemberInfo member, Delegate value)
            {
                _inner.Add(member, value);
            }
            public bool Remove(TMemberInfo member)
            {
                return _inner.Remove(member);
            }
            public void Clear()
            {
                _inner.Clear();
            }
        }

        static Reflect()
        {
            _fieldGettersCache = new DelegateCache<FieldInfo>(20);
            _fieldSettersCache = new DelegateCache<FieldInfo>(20);
            _propertyGettersCache = new DelegateCache<PropertyInfo>(20);
            _propertySettersCache = new DelegateCache<PropertyInfo>(20);
            _methodCache = new DelegateCache<MethodInfo>(30);
            _constructorCache = new DelegateCache<ConstructorInfo>(10);
        }

        public static MemberInfo GetMember(this Type type, MemberPath path, BindingFlags flags = BindingFlags.Default)
        {
            MemberInfo member = null;
            
            foreach (string memberName in path.Segments)
            {
                MemberInfo[] members = type.GetMember(memberName, flags);
                
                if (members == null || members.Length == 0)
                    break;

                // assume it's the first one - I don't really know why they return an array there - especially when the name is used.
                member = members[0];

                // re-assign the current type
                type = member.GetMemberType();
            }

            return member;
        }

        private static object GetValueInternal(Type type, object target, MemberPath path, BindingFlags flags, out MemberInfo member)
        {
            type = type ?? target.GetType();

            member = null;
            object value = target;

            foreach (string memberName in path.Segments)
            {
                MemberInfo[] members = type.GetMember(memberName, flags);

                if (members == null || members.Length == 0)
                    break;

                // assume it's the first one - I don't really know why they return an array there - especially when the name is used.
                member = members[0];

                if (member.IsField())
                    value = ((FieldInfo)member).GetValue(value);
                else if (member.IsProperty())
                    value = ((PropertyInfo)member).GetValue(value);
                else
                    throw new ArgumentException(string.Format("invalid MemberPath, segment {0} is not a field or property.", memberName));

                // cancel all other attempts to get the value
                if (value == null)
                    break;

                // re-assign the current type
                type = value.GetType();
            }

            // see if the member was found - if not remove the starting object (if it exists)
            if (member == null)
                value = null;
            
            return value;
        }

        /// <summary>
        /// Gets the value of a static field or property by using the MemberPath.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="path"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static object GetValue(this Type type, MemberPath path, BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField | BindingFlags.GetProperty)
        {
            MemberInfo member;

            return GetValueInternal(type, null, path, flags, out member);
        }

        /// <summary>
        /// Gets the value for a property or field by using the MemberPath.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static object GetValue(this object obj, MemberPath path, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField| BindingFlags.GetProperty)
        {
            MemberInfo member;

            return GetValueInternal(null, obj, path, flags, out member);
        }

        /// <summary>
        /// Gets the value for a property or field by using the MemberPath.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <returns></returns>        
        public static T GetValue<T>(this object obj, MemberPath path)
        {
            return (T)GetValue(obj, path);
        }

        /// <summary>
        /// Sets the value of a property or field by using the MemberPath.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <param name="value"></param>
        public static void SetValue(this object obj, MemberPath path, object value, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetField | BindingFlags.SetProperty)
        {
            SetValueInternal(null, obj, path, value, flags);
        }

        public static void SetValue(this Type type, MemberPath path, object value, BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.SetField | BindingFlags.SetProperty)
        {
            SetValueInternal(type, null, path, value, flags);
        }
                
        private static void SetValueInternal(Type type, object target, MemberPath path, object value, BindingFlags flags)
        {
            type = type ?? value.GetType();

            MemberInfo member = null;
            
            object current = target;
            
            MemberPath.SegmentList segments = path.Segments;
            int c = segments.Count - 1;

            for (int i = 0; i < c; ++i)
            {
                string memberName = segments[i];
                
                MemberInfo[] members = type.GetMember(memberName, flags);

                if (members == null || members.Length == 0)
                    break;

                member = members[0];
                                
                if (member.IsField())
                    current = ((FieldInfo)member).GetValue(value);
                else if (member.IsProperty())
                    current = ((PropertyInfo)member).GetValue(value);
                else
                    throw new ArgumentException(string.Format("invalid MemberPath, segment {0} is not a field or property.", memberName));
                
                // re-assign the current type
                type = member.GetMemberType();
            }

            if (current != null)
            {
                MemberInfo[] members = current.GetType().GetMember(segments[c], flags);

                if (member.IsField())
                    ((FieldInfo)member).SetValue(current, value);
                else if (member.IsProperty())
                    ((PropertyInfo)member).SetValue(current, value);
            }
        }
        
        // no inlining works safely for debug and release modes
        // while aggressive inlining - or not using the attribute only works for release mode / code optimization (skipping 1 frame)
        /// <summary>
        /// Gets the calling method
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static MethodBase GetCallingMethod()
        {
            return new StackFrame(2, false).GetMethod();
        }
        
        /// <summary>
        /// Indicates if a member is a field.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>Returns true when the member is a field.</returns>
        [MethodImpl((MethodImplOptions)256)]
        public static bool IsField(this MemberInfo member)
        {
            return member.MemberType == MemberTypes.Field;
        }
                
        /// <summary>
        /// Indicates if a member is an event
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>Returns true when the member is an event.</returns>
        [MethodImpl((MethodImplOptions)256)]
        public static bool IsEvent(this MemberInfo member)
        {
            return member.MemberType == MemberTypes.Event;
        }

        /// <summary>
        /// Indicates if a member is a property.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>Returns true when the member is a property</returns>
        [MethodImpl((MethodImplOptions)256)]
        public static bool IsProperty(this MemberInfo member)
        {
            return member.MemberType == MemberTypes.Property;
        }

        /// <summary>
        /// Indicates if a member is a constructor.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>Returns true when the member is a constructor.</returns>
        [MethodImpl((MethodImplOptions)256)]
        public static bool IsConstructor(this MemberInfo member)
        {
            return member.MemberType == MemberTypes.Constructor;
        }
                
        /// <summary>
        /// Indicates if a member is a method.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>Returns true when the member is a method.</returns>
        [MethodImpl((MethodImplOptions)256)]
        public static bool IsMethod(this MemberInfo member)
        {
            return member.MemberType == MemberTypes.Method;
        }
                
        /// <summary>
        /// Indicates if a field is readonly
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        [MethodImpl((MethodImplOptions)256)]
        public static bool IsReadOnly(this FieldInfo field)
        {
            return field.IsInitOnly;
        }

        /// <summary>
        /// Indicates if a property is readonly or not. 
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>Returns true when a property is readonly.</returns>
        [MethodImpl((MethodImplOptions)256)]
        public static bool IsReadOnly(this PropertyInfo property)
        {
            return !property.CanWrite;
        }

        /// <summary>
        /// Creates a delegate for constructor access.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        /// <returns>Returns a delegate that can be used to create an instance.</returns>
        public static Delegate CreateDelegate(this ConstructorInfo constructor)
        {
            if (constructor == null)
                throw new ArgumentNullException("constructor");

            if (constructor.IsStatic)
                throw new ArgumentException("Static constructors are not allowed.");

            Delegate d = null;

            lock (_constructorCache.SyncRoot)
            {
                ParameterInfo[] parameters = constructor.GetParameters();

                int count = parameters.Length;
                ParameterExpression[] constructorParameters = new ParameterExpression[count];

                for (int i = 0; i < count; ++i)
                {
                    ParameterInfo current = parameters[i];
                    ParameterExpression currentParameter = Expressions.Parameter(current.ParameterType, current.Name);
                    constructorParameters[i] = currentParameter;
                }

                NewExpression newExpression = Expression.New(constructor, constructorParameters);
                LambdaExpression lambda = Expression.Lambda(newExpression, constructorParameters);

                _constructorCache.Add(constructor, d = lambda.Compile());
            }

            return d;
        }

        /// <summary>
        /// Creates a delegate for method access.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>Returns a delegate that can be used to invoke the method.</returns>
        public static Delegate CreateDelegate(this MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            Delegate d = null;

            lock (_methodCache.SyncRoot)
            {
                if (!_methodCache.TryGetDelegate(method, out d))
                {
                    LambdaExpression lambda = null;
                    MethodCallExpression call = null;
                    ParameterExpression[] lambdaParameters = null;

                    bool isStatic = method.IsStatic;

                    ParameterInfo[] parameterInfos = method.GetParameters();
                    int count = parameterInfos.Length;

                    if (isStatic)
                    {
                        lambdaParameters = new ParameterExpression[count];

                        for (int i = 0; i < count; ++i)
                        {
                            ParameterInfo current = parameterInfos[i];
                            lambdaParameters[i] = Expressions.Parameter(current.ParameterType, current.Name);
                        }

                        // static methods use the same parameters as the lambda expression
                        call = Expressions.StaticCall(method, lambdaParameters);
                    }
                    else
                    {
                        ParameterExpression target = Expression.Parameter(method.ReflectedType, "target");

                        // method parameters don't need the instance - while lambdas do
                        ParameterExpression[] methodParameters = new ParameterExpression[count];
                        lambdaParameters = new ParameterExpression[count + 1];

                        // add the target object parameter
                        lambdaParameters[0] = target;

                        for (int i = 0; i < count; )
                        {
                            ParameterInfo current = parameterInfos[i];
                            ParameterExpression currentExpression = Expression.Parameter(current.ParameterType, current.Name);

                            methodParameters[i] = currentExpression;
                            lambdaParameters[++i] = currentExpression;
                        }
                        call = Expressions.InstanceCall(target, method, methodParameters);
                    }

                    lambda = Expressions.Lambda(call, lambdaParameters);
                    _methodCache.Add(method, d = lambda.Compile());
                }
            }

            return d;
        }

        /// <summary>
        /// Creates a setter delegate for fields. 
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>Returns a delegate that can be used to set the value of a field.</returns>
        public static Delegate CreateSetterDelegate(this FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            if (field.IsReadOnly())
                throw new ArgumentException("field is readonly");

            Delegate d = null;

            lock (_fieldSettersCache.SyncRoot)
            {
                if (!_fieldSettersCache.TryGetDelegate(field, out d))
                {
                    bool isStatic = field.IsStatic;

                    ParameterExpression target = isStatic ? null : Expression.Parameter(field.ReflectedType, "target");
                    ParameterExpression value = Expression.Parameter(field.FieldType, "value");

                    MemberExpression member = Expression.Field(target, field);
                    BinaryExpression assign = Expression.Assign(member, value);

                    LambdaExpression lambda = isStatic ? Expression.Lambda(assign, value) : Expression.Lambda(assign, target, value);

                    d = lambda.Compile();
                    _fieldSettersCache.Add(field, d);
                }
            }

            return d;
        }

        /// <summary>
        /// Creates a getter delegate for fields.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>Returns a delegate that can be used to get the value of a field.</returns>
        public static Delegate CreateGetterDelegate(this FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            Delegate d = null;

            lock (_fieldGettersCache.SyncRoot)
            {
                if (!_fieldGettersCache.TryGetDelegate(field, out d))
                {
                    bool isStatic = field.IsStatic;

                    ParameterExpression target = isStatic ? null : Expression.Parameter(field.ReflectedType, "target");

                    MemberExpression member = Expression.Field(target, field);
                    LambdaExpression lambda = isStatic ? Expression.Lambda(member) : Expression.Lambda(member, target);

                    _fieldGettersCache.Add(field, d = lambda.Compile());
                }
            }

            return d;
        }

        /// <summary>
        /// Creates a setter delegate for properties.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>Returns a delegate that can be used to set the value of a property.</returns>
        public static Delegate CreateSetterDelegate(this PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            // if the declaring type is an anonymous type - return a delegate to the automatically generated backing field
            // and since the property is readonly I need to do that before I check if it is a true readonly property.
            if (property.DeclaringType.IsAnonymous())
            {
                string fieldName = string.Format("<{0}>i__Field", property.Name);

                return CreateSetterDelegate(property.DeclaringType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance));
            }

            if (property.IsReadOnly())
                throw new ArgumentException("property is readonly");

            Delegate d = null;

            lock (_propertySettersCache.SyncRoot)
            {
                if (!_propertySettersCache.TryGetDelegate(property, out d))
                {
                    bool isStatic = property.IsStatic();

                    ParameterExpression target = isStatic ? null : Expression.Parameter(property.ReflectedType, "target");
                    ParameterExpression value = Expression.Parameter(property.PropertyType, "value");

                    MemberExpression member = Expression.Property(target, property);
                    BinaryExpression assign = Expression.Assign(member, value);

                    LambdaExpression lambda = isStatic ? Expression.Lambda(assign, value) : Expression.Lambda(assign, target, value);
                    _propertySettersCache.Add(property, d = lambda.Compile());
                }
            }

            return d;
        }

        /// <summary>
        /// Creates a getter delegate for property access.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>Returns a delegate that can be used to get the value of a property.</returns>
        public static Delegate CreateGetterDelegate(this PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            if (!property.CanRead)
                throw new ArgumentException("property is not readable");

            Delegate d = null;

            lock (_propertyGettersCache.SyncRoot)
            {
                if (!_propertyGettersCache.TryGetDelegate(property, out d))
                {
                    bool isStatic = property.IsStatic();

                    ParameterExpression target = isStatic ? null : Expression.Parameter(property.ReflectedType, "target");

                    // Expression.Property can be used here as well
                    MemberExpression member = Expression.Property(target, property);

                    LambdaExpression lambda = isStatic ? Expression.Lambda(member) : Expression.Lambda(member, target);
                    _propertyGettersCache.Add(property, d = lambda.Compile());
                }
            }

            return d;
        }

        /// <summary>
        /// Compares two methods for byte-code equality.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="other">The other method.</param>
        /// <returns>Returns true when both methods have the same address or byte-code.</returns>
        public static bool MethodBodyEqual(this MethodInfo method, MethodInfo other)
        {
            if (method == other)
                return true;

            byte[] il1 = method.GetMethodBody().GetILAsByteArray();
            byte[] il2 = other.GetMethodBody().GetILAsByteArray();

            return il1.SequenceEqual(il2);
        }

        /// <summary>
        /// Gets the member type.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>Returns the member type.</returns>
        public static Type GetMemberType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;

                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;

                case MemberTypes.Constructor:
                    return ((ConstructorInfo)member).DeclaringType;

                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;

                case MemberTypes.TypeInfo:
                case MemberTypes.NestedType:
                    return ((Type)member).DeclaringType;

                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                
                default:
                    return null;
            }
        }

        /// <summary>
        /// Indicates if a member is a static member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>Returns true when the member is static.</returns>
        public static bool IsStatic(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)member).IsStatic;

                case MemberTypes.Property:
                    return ((PropertyInfo)member).IsStatic();
                
                case MemberTypes.Constructor:
                    return ((ConstructorInfo)member).IsStatic;

                case MemberTypes.Method:
                    return ((MethodInfo)member).IsStatic;

                case MemberTypes.TypeInfo:
                case MemberTypes.NestedType:
                    return ((Type)member).IsStatic();

                case MemberTypes.Event:
                    return ((EventInfo)member).IsStatic();
                
                default:
                    return ((dynamic)member).IsStatic;
            }
        }

        /// <summary>
        /// Indicates if a type is a static type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Returns true when the type is a static type.</returns>
        public static bool IsStatic(this Type type)
        {
            const TypeAttributes attributes = TypeAttributes.AutoLayout | TypeAttributes.AnsiClass | TypeAttributes.Class | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit;

            return attributes == (type.Attributes & attributes);
        }

        /// <summary>
        /// Indicates if a method is a static method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>Returns true when the method is a static method.</returns>
        public static bool IsStatic(this MethodInfo method)
        {
            return method.IsStatic;
        }

        /// <summary>
        /// Indicates if a constructor is a static constructor.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        /// <returns>Returns true when the constructor is a static constructor.</returns>
        public static bool IsStatic(this ConstructorInfo constructor)
        {
            return constructor.IsStatic;
        }

        /// <summary>
        /// Indicates if a property is a static property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>Returns true when the property is a static property.</returns>
        public static bool IsStatic(this PropertyInfo property)
        {
            return property.GetAccessors()[0].IsStatic;
        }

        /// <summary>
        /// Indicates if an event is a static event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Returns true when the event is a static event.</returns>
        public static bool IsStatic(this EventInfo @event)
        {
            return @event.GetAddMethod().IsStatic;
        }

        /// <summary>
        /// Indicates if an event is generic.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Returns true when the event contains generic parameters.</returns>
        public static bool IsGeneric(this EventInfo @event)
        {
            return @event.GetAddMethod().IsGenericMethod;
        }

        /// <summary>
        /// Indicates if a type is nullable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Returns true when the type is a generic System.Nullable`T type.</returns>
        public static bool IsNullable(this Type type)
        {
            return type.IsValueType && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Gets a static FieldInfo
        /// () => MyClass.myStaticField
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static FieldInfo GetField<TResult>(Expression<Invoke<TResult>> expression)
        {
            return (FieldInfo)((MemberExpression)expression.Body).Member;
        }

        /// <summary>
        /// Gets an instance FieldInfo
        /// p => p.myField
        /// </summary>
        /// <typeparam name="TDeclaringType"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static FieldInfo GetField<TInstance, TResult>(Expression<Invoke<TInstance, TResult>> expression)
        {
            return (FieldInfo)((MemberExpression)expression.Body).Member;
        }

        /// <summary>
        /// Gets a static PropertyInfo
        /// () => MyClass.MyStaticProperty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static PropertyInfo GetProperty<TResult>(Expression<Invoke<TResult>> expression)
        {
            return (PropertyInfo)((MemberExpression)expression.Body).Member;
        }

        /// <summary>
        /// Gets an instance PropertyInfo
        /// p => p.MyProperty
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static PropertyInfo GetProperty<TInstance, TResult>(Expression<Invoke<TInstance, TResult>> expression)
        {
            return (PropertyInfo)((MemberExpression)expression.Body).Member;
        }

        /// <summary>
        /// Gets the method using the name, flags and parameter types.
        /// </summary>
        /// <param name="type">The Type.</param>
        /// <param name="methodName">The method's name.</param>
        /// <param name="flags">The binding flags.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <returns>The method.</returns>
        public static MethodInfo GetMethod(this Type type, string methodName, BindingFlags flags, params Type[] parameterTypes)
        {
            MethodInfo[] methods = type.GetMethods(flags);

            foreach (MethodInfo method in methods)
            {
                if (method.Name == methodName)
                {
                    ParameterInfo[] parameters = method.GetParameters();

                    if (parameters.Length == parameterTypes.Length)
                    {
                        for (int i = 0, c = parameters.Length; i < c; ++i)
                        {
                            ParameterInfo parameter = parameters[i];
                            Type parameterType = parameterTypes[i];

                            if (!parameterType.Is(parameter.ParameterType))
                                return null;
                        }
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// Gets a static MethodInfo
        /// () => MyClass.MyStaticMethod([args])
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MethodInfo GetMethod<TResult>(Expression<Invoke<TResult>> expression)
        {
            return ((MethodCallExpression)expression.Body).Method;
        }

        /// <summary>
        /// Gets an instance MethodInfo
        /// p => p.MyMethod([args])
        /// </summary>
        /// <typeparam name="TDeclaringType"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MethodInfo GetMethod<TInstance, TResult>(Expression<Invoke<TInstance, TResult>> expression)
        {
            return ((MethodCallExpression)expression.Body).Method;
        }

        /// <summary>
        /// Gets the ConstructorInfo
        /// () => new MyClass([args])
        /// </summary>
        /// <typeparam name="TDeclaringType"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>    
        public static ConstructorInfo GetConstructor<TDeclaringType>(Expression<Invoke<TDeclaringType>> expression)
        {
            return ((NewExpression)expression.Body).Constructor;
        }

        /// <summary>
        /// Extracts the instance member from a lambda expression
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MemberInfo GetMember<TInstance, TResult>(Expression<Invoke<TInstance, TResult>> expression)
        {
            return GetMember((LambdaExpression)expression);
        }

        /// <summary>
        /// Extracts the static member from a lambda expression
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MemberInfo GetMember<TResult>(Expression<Invoke<TResult>> expression)
        {
            return GetMember((LambdaExpression)expression);
        }

        private static MemberInfo GetMember(LambdaExpression expression)
        {
            Expression body = expression.Body;

            if (body is MethodCallExpression)
                return ((MethodCallExpression)body).Method;

            if (body is MemberExpression)
                return ((MemberExpression)body).Member;

            if (body is NewExpression)
                return ((NewExpression)body).Constructor;

            throw new ArgumentException("Invalid expression");
        }

        public const BindingFlags AllMembers = BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.InvokeMethod;
        public const BindingFlags AllFields = BindingFlags.GetField | BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
        public const BindingFlags AllMethods = BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
        public const BindingFlags AllConstructors = BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
        public const BindingFlags AllProperties = BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;

        public static IEnumerable<MemberInfo> GetMembers<TDeclaringType>(BindingFlags flags = AllMembers)
        {
            return typeof(TDeclaringType).GetMembers(flags).AsReadOnly();
        }

        public static IEnumerable<ConstructorInfo> GetConstructors<TDeclaringType>(BindingFlags flags = AllConstructors)
        {
            return typeof(TDeclaringType).GetConstructors(flags).AsReadOnly();
        }

        public static IEnumerable<FieldInfo> GetFields<TDeclaringType>(BindingFlags flags = AllFields)
        {
            return typeof(TDeclaringType).GetFields(flags).AsReadOnly();
        }

        public static IEnumerable<PropertyInfo> GetProperties<TDeclaringType>(BindingFlags flags = AllProperties)
        {
            return typeof(TDeclaringType).GetProperties(flags).AsReadOnly();
        }

        public static IEnumerable<MethodInfo> GetMethods<TDeclaringType>(BindingFlags flags = AllMethods)
        {
            return typeof(TDeclaringType).GetMethods(flags).AsReadOnly();
        }
    }
}