using System;

namespace AP;

/// <summary>
/// Contains methods that help with IFreezable implementations
/// </summary>
public static class FreezableHelper
{
    public static void AssertCanWrite(this IFreezable freezable)
    {
        if (freezable.IsFrozen)
            throw new InvalidOperationException("Object cannot be changed after it became readonly");
    }

    public static bool TryFreeze(this IFreezable value)
    {
        if (value.IsFrozen)
            return false;

        value.IsFrozen = true;
        return true;
    }

    public static void Freeze(this IFreezable value) => value.IsFrozen = true;
}