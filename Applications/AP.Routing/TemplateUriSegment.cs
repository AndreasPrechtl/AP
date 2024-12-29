using System;
using System.ComponentModel;

namespace AP.Routing
{
    public sealed class TemplateUriSegment : UriSegment
    {
        private readonly string _name;

        public TemplateUriSegment(string name)
            : base(string.Format("{{0}}", name), UriSegmentType.Template)
        {
            if (name.IsNullOrWhiteSpace())
                throw new ArgumentException("name");

            _name = name;
        }

        public string Name { get { return _name; } }
    }
}