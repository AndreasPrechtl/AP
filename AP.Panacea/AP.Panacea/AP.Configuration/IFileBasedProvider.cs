using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Configuration
{
    /// <summary>
    /// Interface for providers using file access.
    /// </summary>
    public interface IFileBasedProvider : IProvider
    {
        string FileName { get; }
        DateTimeOffset? LastReadDate { get; }
    }
}
