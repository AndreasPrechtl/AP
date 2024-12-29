using System;

namespace AP.Configuration;

/// <summary>
/// Interface for providers using file access.
/// </summary>
public interface IFileBasedProvider : IProvider
{
    string FileName { get; }
    DateTimeOffset? LastReadDate { get; }
}
