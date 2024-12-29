using System.Collections.Generic;

namespace AP.Data;

public interface IUpdate
{
    void Update<TEntity>(params IEnumerable<TEntity> entities) where TEntity : class;
}

public interface IUpdate<TEntity>
    where TEntity : class
{
    void Update(params IEnumerable<TEntity> entities);    
}
