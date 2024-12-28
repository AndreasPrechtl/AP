using System.Collections.Generic;

namespace AP.Data
{
    public interface ICreate 
    {
        void Create<TEntity>(TEntity entity) where TEntity : class;
        void Create<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
    }

    public interface ICreate<in TEntity>
         where TEntity : class
    {
        void Create(TEntity entity);
        void Create(IEnumerable<TEntity> entities);
    }

    //public interface IParallelInsert
    //{
    //    async TEntity Insert<TEntity>(TEntity entity) where TEntity : class;
    //    async IEnumerable<TEntity> Insert<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
    //}

    //public interface IParallelInsert<TEntity>
    //     where TEntity : class
    //{
    //    async TEntity Insert(TEntity entity);
    //    async IEnumerable<TEntity> Insert(IEnumerable<TEntity> entities);
    //}
}
