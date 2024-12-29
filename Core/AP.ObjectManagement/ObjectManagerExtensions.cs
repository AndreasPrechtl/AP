namespace AP.ComponentModel.ObjectManagement;

public static class ObjectManagerExtensions
{
    //public static TBase GetInstance<TBase>(this IObjectManager manager, object key = null)
    //{
    //    TBase instance = default(TBase);
    //    manager.TryGetInstance<TBase>(out instance, key);

    //    return instance;
    //}

    //public static bool TryGetInstance<TBase>(this IObjectManager manager, out TBase instance, object key = null)
    //{
    //    ObjectLifetimeBase<TBase> lifetime = null;

    //    if (manager.TryGetObjectLifetime(out lifetime, key))
    //    {
    //        instance = lifetime.Instance;
    //        return true;
    //    }

    //    instance = default(TBase);
    //    return false;
    //}

    public static void Release<TBase>(this IObjectManager manager, ObjectLifetimeBase<TBase> lifetime)
        where TBase : notnull
        => manager.Release<TBase>(lifetime.Key);

    //public static TBase GetInstance<TSuper, TBase>(this IObjectManager<TSuper> manager, object key = null)
    //    where TBase : TSuper
    //{
    //    TBase instance = default(TBase);
    //    manager.TryGetInstance<TSuper, TBase>(out instance, key);

    //    return instance;
    //}

    //public static bool TryGetInstance<TSuper, TBase>(this IObjectManager<TSuper> manager, out TBase instance, object key = null)
    //    where TBase : TSuper
    //{
    //    ObjectLifetimeBase<TBase> lifetime = null;

    //    if (manager.TryGetObjectLifetime(out lifetime, key))
    //    {
    //        instance = lifetime.Instance;
    //        return true;
    //    }

    //    instance = default(TBase);
    //    return false;
    //}

    public static void Release<TSuper, TBase>(this IObjectManager<TSuper> manager, ObjectLifetimeBase<TBase> lifetime)
        where TSuper : notnull
        where TBase : TSuper => 
        manager.Release<TBase>(lifetime.Key);
}
