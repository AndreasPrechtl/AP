using System.IO;
using System;
using System.Security.AccessControl;
using System.Numerics;

namespace AP.IO;

public class File : FileSystemEntry
{
    private readonly Lazy<string> _extension;

    /// <summary>
    /// Creates a new File. 
    /// Instances can only be created by using the FileSystemContext.
    /// </summary>
    /// <param name="fullName">The full name of the file.</param>
    protected internal File(string fullName)
        : base(fullName)
    {
        _extension = new Lazy<string>(() => System.IO.Path.GetExtension(this.FullName));
    }

    /// <summary>
    /// Gets the file extension.
    /// </summary>
    public string Extension => _extension.Value;

    /// <summary>
    /// Gets the size of the file.
    /// </summary>
    public override BigInteger Size => (BigInteger)new System.IO.FileInfo(this.FullName).Length;


    /// <summary>
    /// Calculates the checksum.
    /// </summary>
    /// <param name="algorithm">The used algorithm. If none is specified SHA512 will be used.</param>
    /// <param name="ownsAlgorithm">Indicates if the algorithm should be disposed after use.</param>
    /// <returns>The checksum.</returns>
    public sealed override byte[] GetChecksum(System.Security.Cryptography.HashAlgorithm? algorithm = null, bool ownsAlgorithm = true)
    {
        if (algorithm == null)
        {
            algorithm = algorithm ?? System.Security.Cryptography.SHA512.Create();
            ownsAlgorithm = true;
        }

        byte[] hash = algorithm.ComputeHash(this.Open()); ;

        if (ownsAlgorithm)
            algorithm.Dispose();

        return hash;
    }

    internal sealed override FileSystemSecurity SecurityInternal
    {
        get => this.Security;
        set => this.Security = (FileSecurity)value;
    }

    internal sealed override FileSystemEntryAttributes AttributesInternal
    {
        get => (FileSystemEntryAttributes)this.Attributes;
        set => this.Attributes = (FileAttributes)value;
    }

    /// <summary>
    /// Opens the file.
    /// </summary>
    /// <param name="mode">The opening mode.</param>
    /// <param name="share">The sharing options.</param>
    /// <returns>A Stream.</returns>
    public virtual Stream Open(FileOpeningMode mode = FileOpeningMode.ReadOrWrite, FileShare share = FileShare.None)
    {
        FileMode m = FileMode.Open;
        FileAccess a = FileAccess.ReadWrite;

        // ignored default cases - correct values are already set.
        switch (mode)
        {
            case FileOpeningMode.Read:
                m = FileMode.Open;
                a = FileAccess.Read;
                break;
            case FileOpeningMode.Write:
                m = FileMode.Open;
                a = FileAccess.Write;
                break;
            case FileOpeningMode.Truncate:
                m = FileMode.Truncate;
                a = FileAccess.Write;
                break;
            case FileOpeningMode.Append:
                m = FileMode.Append;
                a = FileAccess.Write;
                break;
        }

        return System.IO.File.Open(this.FullName, m, a, (System.IO.FileShare)share);
    }

    /// <summary>
    /// Clears the file.
    /// </summary>
    public virtual void Clear() => System.IO.File.Open(this.FullName, FileMode.Truncate).Dispose();

    /// <summary>
    /// Gets or sets the file attributes.
    /// </summary>
    public virtual new FileAttributes Attributes
    {
        get => (FileAttributes)System.IO.File.GetAttributes(this.FullName);
        set
        {
            if ((value & FileAttributes.Normal) != FileAttributes.Normal)
                throw new ArgumentOutOfRangeException(nameof(value));

            System.IO.File.SetAttributes(this.FullName, (System.IO.FileAttributes)value);
        }
    }

    /// <summary>
    /// Gets or sets the last access date/time.
    /// </summary>
    public override DateTimeOffset DateAccessed
    {
        get => System.IO.File.GetLastAccessTimeUtc(this.FullName);
        set => System.IO.File.SetLastAccessTimeUtc(this.FullName, value.UtcDateTime);
    }

    /// <summary>
    /// Gets or sets the creation date/time.
    /// </summary>
    public override DateTimeOffset DateCreated
    {
        get => System.IO.File.GetCreationTimeUtc(this.FullName);
        set => System.IO.File.SetCreationTimeUtc(this.FullName, value.UtcDateTime);
    }

    /// <summary>
    /// Gets or sets the last write date/time.
    /// </summary>
    public override DateTimeOffset DateModified
    {
        get => System.IO.File.GetLastWriteTimeUtc(this.FullName);
        set => System.IO.File.SetLastWriteTimeUtc(this.FullName, value.UtcDateTime);
    }
}
