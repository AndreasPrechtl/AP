using System.IO;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Numerics;

namespace AP.IO;

public class Directory : FileSystemEntry
{
    /// <summary>
    /// Creates a new Directory. 
    /// Instances can only be created by using the FileSystemContext.
    /// </summary>
    /// <param name="fullName">The full name of the directory.</param>
    protected internal Directory(string fullName)
        : base(fullName)
    { }

    #region IDirectory Members

    /// <summary>
    /// Clears the directory.
    /// </summary>
    /// <param name="permanent">Indicates if all content should be deleted permanently.</param>
    public void Clear(bool permanent = true) => FileSystem.Context.ClearDirectory(this, permanent);

    /// <summary>
    /// Opens the directory.
    /// </summary>
    /// <param name="filter">The opening filter.</param>
    /// <param name="searchPattern">The search pattern.</param>
    /// <param name="includeSubdirectories">Indicates if all </param>
    /// <returns></returns>
    public IEnumerable<FileSystemEntry> Open(DirectorySearchFilter filter = DirectorySearchFilter.FilesAndDirectories, string searchPattern = "*") => FileSystem.Context.OpenDirectory(this, filter, searchPattern);

    #endregion

    #region IDirectoryMetaData Members

    /// <summary>
    /// Gets the size of the directory.
    /// </summary>
    public override BigInteger Size
    {
        get
        {
            BigInteger size = 0;

            foreach (FileSystemEntry current in this.Open(DirectorySearchFilter.FilesAndDirectories | DirectorySearchFilter.IncludeSubdirectories, "*"))
                if (current is File)
                    size += current.Size;

            return size;
        }
    }
    
    /// <summary>
    /// Encrypts all files in the directory (including sub-directories).
    /// </summary>
    public override void Encrypt()
    {
        foreach (FileSystemEntry current in this.Open(DirectorySearchFilter.Files | DirectorySearchFilter.IncludeSubdirectories, "*"))
            if (current is File)
                current.Encrypt();
    }

    /// <summary>
    /// Decrypts all files in the directory (including sub-directories).
    /// </summary>
    public override void Decrypt()
    {
        FileSystemContext context = FileSystem.Context;

        foreach (FileSystemEntry current in context.OpenDirectory(this, DirectorySearchFilter.Files | DirectorySearchFilter.IncludeSubdirectories, "*"))
            if (current is File)
                current.Decrypt();
    }

    internal sealed override FileSystemSecurity SecurityInternal
    {
        get => this.Security;
        set => this.Security = (DirectorySecurity)value;
    }

    internal sealed override FileSystemEntryAttributes AttributesInternal
    {
        get => (FileSystemEntryAttributes)this.Attributes;
        set => this.Attributes = (DirectoryAttributes)value;
    }

    /// <summary>
    /// Calculates the checksum.
    /// </summary>
    /// <param name="algorithm">The used algorithm. If none is specified SHA512 will be used.</param>
    /// <param name="ownsAlgorithm">Indicates if the algorithm should be disposed after use.</param>
    /// <returns>The checksum.</returns>
    public override byte[] GetChecksum(System.Security.Cryptography.HashAlgorithm? algorithm = null, bool ownsAlgorithm = true)
    {
        if (algorithm == null)
        {
            algorithm = algorithm ?? System.Security.Cryptography.SHA512.Create();
            ownsAlgorithm = true;
        }

        byte[] hash = null;

        using (MemoryStream ms = new())
        {
            foreach (File current in this.Open(DirectorySearchFilter.Files | DirectorySearchFilter.IncludeSubdirectories, "*"))
            {
                byte[] currentChecksum = current.GetChecksum(algorithm, false);
                ms.Write(currentChecksum, 0, currentChecksum.Length);
                algorithm.Clear();                    
            }
            hash = algorithm.ComputeHash(ms.ToArray());
        }

        if (ownsAlgorithm)
            algorithm.Dispose();

        return hash;
    }

    // todo: AccessControl
    ///// <summary>
    ///// Gets or sets the directory security.
    ///// </summary>
    //public virtual new System.Security.AccessControl.DirectorySecurity Security
    //{
    //    get
    //    {
    //        return System.IO.Directory.GetAccessControl(this.FullName);
    //    }
    //    set
    //    {
    //        System.IO.Directory.SetAccessControl(this.FullName, value);
    //    }
    //}

    /// <summary>
    /// Gets or sets the directory attributes.
    /// </summary>
    public virtual new DirectoryAttributes Attributes
    {
        get => (DirectoryAttributes)System.IO.File.GetAttributes(this.FullName);
        set
        {
            if ((value & DirectoryAttributes.Normal) != DirectoryAttributes.Normal)
                throw new ArgumentOutOfRangeException(nameof(value));

            System.IO.File.SetAttributes(this.FullName, (System.IO.FileAttributes)value);
        }
    }

    /// <summary>
    /// Gets or sets the last access date/time.
    /// </summary>
    public override DateTimeOffset DateAccessed
    {
        get => System.IO.Directory.GetLastAccessTimeUtc(this.FullName);
        set => System.IO.Directory.SetLastAccessTimeUtc(this.FullName, value.UtcDateTime);
    }

    /// <summary>
    /// Gets or sets the creation date/time.
    /// </summary>
    public override DateTimeOffset DateCreated
    {
        get => System.IO.Directory.GetCreationTimeUtc(this.FullName);
        set => System.IO.Directory.SetCreationTimeUtc(this.FullName, value.UtcDateTime);
    }

    /// <summary>
    /// Gets or sets the last write date/time.
    /// </summary>
    public override DateTimeOffset DateModified
    {
        get => System.IO.Directory.GetLastWriteTimeUtc(this.FullName);
        set => System.IO.Directory.SetLastWriteTimeUtc(this.FullName, value.UtcDateTime);
    }

    #endregion
}
