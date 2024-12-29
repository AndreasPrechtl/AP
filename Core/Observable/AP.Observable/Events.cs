using System.ComponentModel;

namespace AP.Observable;

public static class Events
{
    public static void Raise(this PropertyChangingEventHandler handler, object? sender, PropertyChangingEventArgs e)
    {
        handler?.Invoke(sender, e);
    }

    public static void Raise(this PropertyChangedEventHandler handler, object? sender, PropertyChangedEventArgs e)
    {
        handler?.Invoke(sender, e);
    }

    public static void Raise(this ObjectChangingEventHandler handler, object? sender, ObjectChangingEventArgs e)
    {
        handler?.Invoke(sender, e);
    }

    public static void Raise<T>(this ObjectChangingEventHandler<T> handler, object? sender, ObjectChangedEventArgs<T> e)
        where T : INotifyObjectChanging<T>
    {
        handler?.Invoke(sender, e);
    }

    public static void Raise(this ObjectChangedEventHandler handler, object? sender, ObjectChangedEventArgs e)
    {
        handler?.Invoke(sender, e);
    }

    public static void Raise<T>(this ObjectChangedEventHandler<T> handler, object? sender, ObjectChangedEventArgs<T> e)
        where T : INotifyObjectChanged<T>
    {
        handler?.Invoke(sender, e);
    }

    public static void Raise(this MemberAccessingEventHandler handler, object? sender, MemberAccessingEventArgs e)
    {
        handler?.Invoke(sender, e);
    }

    public static void Raise(this MemberAccessedEventHandler handler, object? sender, MemberAccessedEventArgs e)
    {
        handler?.Invoke(sender, e);
    }
}
