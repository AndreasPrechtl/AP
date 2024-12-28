using System.ComponentModel;

namespace AP.Observable;

public static class Events
{
    public static void Raise(this PropertyChangingEventHandler handler, object? sender, PropertyChangingEventArgs e)
    {
        if (handler != null)
            handler(sender, e);
    }

    public static void Raise(this PropertyChangedEventHandler handler, object? sender, PropertyChangedEventArgs e)
    {
        if (handler != null)
            handler(sender, e);
    }

    public static void Raise(this ObjectChangingEventHandler handler, object? sender, ObjectChangingEventArgs e)
    {
        if (handler != null)
            handler(sender, e);
    }

    public static void Raise<T>(this ObjectChangingEventHandler<T> handler, object? sender, ObjectChangedEventArgs<T> e)
        where T : INotifyObjectChanging<T>
    {
        if (handler != null)
            handler(sender, e);
    }

    public static void Raise(this ObjectChangedEventHandler handler, object? sender, ObjectChangedEventArgs e)
    {
        if (handler != null)
            handler(sender, e);
    }

    public static void Raise<T>(this ObjectChangedEventHandler<T> handler, object? sender, ObjectChangedEventArgs<T> e)
        where T : INotifyObjectChanged<T>
    {
        if (handler != null)
            handler(sender, e);
    }

    public static void Raise(this MemberAccessingEventHandler handler, object? sender, MemberAccessingEventArgs e)
    {
        if (handler != null)
            handler(sender, e);
    }

    public static void Raise(this MemberAccessedEventHandler handler, object? sender, MemberAccessedEventArgs e)
    {
        if (handler != null)
            handler(sender, e);
    }
}
