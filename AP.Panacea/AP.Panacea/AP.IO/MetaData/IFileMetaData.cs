using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.IO.MetaData
{
    /// <summary>
    /// Interface for file meta data.
    /// </summary>
    public interface IFileMetaData : IFileSystemEntryMetaData
    {
        /// <summary>
        /// Gets or sets the file attributes.
        /// </summary>
        new FileAttributes Attributes { get; set; }

        /// <summary>
        /// Gets the file extension.
        /// </summary>
        string Extension { get; }
    }
}
