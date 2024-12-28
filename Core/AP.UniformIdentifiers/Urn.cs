using System;
using System.Text;

namespace AP.UniformIdentifiers;

[Serializable]
public abstract class Urn : UriBase
{
    protected override void BuildFullName(ref StringBuilder builder) => builder.Append(base.OriginalString);

    protected Urn(string urn, string detail)            
     //   : base(urn)
    { }
}
