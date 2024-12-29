using System.Collections.Generic;

namespace AP.ComponentModel.ObjectManagement;

//public delegate void ScopeCreated(object sender, EventArgs e);
//public delegate void ScopeDisposed(object sender, EventArgs e);

public interface IObjectManager : AP.IDisposable
{
    ObjectLifetimeBase<TBase> Register<TBase>(ObjectLifetimeBase<TBase> lifetime, bool disposeOnRelease = true) where TBase : notnull;
    ObjectLifetimeBase<TBase> GetLifetime<TBase>(object? key = null) where TBase : notnull;        
    bool Contains<TBase>(object? key = null) where TBase : notnull;
    bool TryGetLifetime<TBase>(out ObjectLifetimeBase<TBase>? lifetime, object? key = null) where TBase : notnull;        
    void Release<TBase>(object? key = null) where TBase : notnull;        
    void Clear();

    ManagedInstance<TBase> GetInstance<TBase>(object? key = null) where TBase : notnull;
    bool TryGetInstance<TBase>(out ManagedInstance<TBase>? instance, object? key = null) where TBase : notnull;

    IEnumerable<ObjectLifetimeBase<TBase>> GetLifetimes<TBase>() where TBase : notnull;
    IEnumerable<ManagedInstance<TBase>> GetInstances<TBase>() where TBase : notnull;

    IObjectManagementScope GetScope();

    //event ScopeCreated ScopeCreated;
    //event ScopeDisposed ScopeDisposed;
}

public interface IObjectManagementScope : System.IDisposable
{
    ManagedInstance<TBase> GetInstance<TBase>(object? key = null) where TBase : notnull;
    IEnumerable<ManagedInstance<TBase>> GetInstances<TBase>() where TBase : notnull;
    bool TryGetInstance<TBase>(out ManagedInstance<TBase>? instance, object? key = null) where TBase : notnull;
}

public interface IObjectManager<TSuper> : AP.IDisposable
    where TSuper : notnull
{
    ObjectLifetimeBase<TBase> Register<TBase>(ObjectLifetimeBase<TBase> lifetime, bool disposeOnRelease = true) where TBase : TSuper;
    bool Contains<TBase>(object? key = null) where TBase : TSuper;        
    bool TryGetObjectLifetime<TBase>(out ObjectLifetimeBase<TBase>? lifetime, object? key = null) where TBase : TSuper;
    ObjectLifetimeBase<TBase> GetLifetime<TBase>(object? key = null) where TBase : TSuper;
    void Release<TBase>(object? key = null) where TBase : TSuper;
    void Clear();

    ManagedInstance<TBase> GetInstance<TBase>(object? key = null) where TBase : TSuper;
    bool TryGetInstance<TBase>(out ManagedInstance<TBase>? instance, object? key = null) where TBase : TSuper;

    IEnumerable<ObjectLifetimeBase<TBase>> GetLifetimes<TBase>() where TBase : TSuper;
    IEnumerable<ManagedInstance<TBase>> GetInstances<TBase>() where TBase : TSuper;

    IObjectManagementScope<TSuper> GetScope();

    //event ScopeCreated ScopeCreated;
    //event ScopeDisposed ScopeDisposed;
}

public interface IObjectManagementScope<TSuper> : System.IDisposable
    where TSuper : notnull
{
    ManagedInstance<TBase>? GetInstance<TBase>(object? key = null) where TBase : TSuper;
    IEnumerable<ManagedInstance<TBase>?> GetInstances<TBase>() where TBase : TSuper;
    bool TryGetInstance<TBase>(out ManagedInstance<TBase>? instance, object? key = null) where TBase : TSuper;
}
