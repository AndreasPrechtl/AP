using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using FA = System.IO.FileAttributes;
using FO = System.IO.FileOptions;
using FS = System.IO.FileShare;

namespace AP.IO;

/// <summary>
/// Attributes for files or directories.
/// </summary>
[Serializable, Flags, DefaultValue(FileSystemEntryAttributes.Normal)]
public enum FileSystemEntryAttributes
{
    Normal = FA.Normal,
    Archive = FA.Archive,
    Compressed = FA.Compressed,
    Encrypted = FA.Encrypted,
    Hidden = FA.Hidden,
    [ComVisible(false)]
    IntegrityStream = 32768, // fa.IntegrityStream,
    [ComVisible(false)]
    NoScrubData = 131072, // fa.NoScrubData,
    NotContentIndexed = FA.NotContentIndexed,
    Offline = FA.Offline,
    ReadOnly = FA.ReadOnly,
    ReparsePoint = FA.ReparsePoint,
    SparseFile = FA.SparseFile,
    System = FA.System,
    Temporary = FA.Temporary,
    Device = FA.Device
}

/// <summary>
/// File attributes.
/// </summary>
[Serializable, Flags, DefaultValue(FileAttributes.Normal)]
public enum FileAttributes
{
    Normal = FA.Normal,
    Archive = FA.Archive,
    Compressed = FA.Compressed,
    Encrypted = FA.Encrypted,
    Hidden = FA.Hidden,
    [ComVisible(false)]
    IntegrityStream = 32768, // fa.IntegrityStream,        
    [ComVisible(false)]
    NoScrubData = 131072, // fa.NoScrubData,
    NotContentIndexed = FA.NotContentIndexed,
    Offline = FA.Offline,
    ReadOnly = FA.ReadOnly,
    ReparsePoint = FA.ReparsePoint,
    SparseFile = FA.SparseFile,
    System = FA.System,
    Temporary = FA.Temporary
}

/// <summary>
/// Directory attributes.
/// </summary>
[Serializable, Flags, DefaultValue(DirectoryAttributes.Normal)]
public enum DirectoryAttributes
{
    Normal = FA.Directory,
    Device = FA.Device,
    System = FA.System,
    Offline = FA.Offline,
    Compressed = FA.Compressed,
    Archive = FA.Archive,
    Hidden = FA.Hidden,
    Encrypted = FA.Encrypted,
    ReadOnly = FA.ReadOnly,
    Temporary = FA.Temporary,
    NotContentIndexed = FA.NotContentIndexed,
    [ComVisible(false)]
    NoScrubData = 131072, // fa.NoScrubData,
    [ComVisible(false)]
    IntegrityStream = 32768 // fa.IntegrityStream,
}

/// <summary>
/// File creation options.
/// </summary>
[Serializable, Flags]
public enum FileCreationOptions
{
    None = FO.None,
    Asynchronous = FO.Asynchronous,
    Encrypted = FO.Encrypted,        
    RandomAccess = FO.RandomAccess,
    SequentialScan = FO.SequentialScan,
    WriteThrough = FO.WriteThrough
}

/// <summary>
/// File opening mode.
/// </summary>
[Serializable, DefaultValue(FileOpeningMode.ReadOrWrite)]
public enum FileOpeningMode
{
    Read = 1,        
    Write = 2,
    ReadOrWrite = 3,
    Truncate = 5,
    Append = 6        
}

/// <summary>
/// Filter for opening directories.
/// </summary>
[Flags, Serializable, DefaultValue(DirectorySearchFilter.FilesAndDirectories)]
public enum DirectorySearchFilter
{   
    FilesAndDirectories = Files | Directories,
    Files = 1, 
    Directories = 2,
    IncludeSubdirectories = 4
}

/// <summary>
/// File share mode.
/// </summary>
[Serializable, DefaultValue(FileShare.None)]
public enum FileShare
{
    None = FS.None,
    Read = FS.Read,
    Write = FS.Write,
    ReadOrWrite = FS.ReadWrite,
    Delete = FS.Delete        
}
