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
    private object _security;
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
    public virtual object Security
    {
        get => _security;
        set => _security = value;
    }

    /// <summary>
    /// Gets or sets the file or directory attribues.
    /// </summary>
    public FileSystemEntryAttributes Attributes
    {
        get => this.AttributesInternal;
        set => this.AttributesInternal = value;
    }

    private DateTimeOffset _dateAccessed, _dateModified, _dateCreated;

    /// <summary>
    /// Gets or sets the last access date/time.
    /// </summary>
    public DateTimeOffset DateAccessed
    {
        get => _dateAccessed;
        set =>
            //if (value < _dateCreated)
            //    throw new ArgumentOutOfRangeException("value");

            _dateAccessed = value;
    }

    /// <summary>
    /// Gets or sets the creation date/time.
    /// </summary>
    public DateTimeOffset DateCreated
    {
        get => _dateCreated;
        set =>
            //if (value > _dateModified || value > _dateAccessed)
            //    throw new ArgumentOutOfRangeException("value");

            _dateCreated = value;
    }

    /// <summary>
    /// Gets or sets the last write date/time.
    /// </summary>
    public DateTimeOffset DateModified
    {
        get => _dateModified;
        set =>
            //if (value < _dateCreated)
            //    throw new ArgumentOutOfRangeException("value");

            _dateModified = value;
    }

    #endregion

    internal virtual FileSystemEntryAttributes AttributesInternal
    {
        get => _attributes;
        set => _attributes = value;
    }
}
