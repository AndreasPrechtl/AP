using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AP.Linq;

namespace AP
{
    // this class can - as soon as the locator pattern is ready - be trashed

    [Serializable]
    public class Path : IPath, ISerializable
    {
        public Path(string path, string separator = "/")
        {
            this.FullName = path;
            _separator = separator;
        }

        public Path(IEnumerable<string> segments, string separator = "/")
        {
            this.Segments = segments;
            _separator = separator;
        }

        public Path(IEnumerable<object> segments, string separator = "/")
            : this(segments.Select(Convert.ToString), separator)
        { }
        
        public string Name
        {
            get
            {
                string n = _name;
                if (n == null)
                    _name = n = this.Segments.Last();
                
                return n;
            }
            private set
            {
                _name = value;
            }
        }

        private string _fullName;
        private string _name;
        private string _separator;
        private IEnumerable<string> _segments;

        public string FullName
        {
            get
            {
                string n = _fullName;

                if (n == null)
                {
                    string separator = _separator;
                    StringBuilder sb = new StringBuilder();
                    foreach (string s in this.Segments)
                    {
                        sb.Append(s);
                        sb.Append(separator);
                    }
                    this.FullName = n = sb.ToString();
                }

                return n;
            }
            private set
            {
                _fullName = value;
            }
        }

        public IEnumerable<string> Segments
        {
            get
            {
                IEnumerable<string> segments = _segments;

                if (segments == null)
                    this.Segments = segments = _fullName.Split(_separator).Select(p => p.Trim()).ToArray();

                return segments;
            }
            private set
            {
                _segments = value;
            }
        }
        
        public string Separator
        {
            get { return _separator; }
            private set { _separator = value; }
        }

        #region ISerializable

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.GetObjectData(info, context);
        }

        protected Path(SerializationInfo info, StreamingContext context)
        {
            this._fullName = (string)info.GetValue("FullName", typeof(string));
        }

        protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FullName", this.FullName);

            string separator = _separator;
            if (separator != null)
                info.AddValue("Separator", separator);
        }

        #endregion
        
        // compatibility patches // will be removed whenever I come to rewriting the whole file manager, the mvc file managers, panacea + mvc
        // todo.

        public static string GetExtension(string fileName)
        {
            return System.IO.Path.GetExtension(fileName);
        }

        public static string GetFileNameWithoutExtension(string fileName)
        {
            return System.IO.Path.GetFileNameWithoutExtension(fileName);
        }

        public static string Combine(params string[] paths)
        {
            return System.IO.Path.Combine(paths);
        }
    }
}
