namespace AP.Data;

//public interface IEntityReference<TEntity>
//    where TEntity : class
//{
//    TEntity Value { get; set; }
//    bool HasValue { get; }
//}

public interface IEntityReference<TKey, TEntity>
    where TEntity : class
{
    TKey Key { get; set; }
    TEntity Value { get; set; }
    bool HasValue { get; }
}