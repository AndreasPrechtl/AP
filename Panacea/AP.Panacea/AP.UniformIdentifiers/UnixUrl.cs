using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AP.Linq;

namespace AP.UniformIdentifiers
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly"), Serializable]
    public sealed class UnixUrl : UriBase, IAbsoluteOrRelativeUri, IHierarchicalUri, IFileUri
    {
        public UnixUrl(string path)
            : this(new string[] { path })
        {
            this.OriginalString = path;
        }

        public UnixUrl(IEnumerable<string> segments)
            : this()
        {
            StringBuilder sb = new StringBuilder();

            foreach (string s in segments)
            {
                sb.Append(s);
                sb.Append('/');
            }

            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            sb.Trim();
            string unclean = sb.ToString();

            // check for illegal characters? nay - it's a posix url - almost anything goes...
            // now split it again and put in some values
            string[] splitted = sb.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            sb.Clear();

            foreach (string s in splitted)
            {
                sb.Append(s.Trim());
                sb.Append('/');
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            this.Path = unclean.Length > 0 && unclean[0] == '/' ? sb.Insert(0, '/').ToString() : sb.ToString();
            
            // should not be needed anymore
            //string name = null;

            //name = splitted.Length > 0 ? splitted[splitted.Length - 1] : string.Empty;
            
            //this.Name = name;
            
            //if (this.Name.IsEmpty() || this.Name.Equals(".") || this.Name.Equals(".."))
            //    this.Extension = string.Empty;
            //else
            //{
            //    int index = name.LastIndexOf('.');
            //    this.Extension = index > 1 ? name.Split(index)[1] : string.Empty;
            //}
        }

        private UnixUrl()
        {
            _parent = new Deferrable<UnixUrl>(this.CreateParent, false);
        }

        public override string Scheme
        {
            get { return @"/"; }
        }

        protected override void BuildFullName(ref StringBuilder builder)
        {
            builder.Clear();
            builder.Append(this.Path);
        }

        #region IFileUri Members

        private string _extension;

        [IgnoreDataMember]
        public string Extension
        {
            get
            {
                string ex = _extension;
                if (ex == null)
                    _extension = ex = System.IO.Path.GetExtension(this.Path);

                return ex;
            }
            private set { _extension = value; }
        }

        private string _name;

        [IgnoreDataMember]
        public string Name
        {
            get
            {
                string name = _name;
                if (name == null)
                    _name = name = System.IO.Path.GetFileName(this.Path);

                return name;
            }
            private set { _name = value; }
        }
        #endregion

        #region IHierarchicalUri Members

        public string Path 
        { 
            get;
            private set;
        }

        #endregion

        #region IAbsoluteOrRelativeUri Members

        public bool IsAbsolute
        {
            get { return this.Path.StartsWith("/") || this.Path.StartsWith("~/"); }
        }

        #endregion

        #region IHierarchicalUri Members

        private readonly Deferrable<UnixUrl> _parent;

        public UnixUrl Parent
        {
            get { return _parent.Value; }
        }
        
        private UnixUrl CreateParent()
        {
            if (!this.IsAbsolute)
                return new UnixUrl() { Path = "../" + this.Path };

            if (this.Path.Equals("/"))
                return null;

            int index = this.Path.LastIndexOf('/');

            if (index > -1)
            {
                if (index == 0)
                    return new UnixUrl() { Path = "/" };
                
                if (this.Path.Length > 1)
                    return new UnixUrl() { Path = this.Path.Remove(index) };
            }
            return null;
        }

        IHierarchicalUri IHierarchicalUri.Parent
        {
            get { return this.Parent; }
        }

        public bool HasParent
        {
            get { return _parent.IsValueActive; }
        }

        #endregion
    }
}
