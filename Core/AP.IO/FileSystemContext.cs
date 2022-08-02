using System.IO;
using AP.ComponentModel;
using AP.UniformIdentifiers;
using AP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.AccessControl;
using System.Runtime.CompilerServices;

namespace AP.IO
{
    /// <summary>
    /// Class for handling filesystem operations.
    /// </summary>
    public class FileSystemContext
    {
        private readonly FilesHelper _files;
        private readonly DirectoriesHelper _directories;

        /// <summary>
        /// Creates a new FileSystemContext instance.
        /// </summary>
        /// <param name="filesHelper">The FilesHelper. When null, a FilesHelper will be created.</param>
        /// <param name="directoriesHelper">The DirectoriesHelper. When null, a DirectoriesHelper will be created.</param>
        public FileSystemContext(FilesHelper filesHelper = null, DirectoriesHelper directoriesHelper = null)
        {
            _files = filesHelper ?? new FilesHelper();
            _directories = directoriesHelper ?? new DirectoriesHelper();
        }

        /// <summary>
        /// The FilesHelper.
        /// </summary>
        public FilesHelper Files { get { return _files; } }

        /// <summary>
        /// The DirectoriesHelper.
        /// </summary>
        public DirectoriesHelper Directories { get { return _directories; } }

        /// <summary>
        /// Opens a directory.
        /// </summary>
        /// <param name="directory">The Directory.</param>
        /// <param name="filter">The opening filter.</param>
        /// <param name="searchPattern">A search pattern.</param>
        /// <returns>An enumerable of files and/or directories - depending on the filter.</returns>
        protected internal virtual IEnumerable<FileSystemEntry> OpenDirectory(Directory directory, DirectorySearchFilter filter = DirectorySearchFilter.FilesAndDirectories, string searchPattern = "*")
        {
            // remove the IncludeSubdirectories flag - if it isn't in there to begin with, it won't matter
            DirectorySearchFilter entries = filter & ~DirectorySearchFilter.IncludeSubdirectories;
            
            // create the System.IO.SearchOption by comparing both filters for equality (if they're not equal - it can be assumed that the flag to include subdirectories was set)
            SearchOption option = filter == entries ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;

            searchPattern = searchPattern ?? "*";
            
            switch (entries)
            {
                case DirectorySearchFilter.FilesAndDirectories:
                    foreach (string fullName in System.IO.Directory.EnumerateFileSystemEntries(directory.FullName, searchPattern, option))
                        yield return this.Get(fullName);
                    break;
                case DirectorySearchFilter.Files:
                    foreach (string fullName in System.IO.Directory.EnumerateFiles(directory.FullName, searchPattern, option))
                        yield return this.Get(fullName);
                    break;
                case DirectorySearchFilter.Directories:
                    foreach (string fullName in System.IO.Directory.EnumerateDirectories(directory.FullName, searchPattern, option))
                        yield return this.Get(fullName);    
                    break;
                default:
                    throw new ArgumentOutOfRangeException("filter");
            }
        }

        /// <summary>
        /// Gets the parent directory using the full name.
        /// </summary>
        /// <param name="fullName">The full name of the FileSystemEntry.</param>
        /// <returns>The parent Directory or null.</returns>
        /// <exception cref="System.InvalidCastException">Throws an exception if the retrieved FileSystemEntry isn't a directory.</exception>
        public virtual Directory GetParent(string fullName)
        {               
            string parentName = System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(fullName));
            
            FileSystemEntry parent = this.Get(parentName);

            if (parent is File)
                throw new DirectoryNotFoundException(parent.FullName);

            return (Directory)parent;
        }

        /// <summary>
        /// Gets the parent of an existing file or directory.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The parent directory or null.</returns>
        protected internal Directory GetParent(FileSystemEntry source)
        {
            return this.GetParent(source.FullName);
        }

        /// <summary>
        /// Creates a new file.
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <param name="options">The file options.</param>
        /// <param name="security">The file security.</param>
        /// <param name="overwrite">Indicates if by creating a new file any existing file or directory should be deleted first.</param>
        /// <returns>The File object.</returns>
        public virtual File CreateFile(string fullName, FileCreationOptions options = FileCreationOptions.None, FileSecurity security = null, bool overwrite = false)
        {
            FileSystemEntry target;
            if (overwrite && this.Exists(fullName, out target))
                this.Delete(target);

            // todo: AccessSecurity
            System.IO.File.Create(fullName, 0, (System.IO.FileOptions)options/*, security*/).Dispose();

            return (File)this.Get(fullName);
        }

        /// <summary>
        /// Creates a new Directory.
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <param name="security">The directory security.</param>
        /// <param name="overwrite">Indicates if by creating a new directory any existing file or directory should be deleted first.</param>
        /// <returns>The Directory object.</returns>
        public virtual Directory CreateDirectory(string fullName, DirectorySecurity security = null, bool overwrite = false)
        {
            FileSystemEntry target;
            if (overwrite && this.Exists(fullName, out target))
                this.Delete(target);

            System.IO.Directory.CreateDirectory(fullName/*, security*/);

            return (Directory)this.Get(fullName);            
        }

        /// <summary>
        /// Clears a directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="permanent">Indicates if the directory should be cleared permanently.</param>
        protected internal virtual void ClearDirectory(Directory directory, bool permanent = true)
        {
            // might cause a problem when the source is a local collection and not some filesystem based stuff
            foreach (FileSystemEntry current in this.OpenDirectory(directory, DirectorySearchFilter.FilesAndDirectories, "*"))
                this.Delete(current, permanent);
        }

        /// <summary>
        /// Gets the FileSystemEntry using the full name.
        /// </summary>
        /// <param name="fullName">The full name of the FileSystemEntry.</param>
        /// <returns>The FileSystemEntry or null.</returns>
        public FileSystemEntry Get(string fullName)
        {
            FileSystemEntry output = null;

            if (this.TryGet(fullName, out output))
                return output;
            
            throw new ArgumentException("File or Directory does not exist.");
        }

        /// <summary>
        /// Gets the FileSystemEntry using the full name.
        /// </summary>
        /// <param name="fullName">The full name of the FileSystemEntry.</param>
        /// <returns>The FileSystemEntry or null.</returns>
        public bool Exists(string fullName)
        {
            FileSystemEntry output = null;

            return this.TryGet(fullName, out output);
        }
                
        /// <summary>
        /// Gets the FileSystemEntry using the full name.
        /// </summary>
        /// <param name="fullName">The full name of the FileSystemEntry.</param>
        /// <param name="target">The output.</param>
        /// <returns>Returns true when a FileSystemEntry can be retrieved.</returns>
        public bool Exists(string fullName, out FileSystemEntry target)
        {
            return this.TryGet(fullName, out target);
        }

        /// <summary>
        /// Gets the FileSystemEntry using the full name.
        /// </summary>
        /// <param name="fullName">The full name of the FileSystemEntry.</param>
        /// <param name="target">The output.</param>
        /// <returns>Returns true when a FileSystemEntry can be retrieved.</returns>
        public virtual bool TryGet(string fullName, out FileSystemEntry target)
        {
            if (fullName.IsNullOrWhiteSpace())
                throw new ArgumentNullException("fullName");

            // makes sure it's a proper fullName
            fullName = System.IO.Path.GetFullPath(fullName);

            if (System.IO.File.Exists(fullName))
            {
                target = new File(fullName);
                return true;
            }

            if (System.IO.Directory.Exists(fullName))
            {
                target = new Directory(fullName);
                return true;
            }

            target = null;
            return false;
        }

        /// <summary>
        /// Gets the file extension.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The file extension.</returns>
        protected internal virtual string GetExtension(File file)
        {
            return System.IO.Path.GetExtension(file.FullName);
        }

        /// <summary>
        /// Gets the name of a file or directory.
        /// </summary>
        /// <param name="target">The file or directory.</param>
        /// <returns>The file or directory name.</returns>
        protected internal virtual string GetName(FileSystemEntry target)
        {
            return System.IO.Path.GetFileName(target.FullName);
        }

        /// <summary>
        /// Copies a file or directory into a target directory.
        /// </summary>
        /// <param name="source">The source file or directory.</param>
        /// <param name="target">The target directory.</param>
        /// <param name="overwrite">Indicates if any existing file or directory should be deleted first.</param>
        /// <param name="newName">An alternate new name.</param>
        public virtual void Copy(FileSystemEntry source, Directory target, bool overwrite = false, string newName = null)
        {
            string targetName = System.IO.Path.Combine(target.FullName, newName ?? target.Name);

            FileSystemEntry tmp = null;

            if (overwrite && this.Exists(targetName, out tmp))
                this.Delete(tmp);

            if (source is File)
                System.IO.File.Copy(tmp.FullName, targetName);
            else 
            {
                int index = source.FullName.Length - 1;

                // open the directory - clone the tree
                foreach (FileSystemEntry current in this.OpenDirectory((Directory)source, DirectorySearchFilter.FilesAndDirectories | DirectorySearchFilter.IncludeSubdirectories, "*"))
                {
                    string currentTargetName = System.IO.Path.Combine(targetName, current.FullName.Substring(index));

                    if (current is File)
                        System.IO.File.Copy(current.FullName, currentTargetName, overwrite);
                    else
                        this.CreateDirectory(currentTargetName, (DirectorySecurity)current.Security, overwrite);                    
                }
            }
        }

        /// <summary>
        /// Moves a file or directory into a target directory.
        /// </summary>
        /// <param name="source">The source file or directory.</param>
        /// <param name="target">The target directory.</param>
        /// <param name="overwrite">Indicates if any existing file or directory should be deleted first.</param>
        /// <param name="newName">An alternate new name.</param>
        public virtual void Move(FileSystemEntry source, Directory target, bool overwrite = false, string newName = null)
        {
            string targetName = System.IO.Path.Combine(target.FullName, newName ?? target.Name);

            FileSystemEntry tmp = null;

            if (overwrite && this.Exists(targetName, out tmp))
                this.Delete(tmp);
            
            if (source is File)
                System.IO.File.Move(tmp.FullName, targetName);
            else
            {
                int index = source.FullName.Length - 1;

                // open the directory - clone the tree
                foreach (FileSystemEntry current in this.OpenDirectory((Directory)source, DirectorySearchFilter.FilesAndDirectories | DirectorySearchFilter.IncludeSubdirectories, "*"))
                {
                    string currentTargetName = System.IO.Path.Combine(targetName, current.FullName.Substring(index));
    
                    FileSystemEntry tmpTarget = null;                    
                    if (overwrite && this.Exists(currentTargetName, out tmpTarget))
                        this.Delete(tmpTarget);

                    if (current is File)
                        System.IO.File.Move(current.FullName, currentTargetName);
                    else
                        System.IO.Directory.Move(current.FullName, currentTargetName);
                }
            }
        }

        /// <summary>
        /// Deletes a file or directory.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="permanent">Indicates if a file or directory should be deleted permanently.</param>
        public virtual void Delete(FileSystemEntry target, bool permanent = true)
        {
            if (target is File)
                VBDeleteHelper.DeleteFile(target.FullName, permanent);
            else
                VBDeleteHelper.DeleteDirectory(target.FullName, permanent);
        }

        /// <summary>
        /// Creates a new File instance (without inheriting).
        /// </summary>
        /// <param name="fullName">The fullname.</param>
        /// <returns>A File instance.</returns>
        protected static File CreateFileInstance(string fullName)
        {
            return new File(fullName);
        }

        /// <summary>
        /// Helper method to create new Directory instances (without inheriting).
        /// </summary>
        /// <param name="fullName">The fullname.</param>
        /// <returns>A Directory instance.</returns>
        protected static Directory CreateDirectoryInstance(string fullName)
        {
            return new Directory(fullName);
        }
    }    
}
