using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.IO.MetaData
{
    /// <summary>
    /// Class for storing file information.
    /// </summary>
    [Serializable]
    public class FileMetaData : FileSystemEntryMetaDataBase, IFileMetaData
    {
        /// <summary>
        /// Creates a new instance of FileMetaData.
        /// </summary>
        /// <param name="uri">The uri.</param>
        public FileMetaData(IFileUri uri)
            : base(uri)
        { }

        /// <summary>
        /// Gets the extension.
        /// </summary>
        public string Extension
        {
            get { return ((IFileUri)_uri).Extension; }
        }

        /// <summary>
        /// Gets or sets the file attributes.
        /// </summary>
        public new FileAttributes Attributes
        {
            get { return (FileAttributes)this.AttributesInternal; }
            set { this.AttributesInternal = (FileSystemEntryAttributes)value; }
        }

        internal sealed override FileSystemEntryAttributes AttributesInternal
        {
            get
            {
                return base.AttributesInternal;
            }
            set
            {
                if ((((FileAttributes)value) & FileAttributes.Normal) != FileAttributes.Normal)
                    throw new ArgumentOutOfRangeException("value");

                base.AttributesInternal = value;
            }
        }
    }
}
