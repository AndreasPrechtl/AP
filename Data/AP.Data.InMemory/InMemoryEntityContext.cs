using System;
using System.Linq;
using System.Text;
using AP.Collections;
using AP.Data;

namespace AP.Data.InMemory
{
    [Flags]
    public enum DataOperation
    {
        None = 0,
        Create = 1,
        Update = 2,
        Delete = 4,
        Query = 8,
        Save = 16,
        Discard = 32,
        All = Create | Update | Query | Delete | Save | Discard
    }
 
    /// <summary>
    /// Represents an entity context in memory without persistance.
    /// Extend this class to add your own persistance layer.
    /// </summary>
    public class InMemoryEntityContext : EntityContextBase
    {
        private AP.Collections.Dictionary<object, DataOperation> _pendingChanges = new AP.Collections.Dictionary<object, DataOperation>();
        private Set<object> _data = new Set<object>();

        private readonly DataOperation _operations;

        public InMemoryEntityContext(DataOperation operations = DataOperation.All)
        {
            _operations = operations;
        }

        public void ThrowIfOperationDisallowed(DataOperation operation)
        {
            this.ThrowIfDisposed();
            if ((_operations & operation) != operation)
                throw new InvalidOperationException("Operation not allowed");
        }

        protected override void OnSave()
        {
            this.ThrowIfOperationDisallowed(DataOperation.Save);
            
            foreach (var kvp in _pendingChanges)
            {
                if (kvp.Value != DataOperation.Delete)
                    _data.Add(kvp.Key);
                else
                    _data.Remove(kvp.Key);
            }

            _pendingChanges.Clear();
        }

        protected override void OnDiscard()
        {
            this.ThrowIfOperationDisallowed(DataOperation.Discard);

            // this will most likely screw up becuz it's not persisted
            _pendingChanges.Clear();
        }

        protected override void OnCreation<TEntity>(TEntity entity)
        {
            this.ThrowIfOperationDisallowed(DataOperation.Create);

            if (_pendingChanges.ContainsKey(entity))
                _pendingChanges[entity] = DataOperation.Create;
            else
                _pendingChanges.Add(entity, DataOperation.Create);
        }

        protected override void OnRegisterForUpdate<TEntity>(TEntity entity)
        {
            this.ThrowIfOperationDisallowed(DataOperation.Update);

            if (_pendingChanges.ContainsKey(entity))
                _pendingChanges[entity] = DataOperation.Update;
            else
                _pendingChanges.Add(entity, DataOperation.Update);
        }

        protected override void OnRegisterForDeletion<TEntity>(TEntity entity)
        {
            this.ThrowIfOperationDisallowed(DataOperation.Delete);
            
            if (_pendingChanges.ContainsKey(entity))
                _pendingChanges[entity] = DataOperation.Delete;
            else
                _pendingChanges.Add(entity, DataOperation.Delete);
        }

        protected override IQueryable<TEntity> OnQuery<TEntity>()
        {
            this.ThrowIfOperationDisallowed(DataOperation.Query);
            
            return _data.OfType<TEntity>().AsQueryable();
        }

        protected override IEntityModel CreateEntityModel()
        {
            return new EntityModel<InMemoryEntityContext>(this);
        }

        protected override IEntitySet<TEntity> CreateEntitySet<TEntity>()
        {
            return new EntitySet<InMemoryEntityContext, TEntity>(this);
        }
        
        protected override void CleanUpResources()
        {
            base.CleanUpResources();
            _pendingChanges = null;
            _data = null;
        }
    }
}
