using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace AP.IO
{
    /// <summary>
    /// File extensions.
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// Creates a new File.
        /// </summary>
        /// <param name="file">The File.</param>
        /// <param name="options">The file options.</param>
        /// <param name="security">The file security.</param>
        /// <param name="overwrite">Indicates if an existing file or directory should be deleted.</param>
        public static void Create(this File file, FileCreationOptions options = FileCreationOptions.None, FileSecurity security = null, bool overwrite = false)
        {
            FileSystem.Context.CreateFile(file.FullName, options, security, overwrite);
        }
    }
}
