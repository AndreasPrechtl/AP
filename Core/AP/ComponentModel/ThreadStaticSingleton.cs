using System;

namespace AP.ComponentModel;

public abstract class ThreadStaticSingleton<TSingleton> : FinalizableObject
    where TSingleton : ThreadStaticSingleton<TSingleton>
{
    public static readonly object SyncRoot = new();

    [ThreadStatic]
    private static volatile TSingleton? s_current;

    protected ThreadStaticSingleton()
    {
        if (!TrySetInstance(out _))
            throw new InvalidOperationException("MultiSingleton");
    }

    public static void Release() => s_current?.TryDispose();

    private static bool TrySetInstance(out TSingleton instance)
    {
        var existingInstance = s_current;
        if (existingInstance is not null)
        {
            instance = existingInstance;
            return false;
        }
        lock (SyncRoot)
        {
            if (s_current is null)
            {
                s_current = instance = New.Instance<TSingleton>();
                return true;
            }
            else
            {
                instance = s_current!;
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
            if (s_current == this)
                s_current = null;
        }
    }
}