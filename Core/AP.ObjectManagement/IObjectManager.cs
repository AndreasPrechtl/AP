using System.Collections.Generic;

namespace AP.ComponentModel.ObjectManagement;

//public delegate void ScopeCreated(object sender, EventArgs e);
//public delegate void ScopeDisposed(object sender, EventArgs e);

public interface IObjectManager : AP.IDisposable
{
    ObjectLifetimeBase<TBase> Register<TBase>(ObjectLifetimeBase<TBase> lifetime, bool disposeOnRelease = true);
    ObjectLifetimeBase<TBase> GetLifetime<TBase>(object? key = null);        
    bool Contains<TBase>(object? key = null);
    bool TryGetLifetime<TBase>(out ObjectLifetimeBase<TBase> lifetime, object? key = null);        
    void Release<TBase>(object? key = null);        
    void Clear();

    ManagedInstance<TBase> GetInstance<TBase>(object? key = null);
    bool TryGetInstance<TBase>(out ManagedInstance<TBase> instance, object? key = null);

    IEnumerable<ObjectLifetimeBase<TBase>> GetLifetimes<TBase>();
    IEnumerable<ManagedInstance<TBase>> GetInstances<TBase>();

    IObjectManagementScope GetScope();

    //event ScopeCreated ScopeCreated;
    //event ScopeDisposed ScopeDisposed;
}

public interface IObjectManagementScope : System.IDisposable
{
    ManagedInstance<TBase> GetInstance<TBase>(object? key = null);
    IEnumerable<ManagedInstance<TBase>> GetInstances<TBase>();
    bool TryGetInstance<TBase>(out ManagedInstance<TBase> instance, object? key = null);
}

public interface IObjectManager<TSuper> : AP.IDisposable
{
    ObjectLifetimeBase<TBase> Register<TBase>(ObjectLifetimeBase<TBase> lifetime, bool disposeOnRelease = true) where TBase : TSuper;
    bool Contains<TBase>(object? key = null) where TBase : TSuper;        
    bool TryGetObjectLifetime<TBase>(out ObjectLifetimeBase<TBase> lifetime, object? key = null) where TBase : TSuper;
    ObjectLifetimeBase<TBase> GetLifetime<TBase>(object? key = null) where TBase : TSuper;
    void Release<TBase>(object? key = null) where TBase : TSuper;
    void Clear();

    ManagedInstance<TBase> GetInstance<TBase>(object? key = null) where TBase : TSuper;
    bool TryGetInstance<TBase>(out ManagedInstance<TBase> instance, object? key = null) where TBase : TSuper;

    IEnumerable<ObjectLifetimeBase<TBase>> GetLifetimes<TBase>() where TBase : TSuper;
    IEnumerable<ManagedInstance<TBase>> GetInstances<TBase>() where TBase : TSuper;

    IObjectManagementScope<TSuper> GetScope();

    //event ScopeCreated ScopeCreated;
    //event ScopeDisposed ScopeDisposed;
}

public interface IObjectManagementScope<TSuper> : System.IDisposable
{
    ManagedInstance<TBase> GetInstance<TBase>(object? key = null) where TBase : TSuper;
    IEnumerable<ManagedInstance<TBase>> GetInstances<TBase>() where TBase : TSuper;
    bool TryGetInstance<TBase>(out ManagedInstance<TBase> instance, object? key = null) where TBase : TSuper;
}
