namespace AP.Data;

public interface IMultiEntityReference<TKey, TEntity>
    where TEntity : class
{
    TKey Key { get; set; }
    IEntitySet<TEntity> Values { get; }
}