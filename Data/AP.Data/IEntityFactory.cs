namespace AP.Data;

public interface IEntityFactory
{
    TEntity GetNewEntity<TEntity>() where TEntity : class;
}

public interface IEntityFactory<out TEntity>
    where TEntity : class
{
    TEntity GetNewEntity();
}
