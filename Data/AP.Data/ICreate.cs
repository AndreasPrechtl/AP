using System.Collections.Generic;
using System.Threading.Tasks;

namespace AP.Data;

public interface ICreate 
{
    void Create<TEntity>(params IEnumerable<TEntity> entities) where TEntity : class;
}

public interface ICreate<in TEntity>
     where TEntity : class
{
    void Create(params IEnumerable<TEntity> entities);
}
