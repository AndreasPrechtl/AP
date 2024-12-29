using System.Collections;

namespace AP.ComponentModel;

/// <summary>
/// An interface for factories.
/// </summary>
public interface IActivator
{
    /// <summary>
    /// Creates a new instance of the given type.
    /// </summary>
    /// <typeparam name="T">The to be created.</typeparam>
    /// <param name="args">The arguments.</param>
    /// <returns>A new instance of "T"</returns>
    T New<T>(params IEnumerable[] args) 
        where T : notnull
        => AP.New.Instance<T>(args);
}

/// <summary>
/// An interface for generic factories.
/// </summary>
/// <typeparam name="TBase"></typeparam>
public interface IActivator<in TBase>
{
    /// <summary>
    /// Creates a new instance of the given type.
    /// </summary>
    /// <typeparam name="T">The to be created.</typeparam>
    /// <param name="args">The arguments.</param>
    /// <returns>A new instance of "T"</returns>
    T New<T>(params object[] args) 
        where T : notnull, TBase
        => AP.New.Instance<T>(args);
}
