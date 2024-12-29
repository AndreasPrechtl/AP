namespace AP.Observable;

public delegate void ObjectChangingEventHandler(object? sender, ObjectChangingEventArgs e);
public delegate void ObjectChangingEventHandler<T>(object? sender, ObjectChangedEventArgs<T> e);
//public delegate void ObjectChangingEventHandler<T, TValue>(object sender, ObjectChangedEventArgs<T, TValue> e);

public interface INotifyObjectChanging
{
    event ObjectChangingEventHandler? Changing;
}

public interface INotifyObjectChanging<T> : INotifyObjectChanging
{
    new event ObjectChangingEventHandler<T>? Changing;
}

//public interface INotifyObjectChanging<T, TValue> : INotifyObjectChanging<T>
//{
//    new event ObjectChangingEventHandler<T, TValue> Changing;
//}
