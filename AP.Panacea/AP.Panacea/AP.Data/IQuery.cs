using System.Linq;

namespace AP.Data
{
    public interface IQuery
    {
        IQueryable<TEntity> Query<TEntity>() where TEntity : class;
    }

    public interface IQuery<out TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> Query();
    }
    
    //public interface IParallelQuery
    //{
    //    ParallelQuery<TEntity> Query<TEntity>() where TEntity : class;
    //}

    //public interface IParallelQuery<TEntity>
    //    where TEntity : class
    //{
    //    ParallelQuery<TEntity> Query();
    //}   
}
