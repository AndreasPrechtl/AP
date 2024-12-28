using System;
using System.Linq;
using AP.Reflection;

namespace AP;

// todo!: SourceGen, use all subclasses to provide a set of combined classes! :)
// todo'ish: rename the StaticType class for SourceGen

/// <summary>
/// Class used for "static inheritance".
/// </summary>
public abstract class StaticType
{
    /// <summary>
    /// Throws an exception if it's being invoked.
    /// </summary>
    protected StaticType()
    {
        ThrowTypeInitializationException(this.GetType());
    }

    /// <summary>
    /// Throws a TypeInitializationException.
    /// Does not check if the type is invalid.
    /// </summary>
    /// <param name="type">The Type.</param>
    public static void ThrowTypeInitializationException(Type type) => throw new TypeInitializationException(type.FullName, new Exception("Type must be abstract"));

    /// <summary>
    /// Tests if the type could be used like a StaticType and throws an exception if it is not abstract.
    /// </summary>
    /// <param name="type">The Type.</param>
    public static void AssertTypeQualifies(Type type)
    {
        if (!type.IsAbstract && type.GetMembers().Any(p => p.IsStatic()))
            ThrowTypeInitializationException(type);
    }
}
