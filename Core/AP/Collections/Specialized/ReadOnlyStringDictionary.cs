using System;
using System.Collections.Generic;

namespace AP.Collections.Specialized;

[Serializable, System.ComponentModel.ReadOnly(true)]
public class ReadOnlyStringDictionary : ReadOnlyNameValueDictionary<string>
{
    public static volatile ReadOnlyStringDictionary _empty;
    
    public new static ReadOnlyStringDictionary Empty
    {
        get
        {
            ReadOnlyStringDictionary empty = _empty;

            if (empty == null)
            {
                StringComparer sc = StringComparer.Ordinal;
                _empty = empty = new ReadOnlyStringDictionary(new AP.Collections.Dictionary<string, string>(0, sc, sc));
            }
            
            return empty;
        }
    }

    public ReadOnlyStringDictionary(IEnumerable<KeyValuePair<string, string>> dictionary) 
        : this(dictionary, null, null)
    { }

    public ReadOnlyStringDictionary(IEnumerable<KeyValuePair<string, string>> dictionary, StringComparer keyComparer, StringComparer valueComparer)
        : base(dictionary, keyComparer ?? StringComparer.Ordinal, valueComparer ?? StringComparer.Ordinal)
    { }

    protected ReadOnlyStringDictionary(Dictionary<string, string> inner) 
        : base(inner)
    { }

    public new ReadOnlyStringDictionary Clone() => this;
}
