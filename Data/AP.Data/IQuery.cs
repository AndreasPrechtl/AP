using System.Linq;

namespace AP.Data;

public interface IQuery
{
    IQueryable<TEntity> Query<TEntity>() where TEntity : class;
}

public interface IQuery<out TEntity>
    where TEntity : class
{
    IQueryable<TEntity> Query();
}    
