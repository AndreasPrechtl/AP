using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.IO.MetaData
{
    /// <summary>
    /// Base class for file or directory metadata.
    /// </summary>
    [Serializable]    
    public abstract class FileSystemEntryMetaDataBase : IFileSystemEntryMetaData
    {
        protected readonly IHierarchicalUri _uri;
        private object _security;
        private FileSystemEntryAttributes _attributes;

        internal FileSystemEntryMetaDataBase(IHierarchicalUri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            _uri = uri;
        }

        #region IFSObjectMetaData Members

        /// <summary>
        /// Gets the full name.
        /// </summary>
        public string FullName
        {
            get { return _uri.FullName; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get { return _uri.Name; }
        }

        /// <summary>
        /// Gets or sets the security.
        /// </summary>
        public virtual object Security
        {
            get { return _security; }
            set { _security = value; }
        }

        /// <summary>
        /// Gets or sets the file or directory attribues.
        /// </summary>
        public FileSystemEntryAttributes Attributes
        {
            get
            {
                return this.AttributesInternal;
            }
            set
            {
                this.AttributesInternal = value;
            }
        }

        private DateTimeOffset _dateAccessed, _dateModified, _dateCreated;

        /// <summary>
        /// Gets or sets the last access date/time.
        /// </summary>
        public DateTimeOffset DateAccessed
        {
            get
            {
                return _dateAccessed;
            }
            set
            {
                //if (value < _dateCreated)
                //    throw new ArgumentOutOfRangeException("value");

                _dateAccessed = value;
            }
        }

        /// <summary>
        /// Gets or sets the creation date/time.
        /// </summary>
        public DateTimeOffset DateCreated
        {
            get { return _dateCreated; }
            set
            {
                //if (value > _dateModified || value > _dateAccessed)
                //    throw new ArgumentOutOfRangeException("value");

                _dateCreated = value;
            }
        }

        /// <summary>
        /// Gets or sets the last write date/time.
        /// </summary>
        public DateTimeOffset DateModified
        {
            get
            {
                return _dateModified;
            }
            set
            {
                //if (value < _dateCreated)
                //    throw new ArgumentOutOfRangeException("value");

                _dateModified = value;
            }
        }

        #endregion

        internal virtual FileSystemEntryAttributes AttributesInternal
        {
            get { return _attributes; }
            set { _attributes = value; }
        }
    }
}
