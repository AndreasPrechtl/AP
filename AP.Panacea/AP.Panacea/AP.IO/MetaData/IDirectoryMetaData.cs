using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.IO.MetaData
{
    /// <summary>
    /// Interface for directory metadata.
    /// </summary>
    public interface IDirectoryMetaData : IFileSystemEntryMetaData
    {
        /// <summary>
        /// Gets or sets the directory attributes.
        /// </summary>
        new DirectoryAttributes Attributes { get; set; }
    }        
}
