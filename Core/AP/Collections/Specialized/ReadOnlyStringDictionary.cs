using System;
using System.Collections.Generic;

namespace AP.Collections.Specialized;

public class ReadOnlyStringDictionary : ReadOnlyNameValueDictionary<string>
{
    public static readonly ReadOnlyStringDictionary s_empty = new([]);

    public new static ReadOnlyStringDictionary Empty => s_empty;

    public ReadOnlyStringDictionary(params IEnumerable<KeyValuePair<string, string>> dictionary) 
        : this(dictionary, null!, null!)
    { }

    public ReadOnlyStringDictionary(IEnumerable<KeyValuePair<string, string>> dictionary, StringComparer keyComparer, StringComparer valueComparer)
        : base(dictionary, keyComparer ?? StringComparer.Ordinal, valueComparer ?? StringComparer.Ordinal)
    { }

    protected ReadOnlyStringDictionary(Dictionary<string, string> inner) 
        : base(inner)
    { }

    public new ReadOnlyStringDictionary Clone() => this;
}
