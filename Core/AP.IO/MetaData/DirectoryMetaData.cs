using AP.UniformIdentifiers;
using System;

namespace AP.IO.MetaData;

/// <summary>
/// Class for storing directory information.
/// </summary>
[Serializable]
public class DirectoryMetaData : FileSystemEntryMetaDataBase, IDirectoryMetaData
{
    /// <summary>
    /// Creates a new DirectoryMetaData instace.
    /// </summary>
    /// <param name="uri">The uri.</param>
    public DirectoryMetaData(IHierarchicalUri uri)
        : base(uri)
    { }

    /// <summary>
    /// Gets or sets directory attributes.
    /// </summary>
    public new DirectoryAttributes Attributes
    {
        get => (DirectoryAttributes)this.AttributesInternal;
        set => this.AttributesInternal = (FileSystemEntryAttributes)value;
    }

    internal sealed override FileSystemEntryAttributes AttributesInternal
    {
        get => base.AttributesInternal;
        set
        {
            if ((((DirectoryAttributes)value) & DirectoryAttributes.Normal) != DirectoryAttributes.Normal)
                throw new ArgumentOutOfRangeException(nameof(value));

            base.AttributesInternal = value;
        }
    }
}    
