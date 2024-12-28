using System;
using System.Collections;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace AP.Reflection;

/// <summary>
/// Extracts extra information about types.
/// </summary>
public static class Types
{
    public static readonly Type ObjectType = typeof(object);
    public static readonly Type StringType = typeof(string);
    private static readonly Type _charArray = typeof(char[]);

    public static readonly Type DynamicType = typeof(IDynamicMetaObjectProvider);

    public static readonly Type[] CollectionTypes = new[] { typeof(System.Collections.Generic.ICollection<>), typeof(System.Collections.ICollection), typeof(System.Collections.IDictionary), typeof(System.Collections.Generic.IReadOnlyCollection<>) };
    public static readonly Type EnumerableType = typeof(IEnumerable);

    /// <summary>
    /// Returns true when a type is a subclass of TBase.
    /// </summary>
    /// <typeparam name="TBase"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsSubclassOf<TBase>(this Type type) => type.IsSubclassOf(typeof(TBase));

    /// <summary>
    /// Returns true when a type is inherited from TBase.
    /// </summary>
    /// <typeparam name="TBase"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool Is<TBase>(this Type type) => Is(typeof(TBase), type);

    public static bool Is(this Type type, Type baseType) => baseType.IsAssignableFrom(type);

    /// <summary>
    /// Returns true when a type implements an interface.
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool Implements<TInterface>(this Type type) => Implements(type, typeof(TInterface));

    /// <summary>
    /// Returns true when a type implements an interface.
    /// </summary>
    /// <typeparam name="TBase"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool Implements(this Type type, Type interfaceType)
    {
        if (!interfaceType.IsInterface)
            throw new ArgumentException("not an interface type");

        return interfaceType.IsAssignableFrom(type);
    }


    public static bool IsAssignableFrom<T>(this Type type) => type.IsAssignableFrom(typeof(T));

    /// <summary>
    /// Returns true when the type is an anonymous type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>True when the type is anonymous.</returns>
    public static bool IsAnonymous(this Type type)
    {
        if (type.IsGenericType && type.IsSecurityCritical && type.IsClass && type.IsNotPublic && type.IsSealed && type.IsAutoLayout && type.IsAnsiClass)
        {
            object[] attributes = type.GetGenericTypeDefinition().GetCustomAttributes(typeof(CompilerGeneratedAttribute), false);

            return attributes?.Length > 0;
        }

        return false;
    }

    /// <summary>
    /// Indicates if a type implements the IEnumerable interface.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>True when the type implements IEnumerable.</returns>
    public static bool IsEnumerable(this Type type) => type.IsArray || type.IsAssignableFrom(EnumerableType);

    /// <summary>
    /// Returns true if the Type is a Reference Type or a Nullable<T>
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsNullable(this Type type)
    {
        if (!type.IsValueType)
            return true;

        if (type.IsGenericType)
        {
            if (!type.IsGenericTypeDefinition)
                type = type.GetGenericTypeDefinition();

            return type.Equals(typeof(Nullable<>));    
        }
        return false;
    }

    /// <summary>
    /// Returns true if type implements a Collection interface (not IEnumerable).
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsCollection(this Type type)
    {
        Type[] interfaces = type.GetInterfaces();
        Type[] collectionTypes = CollectionTypes;
        
        foreach (Type itype in interfaces)
        {
            foreach (Type ctype in collectionTypes)
            {
                // see if it's assignable directly or implements a generic collection interface
                if (ctype.IsAssignableFrom(itype) || (itype.IsInterface && (ctype.IsGenericType && ctype.IsAssignableFrom(itype.IsGenericTypeDefinition ? itype : itype.GetGenericTypeDefinition()))))
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns true if the type is a DynamicObject or ExpandoObject
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsDynamicType(this Type type) => DynamicType.IsAssignableFrom(type);

    /// <summary>
    /// Returns true if the type is a primitive type or a string
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsPrimitiveOrString(this Type type) => type.IsPrimitive || type == StringType || type == _charArray;//TypeCode typeCode = Type.GetTypeCode(type);//switch (typeCode)//{// //   case TypeCode.Boolean:// //   case TypeCode.Byte:// //   case TypeCode.Char:// //   case TypeCode.DateTime:// ////   case TypeCode.DBNull: // //   case TypeCode.Decimal:// //   case TypeCode.Double:// //   case TypeCode.Empty:// //   case TypeCode.Int16:// //   case TypeCode.Int32:// //   case TypeCode.Int64:// //   //  case TypeCode.Object:// //   case TypeCode.SByte:// //   case TypeCode.Single:// //   case TypeCode.String:// //   case TypeCode.UInt16:// //   case TypeCode.UInt32:// //   case TypeCode.UInt64://        //return true;//        //break;//    //default://    //    return false;//    // shortcut ;)//    case TypeCode.Object://    case TypeCode.DBNull://        return false;//    default://        return true;//}
}
