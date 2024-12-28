using AP.UniformIdentifiers;
using System;

namespace AP.IO.MetaData;

/// <summary>
/// Base class for file or directory metadata.
/// </summary>
[Serializable]    
public abstract class FileSystemEntryMetaDataBase : IFileSystemEntryMetaData
{
    protected readonly IHierarchicalUri _uri;
    private FileSystemEntryAttributes _attributes;

    internal FileSystemEntryMetaDataBase(IHierarchicalUri uri)
    {
        ArgumentNullException.ThrowIfNull(uri);
        _uri = uri;
    }

    #region IFSObjectMetaData Members

    /// <summary>
    /// Gets the full name.
    /// </summary>
    public string FullName => _uri.FullName;

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name => _uri.Name;

    /// <summary>
    /// Gets or sets the security.
    /// </summary>
    public virtual object? Security { get; init; }

    /// <summary>
    /// Gets or sets the file or directory attributes.
    /// </summary>
    public FileSystemEntryAttributes Attributes
    {
        get => this.AttributesInternal;
        init => this.AttributesInternal = value;
    }

    /// <summary>
    /// Gets or sets the last access date/time.
    /// </summary>
    public DateTimeOffset DateAccessed { get; init; }

    /// <summary>
    /// Gets or sets the creation date/time.
    /// </summary>
    public DateTimeOffset DateCreated { get; init; }

    /// <summary>
    /// Gets or sets the last write date/time.
    /// </summary>
    public DateTimeOffset DateModified { get; init; }

    #endregion

    internal virtual FileSystemEntryAttributes AttributesInternal
    {
        get => _attributes;
        init => _attributes = value;
    }
}
