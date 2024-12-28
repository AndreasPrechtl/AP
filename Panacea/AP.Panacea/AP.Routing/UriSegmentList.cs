using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Linq;
using AP.Collections;
using AP.Collections.ReadOnly;

namespace AP.Routing
{
    public sealed class UriSegmentList : AP.Collections.ReadOnly.ReadOnlyList<UriSegment>
    {       
        private static volatile UriSegmentList _empty;

        public static new UriSegmentList Empty
        {
            get
            {
                UriSegmentList empty = _empty;

                if (empty == null)
                    _empty = empty = new UriSegmentList(AP.Collections.ReadOnly.ReadOnlyList<UriSegment>.Empty);

                return empty;
            }
        }
                    
        /// <summary>
        /// Performance optimized constructor
        /// </summary>
        /// <param name="list"></param>
        private UriSegmentList(Tuple<AP.Collections.IListView<UriSegment>, string> wrapper)
            : base(wrapper.Item1)
        {
            _stringValue = wrapper.Item2;
        }

        public UriSegmentList(IEnumerable<UriSegment> segments)
            : this(CreateInnerList(segments))
        { }

        public UriSegmentList(string segments)
            : this(CreateInnerList(segments))
        { }

        private static Tuple<AP.Collections.IListView<UriSegment>, string> CreateInnerList(IEnumerable<UriSegment> segments)
        {
            if (segments == null)
                throw new ArgumentNullException("segments");

            StringBuilder sb = new StringBuilder();

            Set<string> names = new Set<string>();

            int count;
            AP.Collections.List<UriSegment> list = new AP.Collections.List<UriSegment>(segments.HasCount(out count) ? count : 12);

            UriSegment last = null;

            int i = 0;
            foreach (UriSegment segment in segments)
            {
                UriSegmentType currentType = segment.Type;

                if (last != null && last.Type == currentType && currentType == UriSegmentType.Template)
                    throw new ArgumentException("Fixed segment expected after a template.");

                switch (currentType)
                {
                    case UriSegmentType.Fixed:
                        string v = ((FixedUriSegment)segment).Value;
                        if (i > 0 && v == string.Empty)
                            throw new ArgumentException("The fixed segment was empty, only the first and only may be empty.");
                        sb.Append(v);
                        break;                    
                    case UriSegmentType.Template:
                        string name = ((TemplateUriSegment)segment).Name;
                        if (!names.Add(name))
                            throw new ArgumentException(string.Format("A template with the same name {0} already exists.", name));
                        sb.Append(name);                        
                        break;
                    default:
                        throw new ArgumentException("segment.Type");
                }

                list.Add(segment);
                last = segment;
                ++i;
            }

            return new Tuple<AP.Collections.IListView<UriSegment>, string>(list, sb.ToString());
        }

        private static Tuple<AP.Collections.IListView<UriSegment>, string> CreateInnerList(string segments)
        {
            if (segments == null)
                throw new ArgumentNullException("segments");

            string original = segments;
            
            AP.Collections.List<UriSegment> list = new AP.Collections.List<UriSegment>();            
                        
            // special case - enables routing for an empty string
            if (segments.Length == 0)
            {
                list.Add(new FixedUriSegment(original));
                return new Tuple<IListView<UriSegment>,string>(list, original);   
            }
            
            bool previousIsTemplate = false;

            Set<string> names = new Set<string>();

            for (int i = 0; i < segments.Length; ++i)
            {
                // template start
                if (segments[i] == '{')
                {
                    if (previousIsTemplate)
                        throw new ArgumentException("expected element was not a fixed segment");

                    int lastFixedIndex = i;

                    bool endFound = false;

                    for (; i < segments.Length; )
                    {
                        if (endFound = (segments[++i] == '}'))
                            break;
                    }

                    if (!endFound)
                        throw new ArgumentException("} expected");
                    
                    previousIsTemplate = true;

                    // create both segments
                    string value = segments.Substring(0, lastFixedIndex);

                    if (value == string.Empty)
                        throw new ArgumentException("The fixed segment was empty, only the first and only may be empty.");

                    FixedUriSegment fixedSegment = new FixedUriSegment(value);

                    // get the name for the next template
                    string name = segments.Substring(lastFixedIndex + 1, i - lastFixedIndex - 1);

                    if (!names.Add(name))
                        throw new ArgumentException(string.Format("A template with the same name {0} already exists.", name));

                    TemplateUriSegment templateSegment = new TemplateUriSegment(name);

                    // add the segments
                    list.Add(fixedSegment);
                    list.Add(templateSegment);
                    
                    // remove the unneeded parts and reset the index
                    segments = segments.Remove(0, i + 1);
                    i = -1;
                }
                else
                {
                    previousIsTemplate = false;
                }
            }

            // wasn't cleared - it's a fixed segment
            if (segments.Length > 0)
                list.Add(new FixedUriSegment(segments));

            return new Tuple<AP.Collections.IListView<UriSegment>, string>(list, original);
        }

        private string _stringValue;
            
        public new UriSegmentList Clone()
        {
            return this;
        }
            
        public override bool Equals(object obj)
        {
            return this.ToString().Equals(obj);
        }
            
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            string s = _stringValue;

            if (s != null) 
                return s;

            StringBuilder sb = new StringBuilder();

            foreach (UriSegment rs in this)
            {
                switch (rs.Type)
                {
                    case UriSegmentType.Fixed:
                        sb.Append(((FixedUriSegment)rs).Value);
                        break;
                    case UriSegmentType.Template:
                        sb.Append(((TemplateUriSegment)rs).Name);
                        break;
                }
            }

            _stringValue = s = sb.ToString();

            return s;
        }
    }    
}
