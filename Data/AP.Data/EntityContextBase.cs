using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AP.Data;

// todo: rework change tracking/saving

/// <summary>
/// Base class for a DataContext - it is also a FinalizableObject to ensure timely resource cleanup.
/// </summary>
public abstract class EntityContextBase : FinalizableObject, IEntityContext
{
    protected readonly CancellationTokenSource cancellationTokenSource = new();

    private class ChangeEntry()
    {
        public object Entity { get; set; }
        public Action Action { get; set; }
    }

    private readonly AP.Collections.List<ChangeEntry> _changes = [];
    

    protected EntityContextBase(object? contextKey = null)
        : base(contextKey)
    { }

    #region IEntityContext Members
    
    public async Task Save(CancellationToken cancellationToken = default)
    {
        this.ThrowIfDisposed();
        await this.OnSave(cancellationToken);
    }

    protected abstract Task OnSave(CancellationToken cancellationToken);

    public void Discard()
    {
        this.ThrowIfDisposed();
        this.OnDiscard();
    }

    protected abstract void OnDiscard();

    #endregion

    #region Helper Methods
    
    private void RegisterChangeInternal<TEntity>(TEntity entity, bool isBatchPart, Action<TEntity> action) where TEntity : class
    {
        _changes.Add(new() { entity = entity, Action = action(entity) });
    }

    #endregion
    
    #region ICreate Members

    protected abstract void OnCreation<TEntity>(TEntity entity) where TEntity : class;
 
    internal void RegisterForCreation<TEntity>(TEntity entity, bool isBatchPart = false) where TEntity : class
    {
        this.ThrowIfDisposed();
        
        this.RegisterChangeInternal(entity, isBatchPart, this.OnCreation<TEntity>);
    }
    
    #endregion

    #region IUpdate Members

    internal void RegisterForUpdate<TEntity>(TEntity entity, bool isBatchPart = false) where TEntity : class
    {
        this.ThrowIfDisposed();
        this.RegisterChangeInternal(entity, isBatchPart, this.OnRegisterForUpdate<TEntity>);
    }

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

        base.CleanUpResources();
    }

    #endregion
}
