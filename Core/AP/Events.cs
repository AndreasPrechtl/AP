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
    /// Raises an event - can be used for any event as long as the delegate uses (object, TEventArgs) as parameters;        
    /// </summary>
    /// <param name="source">The event.</param>
    /// <param name="sender">The object causing the event.</param>
    /// <param name="e">The EventArgs.</param>        
    [MethodImpl((MethodImplOptions)256)]
    public static void Raise<TEventArgs>(this EventHandler<TEventArgs> source, object sender, TEventArgs e)
        where TEventArgs : EventArgs
    {
        if (source != null)
            source(sender, e);                
    }

    /// <summary>
    /// Raises an event - can be used for any event as long as the delegate uses (object, EventArgs) as parameters;
    /// like this: (EventHandler)(Delegate)myEvent - this will generate (EventHandler)myEvent but the compiler error is gone
    /// </summary>
    /// <param name="source">The event.</param>
    /// <param name="sender">The object causing the event.</param>
    /// <param name="e">The EventArgs.</param>        
    [MethodImpl((MethodImplOptions)256)]        
    public static void Raise(this EventHandler source, object sender, EventArgs e)
    {
        if (source != null)
            source(sender, e);            
    }

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
            FieldInfo fi = type.GetField(current.Name, flags);
            fi.SetValue(source, null);               
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
            FieldInfo fi = type.GetField(current.Name, flags);
            fi.SetValue(null, null);
        }
    }        
}
