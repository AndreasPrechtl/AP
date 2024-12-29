using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AP.Linq;

namespace AP.Data;

/// <summary>
/// EntitySet using an external context.
/// </summary>
/// <typeparam name="TEntityContext">The context.</typeparam>
/// <typeparam name="TEntity">The entity type.</typeparam>
public class EntitySet<TEntityContext, TEntity> : IEntitySet<TEntity>, IEntityContextUser<TEntityContext>
    where TEntityContext : EntityContextBase
    where TEntity : class
{
    private TEntityContext _entityContext;
    protected TEntityContext EntityContext => _entityContext;

    public EntitySet(TEntityContext entityContext)
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

    protected void ThrowIfContextIsDisposed()
    {
        TEntityContext ctx = _entityContext;

        if (ctx == null || ctx.IsDisposed)
            throw new ObjectDisposedException("EntityContext is already disposed");
    }


    #region IQuery<TEntity> Members

    public virtual IQueryable<TEntity> Query()
    {
        this.ThrowIfContextIsDisposed();
        return this.OnQuery();
    }

    protected virtual IQueryable<TEntity> OnQuery() => this.EntityContext.Query<TEntity>();

    #endregion

    #region ICreate<TEntity> Members

    public void Create(TEntity entity)
    {
        this.ThrowIfContextIsDisposed();
        this.OnCreate(entity);
        this.EntityContext.RegisterForCreation<TEntity>(entity);
    }

    public void Create(IEnumerable<TEntity> entities)
    {
        this.ThrowIfContextIsDisposed();
        foreach (TEntity entity in entities)
        {
            this.OnCreate(entity);
            this.EntityContext.RegisterForCreation<TEntity>(entity);
        }
    }

    protected virtual void OnCreate(TEntity entity)
    { }

    #endregion

    #region IUpdate<TEntity> Members

    public void Update(TEntity entity)
    {
        this.ThrowIfContextIsDisposed();
        this.OnUpdate(entity);
        this.EntityContext.RegisterForUpdate<TEntity>(entity);
    }

    public void Update(IEnumerable<TEntity> entities)
    {
        this.ThrowIfContextIsDisposed();
        foreach (TEntity entity in entities)
        {
            this.OnUpdate(entity);
            this.EntityContext.RegisterForUpdate<TEntity>(entity);
        }
    }

    public AP.Collections.IListView<TEntity> Update(Expression<Predicate<TEntity>> where, Action<TEntity> action)
    {
        this.ThrowIfContextIsDisposed();
        var q = this.Query().Where(where.Cast<Func<TEntity, bool>>());
        
        AP.Collections.List<TEntity> updates = new();
        foreach (TEntity entity in q)
        {
            action(entity);
            this.OnUpdate(entity);
            this.EntityContext.RegisterForUpdate<TEntity>(entity);
            updates.Add(entity);
        }
        return updates;
    }

    protected virtual void OnUpdate(TEntity entity)
    { }

    #endregion

    #region IDelete<TEntity> Members

    public void Delete(TEntity entity)
    {
        this.ThrowIfContextIsDisposed();
        this.OnDelete(entity);
        this.EntityContext.RegisterForDeletion<TEntity>(entity);
    }

    public void Delete(IEnumerable<TEntity> entities)
    {
        this.ThrowIfContextIsDisposed();
        foreach (TEntity entity in entities)
        {
            this.OnDelete(entity);
            this.EntityContext.RegisterForDeletion<TEntity>(entity);
        }
    }

    public AP.Collections.IListView<TEntity> Delete(Expression<Predicate<TEntity>> where)
    {
        this.ThrowIfContextIsDisposed();
        var q = this.Query().Where(where.Cast<Func<TEntity, bool>>());

        AP.Collections.List<TEntity> deletes = new();
        foreach (TEntity entity in q)
        {
            this.OnDelete(entity);
            this.EntityContext.RegisterForDeletion<TEntity>(entity);
            deletes.Add(entity);
        }
        return deletes;
    }

    protected virtual void OnDelete(TEntity entity) 
    { }

    #endregion

    #region IEntityFactory<TEntity> Members

    public TEntity GetNewEntity()
    {
        this.ThrowIfContextIsDisposed();
        return _entityContext.GetNewEntity<TEntity>();
    }

    #endregion

    #region IEntityContextUser Members

    IEntityContext IEntityContextUser.EntityContext
    {
        get
        {
            this.ThrowIfContextIsDisposed();
            return this.EntityContext;
        }
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
