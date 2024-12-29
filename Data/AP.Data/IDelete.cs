using System.Collections.Generic;

namespace AP.Data;

public interface IDelete
{
    void Delete<TEntity>(params IEnumerable<TEntity> entities) where TEntity : class;
}

public interface IDelete<TEntity>
    where TEntity : class
{
    void Delete(params IEnumerable<TEntity> entities); 
}