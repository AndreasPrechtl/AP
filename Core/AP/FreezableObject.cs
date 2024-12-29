namespace AP;

/// <summary>
/// Base class for freezable objects
/// </summary>
public abstract class FreezableObject : IFreezable
{
    private bool _isFrozen;

    /// <summary>
    /// Creates a new FreezableObject
    /// </summary>
    /// <param name="isReadOnly">Indicates if the object should be frozen directly</param>
    protected FreezableObject(bool isFrozen = false)
    {
        _isFrozen = isFrozen;
    }

    #region IFreezable Members

    /// <summary>
    /// Gets the current status or freezes the object;
    /// once it is frozen the setter no longer works
    /// </summary>
    /// <exception cref="System.InvalidOperationException" />
    public virtual bool IsFrozen
    {
        get => _isFrozen;
        set
        {
            this.AssertCanWrite();
            _isFrozen = value;
        }
    }

    /// <summary>
    /// Throws a System.InvalidOperationException if the object is frozen and therefore readonly
    /// </summary>        
    protected virtual void AssertCanWrite() => FreezableHelper.AssertCanWrite(this);

    #endregion
}
