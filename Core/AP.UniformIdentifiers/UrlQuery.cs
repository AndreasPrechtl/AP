using System.Collections.Generic;

namespace AP.UniformIdentifiers;

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

    public sealed override string ToString() => this.Value;

    private static readonly UrlQuery s_empty = new();

    public new static UrlQuery Empty => s_empty;
}
