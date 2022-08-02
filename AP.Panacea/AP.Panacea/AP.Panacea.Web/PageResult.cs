using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.UniformIdentifiers;

namespace AP.Panacea.Web
{
    public class PageResult
    {
        public IUri Uri { get; private set; }
        
        public PageResult(IUri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            this.Uri = uri;
        }
    }
}
