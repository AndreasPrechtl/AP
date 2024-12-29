using System;
using System.Collections.Generic;
using System.Linq;

namespace AP.Data;

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
        ArgumentNullException.ThrowIfNull(entitySet);

        _inner = entitySet;
    }

    #region IQuery<TEntity> Members

    public IQueryable<TEntity> Query() => _inner.Query();

    #endregion

    #region ICreate<TEntity> Members

    public virtual void Create(params IEnumerable<TEntity> entities) => _inner.Create(entities);

    #endregion

    #region IUpdate<TEntity> Members
        
    public virtual void Update(params IEnumerable<TEntity> entities) => _inner.Update(entities);
    
    #endregion

    #region IDelete<TEntity> Members

    public virtual void Delete(params IEnumerable<TEntity> entities) => _inner.Delete(entities);

    #endregion

    #region IEntityFactory<TEntity> Members

    public virtual TEntity GetNewEntity() => _inner.GetNewEntity();

    #endregion
}
