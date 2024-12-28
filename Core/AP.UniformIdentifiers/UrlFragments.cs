using System.Collections.Generic;

namespace AP.UniformIdentifiers;

public class UrlFragments : UrlParameterCollectionBase
{
    public UrlFragments(IEnumerable<KeyValuePair<string, IEnumerable<string>>> fragments)
        : base(fragments)
    { }

    public UrlFragments(string fragments)
        : base(fragments)
    { }

    protected UrlFragments()
        : base()
    { }

    private static volatile UrlFragments _empty;

    public new static UrlFragments Empty
    {
        get 
        { 
            UrlFragments fragments = _empty;

            if (fragments == null)
                _empty = fragments = new UrlFragments();

            return fragments;
        }
    }
}
