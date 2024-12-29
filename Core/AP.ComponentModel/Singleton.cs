using System;

namespace AP.ComponentModel;

public abstract class Singleton<TSingleton> : FinalizableObject
    where TSingleton : notnull, Singleton<TSingleton>
{
    public static readonly object SyncRoot = new();

    private static volatile TSingleton? s_instance;

    protected Singleton()
    {
        if (!TrySetInstance(out _))
            throw new InvalidOperationException("MultiSingleton");
    }

    public static void Release() => s_instance?.TryDispose();

    private static bool TrySetInstance(out TSingleton instance)
    {
        var existingInstance = s_instance;
        if (existingInstance is not null)
        {
            instance = existingInstance;
            return false;
        }
        lock (SyncRoot)
        {
            if (s_instance is null)
            {
                s_instance = instance = New.Instance<TSingleton>();
                return true;
            }
            else
            {
                instance = s_instance!;
                return false;
            }
        }
    }

    public static TSingleton Instance
    {
        get
        {
            TrySetInstance(out var instance);
            return instance!;
        }
    }

    protected override void CleanUpResources()
    {
        base.CleanUpResources();

        lock (SyncRoot)
        {
            if (s_instance == this)
                s_instance = null;
        }
    }
}