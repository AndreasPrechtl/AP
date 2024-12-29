namespace AP.Data;

public interface IEntitySet<TEntity> :
    IQuery<TEntity>,
    ICreate<TEntity>,
    IUpdate<TEntity>,
    IDelete<TEntity>,
    IEntityFactory<TEntity>
    where TEntity : class    
{ }