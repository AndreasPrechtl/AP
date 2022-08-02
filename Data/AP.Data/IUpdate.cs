using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AP.Data
{
    public interface IUpdate
    {
        void Update<TEntity>(TEntity entity) where TEntity : class;
        void Update<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        AP.Collections.IListView<TEntity> Update<TEntity>(Expression<Predicate<TEntity>> where, Action<TEntity> action) where TEntity : class;
    }

    public interface IUpdate<TEntity>
        where TEntity : class
    {
        void Update(TEntity entity);
        void Update(IEnumerable<TEntity> entities);
        AP.Collections.IListView<TEntity> Update(Expression<Predicate<TEntity>> where, Action<TEntity> action);
    }

    //public interface IParallelUpdate
    //{
    //    async TEntity Update<TEntity>(TEntity entity) where TEntity : class;
    //    async IEnumerable<TEntity> Update<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
    //    async IEnumerable<TEntity> Update<TEntity>(Expression<Predicate<TEntity>> where, ModifyAndReturn<TEntity> action) where TEntity : class;
    //    async IEnumerable<TEntity> Update<TEntity>(Expression<Predicate<TEntity>> where, Action<TEntity> action) where TEntity : class;
    //}

    //public interface IParallelUpdate<TEntity>
    //    where TEntity : class
    //{
    //    async TEntity Update(TEntity entity);
    //    async IEnumerable<TEntity> Update(IEnumerable<TEntity> entities);
    //    async IEnumerable<TEntity> Update(Expression<Predicate<TEntity>> where, ModifyAndReturn<TEntity> action);
    //    async IEnumerable<TEntity> Update(Expression<Predicate<TEntity>> where, Action<TEntity> action);
    //}
}
