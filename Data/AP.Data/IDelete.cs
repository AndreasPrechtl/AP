using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AP.Data
{
    public interface IDelete
    {
        void Delete<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        AP.Collections.IListView<TEntity> Delete<TEntity>(Expression<Predicate<TEntity>> where) where TEntity : class;
    }

    public interface IDelete<TEntity>
        where TEntity : class
    {
        void Delete(TEntity entity);
        void Delete(IEnumerable<TEntity> entities);
        AP.Collections.IListView<TEntity> Delete(Expression<Predicate<TEntity>> where);
    }

    //public interface IParallelDelete
    //{
    //    async TEntity Delete<TEntity>(TEntity entity) where TEntity : class;
    //    async IEnumerable<TEntity> Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
    //    async IEnumerable<TEntity> Delete<TEntity>(Expression<Predicate<TEntity>> where) where TEntity : class;
    //}

    //public interface IParallelDelete<TEntity>
    //    where TEntity : class
    //{
    //    async TEntity Delete(TEntity entity);
    //    async IEnumerable<TEntity> Delete(IEnumerable<TEntity> entities);
    //    async IEnumerable<TEntity> Delete(Expression<Predicate<TEntity>> where);
    //}
}
