using AP.Collections;
using AP.Linq;
using AP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using AP.Collections.Specialized;

namespace AP.UniformIdentifiers
{
    // use a builtin collection type or make a customized version?
    // -> Parameters as property not as inherited
    // now still - ToString may become overly complex by different variables
    // (?foo=bar&food=tar&so=on)
    public class UrlQuery : UrlParameterCollectionBase
    {
        public UrlQuery(IEnumerable<KeyValuePair<string, IEnumerable<string>>> query)
            : base(query)
        { }

        public UrlQuery(string query)
            : base(query)
        { }

        protected UrlQuery()
            : base()
        { }

        public sealed override string ToString()
        {
            return this.Value;   
        }

        private static volatile UrlQuery _empty;

        public new static UrlQuery Empty
        {
            get
            {
                UrlQuery fragments = _empty;

                if (fragments == null)
                    _empty = fragments = new UrlQuery();

                return fragments;
            }
        }
    }
}
