using System;
using System.Linq;

namespace AP.Data;

/// <summary>
/// Base class for a DataContext - it is also a FinalizableObject to ensure timely resource cleanup.
/// </summary>
public abstract class EntityContextBase : FinalizableObject, IEntityContext
{
    private readonly SaveMode _saveMode;

    protected EntityContextBase(SaveMode saveMode = SaveMode.Default, object? contextKey = null)
        : base(contextKey)
    {
        _saveMode = saveMode;
    }

    #region IEntityContext Members
    
    public void Save()
    {
        this.ThrowIfDisposed();
        this.OnSave();
    }

    protected abstract void OnSave();

    public void Discard()
    {
        this.ThrowIfDisposed();
        this.OnDiscard();
    }

    protected abstract void OnDiscard();

    public SaveMode SaveMode
    {
        get
        {
            this.ThrowIfDisposed();
            return _saveMode;
        }
    }

    #endregion

    #region Helper Methods
    
    //private void RegisterChangeInternal<TEntity>(IEnumerable<TEntity> entities, Action<TEntity> action)
    //{
    //    if ((_saveMode & Data.SaveMode.Batch) == Data.SaveMode.Batch)
    //    {
    //        foreach (TEntity entity in entities)
    //            action(entity);

    //        this.Save();

    //        return;
    //    }

    //    if ((_saveMode & Data.SaveMode.Entry) == Data.SaveMode.Entry)
    //    {
    //        foreach (TEntity entity in entities)
    //        {
    //            action(entity);
    //            this.Save();
    //        }
    //        return;
    //    }

    //    foreach (TEntity entity in entities)
    //        action(entity);
    //}

    private void RegisterChangeInternal<TEntity>(TEntity entity, bool isBatchPart, Action<TEntity> action) where TEntity : class
    {
        action(entity);

        if ((_saveMode & Data.SaveMode.Entry) == Data.SaveMode.Entry || (!isBatchPart && (_saveMode & Data.SaveMode.Batch) == Data.SaveMode.Batch))
            this.Save();
    }

    #endregion
    
    #region ICreate Members

    protected abstract void OnCreation<TEntity>(TEntity entity) where TEntity : class;
 
    internal void RegisterForCreation<TEntity>(TEntity entity, bool isBatchPart = false) where TEntity : class
    {
        this.ThrowIfDisposed();
        
        this.RegisterChangeInternal(entity, isBatchPart, this.OnCreation<TEntity>);
    }
    
    //internal void RegisterForCreation<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    //{
    //    this.ThrowIfDisposed();
    //    this.RegisterChangeInternal(entities, this.OnCreation<TEntity>);
    //}

    #endregion

    #region IUpdate Members

    internal void RegisterForUpdate<TEntity>(TEntity entity, bool isBatchPart = false) where TEntity : class
    {
        this.ThrowIfDisposed();
        this.RegisterChangeInternal(entity, isBatchPart, this.OnRegisterForUpdate<TEntity>);
    }

    //internal void RegisterForUpdate<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    //{
    //    this.ThrowIfDisposed();
    //    this.RegisterChangeInternal(entities, this.OnRegisterForUpdate<TEntity>);
    //}

    //internal void RegisterForUpdate<TEntity>(Expression<Predicate<TEntity>> where, Expression<Action<TEntity>> action) where TEntity : class
    //{
    //    this.ThrowIfDisposed();
    //    this.OnRegisterForUpdate<TEntity>(where, action);
    //}

    //protected virtual void OnRegisterForUpdate<TEntity>(Expression<Predicate<TEntity>> where, Expression<Action<TEntity>> action) where TEntity : class
    //{
    //    Expression<Func<TEntity, bool>> w = where.Cast<Func<TEntity, bool>>();

    //    IQueryable<TEntity> entities = this.OnQuery<TEntity>().Where(w);

    //    Action<TEntity> a = action.Compile();

    //    // quick fix so I don't have to re-implement all the logic for this one
    //    Action<TEntity> changer = delegate(TEntity p)
    //    {
    //        a(p);
    //        this.OnRegisterForUpdate(p);
    //    };
        
    //    this.RegisterChangeInternal<TEntity>(entities, changer);
    //}

    protected abstract void OnRegisterForUpdate<TEntity>(TEntity entity) where TEntity : class;
    
    #endregion

    #region IDelete Members

    internal void RegisterForDeletion<TEntity>(TEntity entity, bool isBatchPart = false) where TEntity : class
    {
        this.ThrowIfDisposed();
        this.RegisterChangeInternal(entity, isBatchPart, this.OnRegisterForDeletion<TEntity>);
    }

    //internal void RegisterForDeletion<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    //{
    //    this.ThrowIfDisposed();
    //    this.RegisterChangeInternal(entities, this.OnRegisterForDeletion<TEntity>);
    //}

    //internal void RegisterForDeletion<TEntity>(Expression<Predicate<TEntity>> where)
    //{
    //    this.ThrowIfDisposed();
    //    this.OnRegisterForDeletion<TEntity>(where);
    //}
    
    //protected virtual void OnRegisterForDeletion<TEntity>(Expression<Predicate<TEntity>> where)
    //{
    //    this.RegisterChangeInternal<TEntity>(this.OnQuery<TEntity>().Where(where.Cast<Func<TEntity, bool>>()), this.OnRegisterForDeletion<TEntity>);
    //}

    protected abstract void OnRegisterForDeletion<TEntity>(TEntity entity) where TEntity : class;

    #endregion

    #region IQuery Members

    internal IQueryable<TEntity> Query<TEntity>() where TEntity : class
    {
        this.ThrowIfDisposed();
        return this.OnQuery<TEntity>();
    }

    protected abstract IQueryable<TEntity> OnQuery<TEntity>() where TEntity : class;

    #endregion

    #region IEntityModelProvider Members

    private IEntityModel _model;

    public IEntityModel GetEntityModel()
    {
        this.ThrowIfDisposed();
        IEntityModel model = _model;

        if (model == null)
            _model = model = this.CreateEntityModel();

        return model;
    }

    protected abstract IEntityModel CreateEntityModel();

    #endregion

    #region IEntitySetProvider Members

    private readonly object _syncRoot = new();
    private readonly AP.Collections.Dictionary<Type, object> _entitySets = new();
    
    public IEntitySet<TEntity> GetEntitySet<TEntity>() where TEntity : class
    {
        this.ThrowIfDisposed();
        
        lock (_syncRoot)
        {
            Type type = typeof(IEntitySet<TEntity>);

            if (_entitySets.Contains(type, out object es))
                return (IEntitySet<TEntity>)es;

            es = this.CreateEntitySet<TEntity>();
            _entitySets.Add(type, es);

            return (IEntitySet<TEntity>)es;
        }
    }

    protected abstract IEntitySet<TEntity> CreateEntitySet<TEntity>() where TEntity : class;
    
    #endregion

    #region IEntityFactory Members

    public TEntity GetNewEntity<TEntity>() where TEntity : class
    {
        this.ThrowIfDisposed();
        return this.OnGetNewEntity<TEntity>();
    }

    protected virtual TEntity OnGetNewEntity<TEntity>() => AP.New.Instance<TEntity>();

    #endregion

    #region IDisposable Members

    protected override void CleanUpResources()
    {
        if ((_saveMode & Data.SaveMode.Disposal) == Data.SaveMode.Disposal)
            this.Save();
        
        //else // rollback or simply do nothing? - well this can be customized by overriding this method
        //    this.OnDiscard();

        base.CleanUpResources();
    }

    #endregion
}
