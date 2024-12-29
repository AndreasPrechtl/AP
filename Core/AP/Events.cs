using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AP;

/// <summary>
/// Contains several methods to help the use of events
/// </summary>
public static class Events
{
    /// <summary>
    /// Removes all event listeners.
    /// </summary>
    /// <param name="eventHandler">The event.</param>
    public static void ClearHandlers(ref EventHandler? source) => source = null;

    /// <summary>
    /// Removes all event listeners.
    /// </summary>
    /// <param name="eventHandler">The event.</param>
    public static void ClearHandlers<TEventArgs>(ref EventHandler<TEventArgs>? source)
        where TEventArgs : EventArgs => source = null;

    /// <summary>
    /// Uses reflection to remove all event handlers of an instance.
    /// However, this does not include static events.
    /// </summary>
    /// <param name="source">The source.</param>
    public static void ClearHandlers(object source)
    {
        ArgumentNullException.ThrowIfNull(source);

        BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy;

        Type type = source.GetType();

        foreach (EventInfo current in type.GetEvents(flags))
        {
            var fi = type.GetField(current.Name, flags);
            fi?.SetValue(source, null);               
        }
    }

    /// <summary>
    /// Uses reflection to remove all static event handlers.
    /// </summary>
    /// <param name="type">The type.</param>
    public static void ClearHandlers(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy;

        foreach (EventInfo current in type.GetEvents(flags))
        {
            var fi = type.GetField(current.Name, flags);
            fi?.SetValue(null, null);
        }
    }        
}
