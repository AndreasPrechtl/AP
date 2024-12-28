using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AP.Data;

public interface IDelete
{
    Task Delete<TEntity>(TEntity entity) where TEntity : class;
    Task Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
    Task<AP.Collections.IListView<TEntity>> Delete<TEntity>(Expression<Predicate<TEntity>> where) where TEntity : class;
}

public interface IDelete<TEntity>
    where TEntity : class
{
    Task Delete(TEntity entity);
    Task Delete(IEnumerable<TEntity> entities);
    Task<AP.Collections.IListView<TEntity>> Delete(Expression<Predicate<TEntity>> where);
}
