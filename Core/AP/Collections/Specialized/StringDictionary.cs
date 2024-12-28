using System;
using System.Collections.Generic;

namespace AP.Collections.Specialized;

public class StringDictionary : NameValueDictionary<string>
{
    public StringDictionary(StringComparison keyComparison = StringComparison.Ordinal, StringComparison valueComparison = StringComparison.Ordinal)
        : this(0, Strings.GetComparer(keyComparison), Strings.GetComparer(valueComparison))
    { }

    public StringDictionary(int capacity, StringComparison keyComparison = StringComparison.Ordinal, StringComparison valueComparison = StringComparison.Ordinal) 
        : this(capacity, Strings.GetComparer(keyComparison), Strings.GetComparer(valueComparison))
    { }
    
    public StringDictionary(IEqualityComparer<string> keyComparer, IEqualityComparer<string> valueComparer)
        : this(0, keyComparer, valueComparer)
    { }
    
    public StringDictionary(int capacity, IEqualityComparer<string> keyComparer, IEqualityComparer<string> valueComparer)
        : base(capacity, keyComparer ?? StringComparer.Ordinal, valueComparer ?? StringComparer.Ordinal)
    { }
    
    public StringDictionary(params IEnumerable<KeyValuePair<string, string>> dictionary)
        : base(dictionary)
    { }
    
    public StringDictionary(IEnumerable<KeyValuePair<string, string>> dictionary, IEqualityComparer<string> keyComparer, IEqualityComparer<string> valueComparer)
        : base(dictionary, keyComparer ?? StringComparer.Ordinal, valueComparer ?? StringComparer.Ordinal)
    { }

    protected StringDictionary(Dictionary<string, string> inner) 
        : base(inner)
    { }

    public new StringDictionary Clone() => (StringDictionary)this.OnClone();

    protected override Dictionary<string, string> OnClone() => new StringDictionary(this, this.KeyComparer, this.ValueComparer);
}
