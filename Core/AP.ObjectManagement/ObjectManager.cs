using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections;
using System.Linq.Expressions;

namespace AP.ComponentModel.ObjectManagement
{
    public sealed partial class ObjectManager : FinalizableObject, IObjectManager, IObjectManagerInternal
    {     
        private readonly Dictionary<Type, Group> _map = new Dictionary<Type, Group>();        
        public readonly object SyncRoot = new object();
            
        public ObjectManager()
            : this(null)
        { }
        
        internal ObjectManager(IObjectManagerInternal parent)
        {
            if (parent == null && !ManagedObjects.HasManager)
                ManagedObjects.Manager = this;
        }
        
        public ObjectLifetimeBase<TBase> Register<TBase>(ObjectLifetimeBase<TBase> lifetime, bool disposeOnRelease = true)
        {
            base.ThrowIfDisposed();
            
            Group group = null;
            Type groupKey = typeof(TBase);

            if (_map.TryGetValue(groupKey, out group))
                group.Register(lifetime);
            else
            {
                group = new Group();
                group.Register(lifetime);

                lock (SyncRoot)
                    _map.Add(groupKey, group);
            }

            return lifetime;
        }

        ///// <summary>
        ///// Dirty little helper,
        ///// will only be uncommented for IoC performance programs that can't use the generic method directly.
        ///// - beware, it's fast :)
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public object GetInstance(Type type)
        //{
        //    return _map[type].Get(null).Instance;
        //}

        public ManagedInstance<TBase> GetInstance<TBase>(object key = null)
        {
            return this.GetLifetime<TBase>(key).Instance;
        }

        public IEnumerable<ObjectLifetimeBase<TBase>> GetLifetimes<TBase>()
        {
            base.ThrowIfDisposed();

            Type type = typeof(TBase);
            Group group = null;

            if (_map.TryGetValue(type, out group))
                foreach (Item item in group)
                    yield return (ObjectLifetimeBase<TBase>)item.Lifetime;
            else
                yield break;
        }

        public IEnumerable<ManagedInstance<TBase>> GetInstances<TBase>()
        {
            foreach (ObjectLifetimeBase<TBase> lifetime in this.GetLifetimes<TBase>())
                yield return lifetime.Instance;
        }

        public ObjectLifetimeBase<TBase> GetLifetime<TBase>(object key = null)
        {
            base.ThrowIfDisposed();

            //Group g = null;

            //if (_map.TryGetValue(typeof(TBase), out g))
              // return g.Get(key);
            
            // or throw an error?

            return (ObjectLifetimeBase<TBase>)_map[typeof(TBase)].Get(key);
        }

        public bool TryGetInstance<TBase>(out ManagedInstance<TBase> instance, object key = null)
        {
            base.ThrowIfDisposed();

            ObjectLifetimeBase<TBase> lifetime = null;
            instance = null;
            bool r = false;

            if (r = this.TryGetLifetime<TBase>(out lifetime, key))
                instance = lifetime.Instance;

            return r;
        }

        public bool TryGetLifetime<TBase>(out ObjectLifetimeBase<TBase> lifetime, object key = null)
        {
            base.ThrowIfDisposed();

            lifetime = null;
            Group group = null;
            bool r = false;

            if (r = _map.TryGetValue(typeof(TBase), out group))
            {
                IObjectLifetimeInternal lt = null;
                if (r = group.TryGetValue(out lt, key))
                    lifetime = (ObjectLifetimeBase<TBase>)lt;
            }

            return r;
        }

        public bool Contains<TBase>(object key = null)
        {
            base.ThrowIfDisposed();
            Group g = null;

            return (_map.TryGetValue(typeof(TBase), out g) && g.Contains(key));
        }

        public void Release<TBase>(object key = null)
        {
            this.ThrowIfDisposed();
            Type groupKey = typeof(TBase);
            Group group = _map[groupKey];

            group.Release(key);

            if (group.IsEmpty)
                lock (SyncRoot)
                    _map.Remove(groupKey);            
        }

        public void Clear()
        {
            base.ThrowIfDisposed();
            
            lock (SyncRoot)
            {
                foreach (var kvp in _map)
                {
                    Group group = kvp.Value;
                    group.Clear();
                }
                _map.Clear();
            }
        }

        #region IObjectManager Members

        public Scope GetScope()
        {
            return new Scope(this);
        }

        IObjectManagementScope IObjectManager.GetScope()
        {
            return this.GetScope();
        }

        #endregion

        #region IDisposable Members

        protected override void CleanUpResources()
        {
            base.CleanUpResources();

            if (ManagedObjects.HasManager && ManagedObjects.Manager == this)
                ManagedObjects.Dispose(false);

            this.Clear();         
        }

        #endregion
    }
}
