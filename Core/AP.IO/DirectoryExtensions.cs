using System.Security.AccessControl;

namespace AP.IO;

/// <summary>
/// Extension methods for Directory
/// </summary>
public static class DirectoryExtensions
{
    /// <summary>
    /// Creates a new Directory.
    /// </summary>
    /// <param name="directory">The directory.</param>
    /// <param name="security">The directory security.</param>
    /// <param name="overwrite">Indicates if an existing file or directory should be deleted.</param>
    public static void Create(this Directory directory, DirectorySecurity? security = null, bool overwrite = false) => FileSystem.Context.CreateDirectory(directory.FullName, security, overwrite);
}
