using System;
using System.Security.AccessControl;

namespace AP.IO;

/// <summary>
/// Provides access to the filesystem.
/// </summary>
public abstract class FileSystem : StaticType
{
    private static volatile FileSystemContext _context;

    /// <summary>
    /// Gets the currently used FileSystemContext.
    /// </summary>
    public static FileSystemContext Context => _context ?? new FileSystemContext();

    protected static readonly object SyncRoot = new();
    private static bool _isInitialized = false;

    protected FileSystem()
        : base()
    { }

    /// <summary>
    /// Initializes the filesystem to use a specific context.
    /// May only run once, unless has been released before.
    /// </summary>
    /// <param name="context">The context. When it is null a new FileSystemContext will be generated.</param>
    public static void Initialize(FileSystemContext? context = null)
    {
        lock (SyncRoot)
        {
            if (_isInitialized)
                throw new InvalidOperationException("Already initialized - use Release() first");

            _context = context ?? new FileSystemContext();                
            _isInitialized = true;
        }
    }

    /// <summary>
    /// Releases the currently used FileSystemContext. 
    /// </summary>
    public static void Release()
    {
        lock (SyncRoot)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Not initialized");

            _context.Dispose();
            _context = null;
        }
    }

    /// <summary>
    /// Gets the FileSystemEntry using the full name.
    /// </summary>
    /// <param name="fullName">The full name of the FileSystemEntry.</param>
    /// <returns>The FileSystemEntry or null.</returns>
    public static FileSystemEntry Get(string fullName) => Context.Get(fullName);

    /// <summary>
    /// Gets the FileSystemEntry using the full name.
    /// </summary>
    /// <param name="fullName">The full name of the FileSystemEntry.</param>
    /// <param name="target">The output.</param>
    /// <returns>Returns true when a FileSystemEntry can be retrieved.</returns>
    public static bool TryGet(string fullName, out FileSystemEntry target) => Context.TryGet(fullName, out target);

    /// <summary>
    /// Gets the FileSystemEntry using the full name.
    /// </summary>
    /// <param name="fullName">The full name of the FileSystemEntry.</param>
    /// <returns>The FileSystemEntry or null.</returns>
    public static bool Exists(string fullName) => Context.Exists(fullName);

    /// <summary>
    /// Gets the FileSystemEntry using the full name.
    /// </summary>
    /// <param name="fullName">The full name of the FileSystemEntry.</param>
    /// <param name="target">The output.</param>
    /// <returns>Returns true when a FileSystemEntry can be retrieved.</returns>
    public static bool Exists(string fullName, out FileSystemEntry target) => Context.Exists(fullName, out target);

    /// <summary>
    /// Returns the parent directory using the fullname of an existing file or directory.
    /// </summary>
    /// <param name="fullName">The fullname of the file or directory.</param>
    /// <returns>The parent directory.</returns>
    public static Directory GetParent(string fullName) => Context.GetParent(fullName);

    /// <summary>
    /// Copies a file or directory into a target directory.
    /// </summary>
    /// <param name="source">The source file or directory.</param>
    /// <param name="target">The target directory.</param>
    /// <param name="overwrite">Indicates if any existing file or directory should be deleted first.</param>
    /// <param name="newName">An alternate new name.</param>
    public static void Copy(FileSystemEntry source, Directory target, bool overwrite = false, string? newName = null) => Context.Copy(source, target, overwrite, newName);

    /// <summary>
    /// Moves a file or directory into a target directory.
    /// </summary>
    /// <param name="source">The source file or directory.</param>
    /// <param name="target">The target directory.</param>
    /// <param name="overwrite">Indicates if any existing file or directory should be deleted first.</param>
    /// <param name="newName">An alternate new name.</param>
    public static void Move(FileSystemEntry source, Directory target, bool overwrite = false, string? newName = null) => Context.Move(source, target, overwrite, newName);

    /// <summary>
    /// Deletes a file or directory.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <param name="permanent">Indicates if a file or directory should be deleted permanently.</param>
    public static void Delete(FileSystemEntry target, bool permanent = true) => Context.Delete(target);

    /// <summary>
    /// Creates a new file.
    /// </summary>
    /// <param name="fullName">The full name.</param>
    /// <param name="options">The file options.</param>
    /// <param name="security">The file security.</param>
    /// <param name="overwrite">Indicates if any existing file or directory should be deleted first.</param>
    /// <returns>The File object.</returns>
    public static File CreateFile(string fullName, FileCreationOptions options = FileCreationOptions.None, FileSecurity? security = null, bool overwrite = false) => Context.CreateFile(fullName, options, security, overwrite);

    /// <summary>
    /// Creates a new Directory.
    /// </summary>
    /// <param name="fullName">The full name.</param>
    /// <param name="security">The directory security.</param>
    /// <param name="overwrite">Indicates if any existing file or directory should be deleted first.</param>
    /// <returns>The Directory object.</returns>
    public static Directory CreateDirectory(string fullName, DirectorySecurity? security = null, bool overwrite = false) => Context.CreateDirectory(fullName, security, overwrite);
}
