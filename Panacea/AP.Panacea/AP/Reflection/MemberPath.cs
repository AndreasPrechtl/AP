using AP.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Linq;

namespace AP.Reflection
{
    [Serializable]
    public sealed class MemberPath : IComparable
    {
        public sealed class SegmentList : AP.Collections.ReadOnly.ReadOnlyList<string>
        {
            internal readonly string _value;
            
            private SegmentList(AP.Collections.List<string> segments, string value)
                : base(segments)
            {
                _value = value;
            }

            internal static SegmentList Create(IEnumerable<string> segments)
            {
                if (segments == null)
                    throw new ArgumentNullException("segments");

                StringBuilder sb = new StringBuilder();
                AP.Collections.List<string> list = new AP.Collections.List<string>();

                foreach (string s in segments)
                {
                    string[] split = s.Split('.');

                    if (split.Length > 0)
                    {
                        foreach (string s0 in split)
                        {
                            string t = s0.Trim();
                            if (!t.IsNullOrWhiteSpace())
                            {
                                list.Add(t);
                                sb.Append(t);
                                sb.Append('.');
                            }
                        }
                    }
                    else
                    {
                        string t = s.Trim();
                        if (!t.IsNullOrWhiteSpace())
                        {
                            list.Add(t);
                            sb.Append(t);
                            sb.Append('.');
                        }
                    }
                }

                if (list.Count > 0)
                {
                    // remove the last '.'
                    sb.Remove(sb.Length - 1, 1);
                }

                return new SegmentList(list, sb.ToString());
            }

            public new SegmentList Clone()
            {
                return this;
            }
        }

        private readonly string _name;
        private readonly SegmentList _segments;
        private static volatile MemberPath _empty;
        
        public static MemberPath Empty 
        { 
            get             
            {
                MemberPath empty = _empty;

                if (empty == null)
                    _empty = empty = new MemberPath(string.Empty);

                return empty;             
            } 
        }

        public SegmentList Segments
        {
            get { return _segments; }
        }

        public MemberPath(string memberPath)
            : this(New.Enumerable<string>(memberPath))
        { }

        public MemberPath(IEnumerable<string> segments)
        {
            SegmentList s = SegmentList.Create(segments);
            _segments = s;
            _name = s[s.Count - 1];
        }

        /// <summary>
        /// Gets the member name.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the string representation.
        /// </summary>
        public string Value
        {
            get
            {
                return _segments._value;
            }
        }

        /// <summary>
        /// Gets the total length of the string representation (including separators).
        /// </summary>
        public int Length
        {
            get { return _segments._value.Length; }
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;

            if (obj == null)
                return false;

            if (obj is MemberPath)
                return this.Value.Equals(((MemberPath)obj).Value, StringComparison.OrdinalIgnoreCase);

            return this.Value.Equals(obj.ToString(), StringComparison.OrdinalIgnoreCase);
        }
        
        public static implicit operator MemberPath(string path)
        {
            return path != null ? new MemberPath(path) : null;
        }

        public static implicit operator string(MemberPath path)
        {
            return path != null ? path.Value : null;
        }

        int IComparable.CompareTo(object obj)
        {
            if (this.Equals(obj))
                return 0;
            else
                return _segments._value.CompareTo(obj);
        }
    }
}
