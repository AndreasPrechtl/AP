using AP.UniformIdentifiers;
using System;

namespace AP.IO.MetaData;

/// <summary>
/// Class for storing file information.
/// </summary>
[Serializable]
public class FileMetaData : FileSystemEntryMetaDataBase, IFileMetaData
{
    /// <summary>
    /// Creates a new instance of FileMetaData.
    /// </summary>
    /// <param name="uri">The uri.</param>
    public FileMetaData(IFileUri uri)
        : base(uri)
    { }

    /// <summary>
    /// Gets the extension.
    /// </summary>
    public string Extension => ((IFileUri)_uri).Extension;

    /// <summary>
    /// Gets or sets the file attributes.
    /// </summary>
    public new FileAttributes Attributes
    {
        get => (FileAttributes)this.AttributesInternal;
        init => this.AttributesInternal = (FileSystemEntryAttributes)value;
    }

    internal sealed override FileSystemEntryAttributes AttributesInternal
    {
        get => base.AttributesInternal;
        init
        {
            if ((((FileAttributes)value) & FileAttributes.Normal) != FileAttributes.Normal)
                throw new ArgumentOutOfRangeException(nameof(value));

            base.AttributesInternal = value;
        }
    }
}
