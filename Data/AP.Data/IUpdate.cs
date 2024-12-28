using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AP.Data;

public interface IUpdate
{
    Task Update<TEntity>(TEntity entity) where TEntity : class;
    Task Update<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
    Task<AP.Collections.IListView<TEntity>> Update<TEntity>(Expression<Predicate<TEntity>> where, Action<TEntity> action) where TEntity : class;
}

public interface IUpdate<TEntity>
    where TEntity : class
{
    Task Update(TEntity entity);
    Task Update(IEnumerable<TEntity> entities);
    Task<AP.Collections.IListView<TEntity>> Update(Expression<Predicate<TEntity>> where, Action<TEntity> action);
}
