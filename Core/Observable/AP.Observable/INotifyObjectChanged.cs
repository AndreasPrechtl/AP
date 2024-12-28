namespace AP.Observable;

public delegate void ObjectChangedEventHandler(object? sender, ObjectChangedEventArgs e);
public delegate void ObjectChangedEventHandler<T>(object? sender, ObjectChangedEventArgs<T> e);
//public delegate void ObjectChangedEventHandler<T, TValue>(object sender, ObjectChangedEventArgs<T, TValue> e);

public interface INotifyObjectChanged
{
    event ObjectChangedEventHandler Changed;
}

public interface INotifyObjectChanged<T> : INotifyObjectChanged
{
    new event ObjectChangedEventHandler<T> Changed;
}

//public interface INotifyObjectChanged<T, TValue> : INotifyObjectChanged<T>
//{
//    new event ObjectChangedEventHandler<T, TValue> Changed;
//}
