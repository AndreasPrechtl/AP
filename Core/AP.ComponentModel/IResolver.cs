namespace AP.ComponentModel;

public interface IResolver 
{
    /// <summary>
    /// If the key is null it returns the first match
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    T Get<T>(object? key = null);        
}

public interface IResolver<TBase>    
{
    /// <summary>
    /// If the key is null it returns the first match
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    T Get<T>(object? key = null) where T : TBase;
}

public interface IResolver<TKey, TBase>
{
    /// <summary>
    /// If the key is null it returns the first match
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    T Get<T>(TKey? key = default) where T : TBase;
}
