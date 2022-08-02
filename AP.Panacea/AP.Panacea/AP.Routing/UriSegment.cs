using System;
namespace AP.Routing
{
    public abstract class UriSegment
    {
        private readonly string _value;
        private readonly UriSegmentType _type;
                
        internal UriSegment(string value, UriSegmentType type)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            
            _value = value;
            _type = type;
        }

        public string Value { get { return _value; } }
        
        public UriSegmentType Type { get { return _type; } }

        public static FixedUriSegment Fixed(string value)
        {
            return new FixedUriSegment(value);
        }

        public static TemplateUriSegment Template(string name)
        {
            return new TemplateUriSegment(name);
        }
    }
}