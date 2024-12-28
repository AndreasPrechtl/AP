using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using fa = System.IO.FileAttributes;
using fo = System.IO.FileOptions;
using fs = System.IO.FileShare;

namespace AP.IO;

/// <summary>
/// Attributes for files or directories.
/// </summary>
[Serializable, Flags, DefaultValue(FileSystemEntryAttributes.Normal)]
public enum FileSystemEntryAttributes
{
    Normal = fa.Normal,
    Archive = fa.Archive,
    Compressed = fa.Compressed,
    Encrypted = fa.Encrypted,
    Hidden = fa.Hidden,
    [ComVisible(false)]
    IntegrityStream = 32768, // fa.IntegrityStream,
    [ComVisible(false)]
    NoScrubData = 131072, // fa.NoScrubData,
    NotContentIndexed = fa.NotContentIndexed,
    Offline = fa.Offline,
    ReadOnly = fa.ReadOnly,
    ReparsePoint = fa.ReparsePoint,
    SparseFile = fa.SparseFile,
    System = fa.System,
    Temporary = fa.Temporary,
    Device = fa.Device
}

/// <summary>
/// File attributes.
/// </summary>
[Serializable, Flags, DefaultValue(FileAttributes.Normal)]
public enum FileAttributes
{
    Normal = fa.Normal,
    Archive = fa.Archive,
    Compressed = fa.Compressed,
    Encrypted = fa.Encrypted,
    Hidden = fa.Hidden,
    [ComVisible(false)]
    IntegrityStream = 32768, // fa.IntegrityStream,        
    [ComVisible(false)]
    NoScrubData = 131072, // fa.NoScrubData,
    NotContentIndexed = fa.NotContentIndexed,
    Offline = fa.Offline,
    ReadOnly = fa.ReadOnly,
    ReparsePoint = fa.ReparsePoint,
    SparseFile = fa.SparseFile,
    System = fa.System,
    Temporary = fa.Temporary
}

/// <summary>
/// Directory attributes.
/// </summary>
[Serializable, Flags, DefaultValue(DirectoryAttributes.Normal)]
public enum DirectoryAttributes
{
    Normal = fa.Directory,
    Device = fa.Device,
    System = fa.System,
    Offline = fa.Offline,
    Compressed = fa.Compressed,
    Archive = fa.Archive,
    Hidden = fa.Hidden,
    Encrypted = fa.Encrypted,
    ReadOnly = fa.ReadOnly,
    Temporary = fa.Temporary,
    NotContentIndexed = fa.NotContentIndexed,
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
    None = fo.None,
    Asynchronous = fo.Asynchronous,
    Encrypted = fo.Encrypted,        
    RandomAccess = fo.RandomAccess,
    SequentialScan = fo.SequentialScan,
    WriteThrough = fo.WriteThrough
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
    None = fs.None,
    Read = fs.Read,
    Write = fs.Write,
    ReadOrWrite = fs.ReadWrite,
    Delete = fs.Delete        
}
