using System;

namespace AP.Routing
{    
    public sealed class FixedUriSegment : UriSegment
    {
        public FixedUriSegment(string value)
            : base(value, UriSegmentType.Fixed)
        { }
    }
}