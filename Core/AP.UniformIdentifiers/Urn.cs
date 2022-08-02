using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.UniformIdentifiers
{
    [Serializable]
    public abstract class Urn : UriBase
    {
        protected override void BuildFullName(ref StringBuilder builder)
        {
            builder.Append(base.OriginalString);
        }

        protected Urn(string urn, string detail)            
         //   : base(urn)
        { }
    }
}
