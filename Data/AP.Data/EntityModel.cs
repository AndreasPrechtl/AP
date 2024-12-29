using System;
using System.Collections.Generic;
using System.Linq;

namespace AP.Data;

/// <summary>
/// Class for an EntityModel with an external context
/// </summary>
/// <typeparam name="TEntityContext">The external context.</typeparam>
public class EntityModel<TEntityContext> : IEntityModel, IEntityContextUser<TEntityContext>
    where TEntityContext : EntityContextBase
{
    private TEntityContext _entityContext;

    protected TEntityContext EntityContext => _entityContext;

    public EntityModel(TEntityContext entityContext)
    {
        ArgumentNullException.ThrowIfNull(entityContext);

        _entityContext = entityContext;
        entityContext.Disposed += EntityContextDisposed;
    }

    private void EntityContextDisposed(object sender, EventArgs e)
    {
        TEntityContext ctx = _entityContext;
        if (ctx != null)
        {
            ctx.Disposed -= this.EntityContextDisposed;
            _entityContext = null!;
        }
    }

    public IQueryable<TEntity> Query<TEntity>() 
        where TEntity : class
        => this.GetEntitySet<TEntity>().Query();

    public void Create<TEntity>(params IEnumerable<TEntity> entities)
        where TEntity : class
        => this.GetEntitySet<TEntity>().Create(entities);
        
    public void Update<TEntity>(params IEnumerable<TEntity> entities) 
        where TEntity : class
        => this.GetEntitySet<TEntity>().Update(entities);
        
    public void Delete<TEntity>(params IEnumerable<TEntity> entities) 
        where TEntity : class
        => this.GetEntitySet<TEntity>().Delete(entities);

    public IEntitySet<TEntity> GetEntitySet<TEntity>() 
        where TEntity : class
        => this.EntityContext.GetEntitySet<TEntity>();

    public TEntity GetNewEntity<TEntity>() 
        where TEntity : class
        => this.EntityContext.GetNewEntity<TEntity>();

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
