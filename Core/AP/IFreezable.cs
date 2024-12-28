namespace AP;

/// <summary>
/// Interface for objects that can be frozen
/// </summary>
public interface IFreezable // : IReadOnly
{
    /// <summary>
    /// Gets or sets the status of the freezable object
    /// </summary>
    bool IsFrozen { get; set; }        
}
