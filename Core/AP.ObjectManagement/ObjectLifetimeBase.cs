namespace AP.ComponentModel.ObjectManagement;

public abstract class ObjectLifetimeBase<TBase> : DisposableObject, IObjectLifetimeInternal
{
    private readonly object _key;

    protected ObjectLifetimeBase(object? key = null)
    {
        _key = key;
    }

    public abstract ManagedInstance<TBase> Instance { get; }

    #region IObjectLifetimeInternal Members

    public object Key => _key;

    //object IObjectLifetimeInternal.Instance
    //{
    //    get { return this.Instance; }
    //}

    #endregion
}
