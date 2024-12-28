using System;
using AP.ComponentModel;

namespace AP.Configuration
{
    /// <summary>
    /// Interface for providers.
    /// </summary>
    public interface IProvider : AP.IDisposable
    {
        string Name { get; }
    }
}
