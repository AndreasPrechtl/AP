using System.Collections.Generic;
using System.Threading.Tasks;

namespace AP.Data;

public interface ICreate 
{
    Task Create<TEntity>(TEntity entity) where TEntity : class;
    Task Create<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
}

public interface ICreate<in TEntity>
     where TEntity : class
{
    Task Create(TEntity entity);
    Task Create(IEnumerable<TEntity> entities);
}
