using System;
using System.Collections.Generic;

namespace AP.ComponentModel.ObjectManagement;

public sealed partial class ObjectManager : FinalizableObject, IObjectManager, IObjectManagerInternal
{     
    private readonly Dictionary<Type, Group> _map = [];        
    public readonly object SyncRoot = new();
        
    public ObjectManager()
        : this(null)
    { }
    
    internal ObjectManager(IObjectManagerInternal? parent)
    {
        if (parent == null && !ManagedObjects.HasManager)
            ManagedObjects.Manager = this;
    }
    
    public ObjectLifetimeBase<TBase> Register<TBase>(ObjectLifetimeBase<TBase> lifetime, bool disposeOnRelease = true)
        where TBase : notnull
    {
        base.ThrowIfDisposed();

        Type groupKey = typeof(TBase);

        if (_map.TryGetValue(groupKey, out var group))
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

    public ManagedInstance<TBase> GetInstance<TBase>(object? key = null)
        where TBase : notnull 
        => this.GetLifetime<TBase>(key).Instance;

    public IEnumerable<ObjectLifetimeBase<TBase>> GetLifetimes<TBase>()
        where TBase : notnull
    {
        base.ThrowIfDisposed();

        Type type = typeof(TBase);

        if (_map.TryGetValue(type, out var group))
            foreach (Item item in group)
                yield return (ObjectLifetimeBase<TBase>)item.Lifetime;
        else
            yield break;
    }

    public IEnumerable<ManagedInstance<TBase>> GetInstances<TBase>()
        where TBase : notnull
    {
        foreach (ObjectLifetimeBase<TBase> lifetime in this.GetLifetimes<TBase>())
            yield return lifetime.Instance;
    }

    public ObjectLifetimeBase<TBase> GetLifetime<TBase>(object? key = null)
        where TBase : notnull
    {
        base.ThrowIfDisposed();

        return (ObjectLifetimeBase<TBase>)_map[typeof(TBase)].Get(key);
    }

    public bool TryGetInstance<TBase>(out ManagedInstance<TBase>? instance, object? key = null)
        where TBase : notnull
    {
        base.ThrowIfDisposed();

        instance = null;
        bool r = false;

        if (r = this.TryGetLifetime<TBase>(out var lifetime, key))
            instance = lifetime!.Instance;

        return r;
    }

    public bool TryGetLifetime<TBase>(out ObjectLifetimeBase<TBase>? lifetime, object? key = null)
        where TBase : notnull
    {
        base.ThrowIfDisposed();

        lifetime = null;
        bool r = false;

        if (r = _map.TryGetValue(typeof(TBase), out var group))
        {
            if (r = group!.TryGetValue(out var lt, key))
                lifetime = (ObjectLifetimeBase<TBase>)lt!;
        }

        return r;
    }

    public bool Contains<TBase>(object? key = null)
        where TBase : notnull
    {
        base.ThrowIfDisposed();

        return (_map.TryGetValue(typeof(TBase), out var group) && group!.Contains(key));
    }

    public void Release<TBase>(object? key = null)
        where TBase : notnull
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

    public Scope GetScope() => new(this);

    IObjectManagementScope IObjectManager.GetScope() => this.GetScope();

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
