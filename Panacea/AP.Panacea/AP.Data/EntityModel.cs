using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Linq.Expressions;

namespace AP.Data
{
    /// <summary>
    /// Class for an EntityModel with an external context
    /// </summary>
    /// <typeparam name="TEntityContext">The external context.</typeparam>
    public class EntityModel<TEntityContext> : IEntityModel, IEntityContextUser<TEntityContext>
        where TEntityContext : EntityContextBase
    {
        private TEntityContext _entityContext;

        protected TEntityContext EntityContext
        {
            get { return _entityContext; }
        }

        public EntityModel(TEntityContext entityContext)
        {
            ExceptionHelper.AssertNotNull(() => entityContext);

            _entityContext = entityContext;
            entityContext.Disposed += EntityContextDisposed;
        }

        private void EntityContextDisposed(object sender, EventArgs e)
        {
            TEntityContext ctx = _entityContext;
            if (ctx != null)
            {
                ctx.Disposed -= this.EntityContextDisposed;
                _entityContext = null;
            }
        }

        public IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            return this.GetEntitySet<TEntity>().Query();
        }

        public void Create<TEntity>(TEntity entity) where TEntity : class
        {
            this.GetEntitySet<TEntity>().Create(entity);
        }

        public void Create<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            this.GetEntitySet<TEntity>().Create(entities);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            this.GetEntitySet<TEntity>().Update(entity);
        }

        public void Update<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            this.GetEntitySet<TEntity>().Update(entities);
        }

        public Collections.IList<TEntity> Update<TEntity>(Expression<Predicate<TEntity>> where, Action<TEntity> action) where TEntity : class
        {
            return this.GetEntitySet<TEntity>().Update(where, action);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            this.GetEntitySet<TEntity>().Delete(entity);
        }

        public void Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            this.GetEntitySet<TEntity>().Delete(entities);
        }

        public Collections.IList<TEntity> Delete<TEntity>(Expression<Predicate<TEntity>> where) where TEntity : class
        {
            return this.GetEntitySet<TEntity>().Delete(where);
        }

        public IEntitySet<TEntity> GetEntitySet<TEntity>() where TEntity : class
        {
            return this.EntityContext.GetEntitySet<TEntity>();
        }

        public TEntity GetNewEntity<TEntity>() where TEntity : class
        {
            return this.EntityContext.GetNewEntity<TEntity>();
        }

        #region IEntityContextUser Members

        IEntityContext IEntityContextUser.EntityContext
        {
            get
            {
                this.ThrowIfContextIsDisposed();
                return this.EntityContext;
            }
        }

        protected void ThrowIfContextIsDisposed()
        {
            TEntityContext ctx = _entityContext;

            if (ctx == null || ctx.IsDisposed)
                throw new ObjectDisposedException("EntityContext is already disposed");
        }

        TEntityContext IEntityContextUser<TEntityContext>.EntityContext
        {
            get
            {
                this.ThrowIfContextIsDisposed();
                return this.EntityContext;
            }
        }

        #endregion
    }
}
