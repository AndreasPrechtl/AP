using System;

namespace AP.IO;

/// <summary>
/// Extensions for FileSystemEntrys.
/// </summary>
public static class FileSystemEntryExtensions
{
    /// <summary>
    /// Renames a file or directory.
    /// </summary>
    /// <param name="target">The file or directory.</param>
    /// <param name="newName">The new name.</param>
    /// <param name="overwrite">Indicates if an existing item should be deleted.</param>
    public static void Rename(this FileSystemEntry target, string newName, bool overwrite = false)
    {
        if (newName.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(newName));

        FileSystem.Context.GetParent(target);
    }

    /// <summary>
    /// Moves a file or directory into a target directory.
    /// </summary>
    /// <param name="source">The source file or directory.</param>
    /// <param name="target">The target directory.</param>
    /// <param name="overwrite">Indicates if any existing file or directory should be deleted first.</param>
    /// <param name="newName">An alternate new name.</param>
    public static void MoveTo(this FileSystemEntry source, Directory target, bool overwrite = false, string? newName = null) => FileSystem.Context.Move(source, target, overwrite, newName);

    /// <summary>
    /// Copies a file or directory into a target directory.
    /// </summary>
    /// <param name="source">The source file or directory.</param>
    /// <param name="target">The target directory.</param>
    /// <param name="overwrite">Indicates if any existing file or directory should be deleted first.</param>
    /// <param name="newName">An alternate new name.</param>
    public static void CopyTo(this FileSystemEntry source, Directory target, bool overwrite = false, string? newName = null) => FileSystem.Context.Move(source, target, overwrite, newName);

    /// <summary>
    /// Deletes a file or directory.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <param name="permanent">Indicates if a file or directory should be deleted permanently.</param>
    public static void Delete(this FileSystemEntry target, bool permanent = true) => FileSystem.Context.Delete(target, permanent);
}
