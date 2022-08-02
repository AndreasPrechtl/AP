using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.IO.MetaData
{
    /// <summary>
    /// Class for storing directory information.
    /// </summary>
    [Serializable]
    public class DirectoryMetaData : FileSystemEntryMetaDataBase, IDirectoryMetaData
    {
        /// <summary>
        /// Creates a new DirectoryMetaData instace.
        /// </summary>
        /// <param name="uri">The uri.</param>
        public DirectoryMetaData(IHierarchicalUri uri)
            : base(uri)
        { }

        /// <summary>
        /// Gets or sets directory attributes.
        /// </summary>
        public new DirectoryAttributes Attributes
        {
            get { return (DirectoryAttributes)this.AttributesInternal; }
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
                if ((((DirectoryAttributes)value) & DirectoryAttributes.Normal) != DirectoryAttributes.Normal)
                    throw new ArgumentOutOfRangeException("value");

                base.AttributesInternal = value;
            }
        }
    }    
}
