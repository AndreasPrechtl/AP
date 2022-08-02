using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Data
{
    /// <summary>
    /// Acts as a Wrapper for any EntitySet that may have an EntityContext
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntitySet<TEntity> : IEntitySet<TEntity>
        where TEntity : class
    {
        private readonly IEntitySet<TEntity> _inner;

        public EntitySet(IEntitySet<TEntity> entitySet)
        {
            if (entitySet == null)
                throw new ArgumentNullException("entitySet");

            _inner = entitySet;
        }

        public IQueryable<TEntity> Query()
        {
            return _inner.Query();
        }

        #region ICreate<TEntity> Members

        public virtual void Create(TEntity entity)
        {
            _inner.Create(entity);
        }

        public virtual void Create(IEnumerable<TEntity> entities)
        {
            _inner.Create(entities);
        }

        #endregion

        #region IUpdate<TEntity> Members

        public virtual void Update(TEntity entity)
        {
            _inner.Update(entity);
        }

        public virtual void Update(IEnumerable<TEntity> entities)
        {
            _inner.Update(entities);
        }

        public virtual Collections.IListView<TEntity> Update(System.Linq.Expressions.Expression<Predicate<TEntity>> where, Action<TEntity> action)
        {
            return _inner.Update(where, action);
        }

        #endregion

        #region IDelete<TEntity> Members

        public virtual void Delete(TEntity entity)
        {
            _inner.Delete(entity);
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            _inner.Delete(entities);
        }

        public virtual Collections.IListView<TEntity> Delete(System.Linq.Expressions.Expression<Predicate<TEntity>> where)
        {
            return _inner.Delete(where);
        }

        #endregion

        #region IEntityFactory<TEntity> Members

        public virtual TEntity GetNewEntity()
        {
            return _inner.GetNewEntity();
        }

        #endregion
    }
}
