using System;
using System.Security.AccessControl;
using System.Numerics;
using System.Reflection;

namespace AP.IO;

public abstract class FileSystemEntry
{
    private readonly string _fullName;
    private readonly Deferrable<string> _name;

    /// <summary>
    /// Creates a new FileSystemEntry instance. 
    /// Instances can only be created by using the FileSystemContext.
    /// </summary>
    /// <param name="fullName">The full name of the file or directory.</param>
    internal FileSystemEntry(string fullName)
    {
        if (fullName.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(fullName));

        var ci = this.GetType().GetConstructor([typeof(string)]);

        if (ci != null)
            throw new InvalidOperationException("FileSystemEntries cannot have a public constructor.");

        //// clean the path
        //fullName = System.IO.Path.GetFullPath(fullName);

        // get the caller and block any other attempt to create a FileSystemEntry without using the context
                    
        // nasty code for inheritance - removed it 
        //Type t = new System.Diagnostics.StackFrame(3, false).GetMethod().DeclaringType;
        
        //if (!t.Is(typeof(FileSystemContext)))
        //    throw new InvalidOperationException("ctor access only allowed via FSContext");

        _fullName = fullName;
        _name = new Deferrable<string>(() => FileSystem.Context.GetName(this));
    }

    /// <summary>
    /// Gets the parent directory.
    /// </summary>
    public Directory? Parent => FileSystem.Context.GetParent(this);

    /// <summary>
    /// Gets the size.
    /// </summary>
    public abstract BigInteger Size { get; }

    /// <summary>
    /// Gets the full name.
    /// </summary>
    public string FullName => _fullName;

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name => _name.Value;

    internal abstract FileSystemSecurity SecurityInternal { get; set; }
    internal abstract FileSystemEntryAttributes AttributesInternal { get; set; }
    
    /// <summary>
    /// Calculates the checksum.
    /// </summary>
    /// <param name="algorithm">The used algorithm. If none is specified SHA512 will be used.</param>
    /// <param name="ownsAlgorithm">Indicates if the algorithm should be disposed after use.</param>
    /// <returns>The checksum.</returns>
    public abstract byte[] GetChecksum(System.Security.Cryptography.HashAlgorithm? algorithm = null, bool ownsAlgorithm = true);

    /// <summary>
    /// Gets or sets the file or directory security.
    /// </summary>
    public FileSystemSecurity Security
    {
        get => this.SecurityInternal;
        set => this.SecurityInternal = value;
    }

    /// <summary>
    /// Gets or sets the file or directory attributes.
    /// </summary>
    public FileSystemEntryAttributes Attributes
    {
        get => this.AttributesInternal;
        set => this.AttributesInternal = value;
    }

    /// <summary>
    /// Gets or sets the last access date/time.
    /// </summary>
    public abstract DateTimeOffset DateAccessed { get; set; }

    /// <summary>
    /// Gets or sets the creation date/time.
    /// </summary>
    public abstract DateTimeOffset DateCreated { get; set; }

    /// <summary>
    /// Gets or sets the last write date/time.
    /// </summary>
    public abstract DateTimeOffset DateModified { get; set; }
}
