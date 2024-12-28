namespace AP.IO.MetaData;

/// <summary>
/// Interface for file meta data.
/// </summary>
public interface IFileMetaData : IFileSystemEntryMetaData
{
    /// <summary>
    /// Gets or sets the file attributes.
    /// </summary>
    new FileAttributes Attributes { get; }

    /// <summary>
    /// Gets the file extension.
    /// </summary>
    string Extension { get; }
}
