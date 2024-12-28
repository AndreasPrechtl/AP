using System;

namespace AP.IO.MetaData;

/// <summary>
/// Base interface for file or directory metadata.
/// </summary>
public interface IFileSystemEntryMetaData
{
    /// <summary>
    /// Gets the full name.
    /// </summary>
    string FullName { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets or sets the file or directory security.
    /// </summary>
    object Security { get; set; }

    /// <summary>
    /// Gets or sets the file or directory attributes.
    /// </summary>
    FileSystemEntryAttributes Attributes { get; set; }

    /// <summary>
    /// Gets or sets the last access date/time.
    /// </summary>
    DateTimeOffset DateAccessed { get; set; }

    /// <summary>
    /// Gets or sets the creation date/time.
    /// </summary>
    DateTimeOffset DateCreated { get; set; }

    /// <summary>
    /// Gets or sets the last write date/time.
    /// </summary>
    DateTimeOffset DateModified { get; set; }
}
