namespace AP.ComponentModel;

public interface IManager : IResolver
{
    void Register<T>(object key, T instance);
    void Release(object key);        
}

public interface IManager<TBase> : IResolver<TBase>
{
    void Register<T>(object key, T instance) where T : TBase;
    void Release(object key);
}

public interface IManager<TKey, TBase> : IResolver<TKey, TBase>
{
    void Register<T>(TKey key, T instance) where T : TBase;
    void Release(TKey key);
}