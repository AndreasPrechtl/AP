namespace AP.Data;

public interface IEntitySet<TEntity> :
    IQuery<TEntity>,
    ICreate<TEntity>,
    IUpdate<TEntity>,
    IDelete<TEntity>,
    IEntityFactory<TEntity>
    where TEntity : class    
{ }

//public interface IParallelEntitySet<TEntity> :
//   IParallelQuery<TEntity>,
//   IParallelInsert<TEntity>,
//   IParallelUpdate<TEntity>,
//   IParallelDelete<TEntity>,
//   IEntityFactory<TEntity>,
//   AP.IDisposable
//   where TEntity : class
//{ }
