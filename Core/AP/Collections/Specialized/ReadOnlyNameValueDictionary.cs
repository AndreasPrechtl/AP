using System;
using System.Collections.Generic;

namespace AP.Collections.Specialized;

[Serializable, System.ComponentModel.ReadOnly(true)]
public class ReadOnlyNameValueDictionary<T> : AP.Collections.ReadOnly.ReadOnlyDictionary<string, T>
{
    private static volatile ReadOnlyNameValueDictionary<T> _empty;

    public new static ReadOnlyNameValueDictionary<T> Empty
    {
        get
        {
            ReadOnlyNameValueDictionary<T> empty = _empty;

            if (empty == null)
                _empty = empty = new ReadOnlyNameValueDictionary<T>(new AP.Collections.Dictionary<string, T>(0, StringComparer.Ordinal, EqualityComparer<T>.Default));

            return empty;
        }
    }

    public static ReadOnlyNameValueDictionary<T> FromObject(object obj) => new(NameValueDictionary<T>.FromObject(obj));

    public ReadOnlyNameValueDictionary(IEnumerable<KeyValuePair<string, T>> dictionary, StringComparison keyComparison = StringComparison.Ordinal) 
        : this(dictionary, Strings.GetComparer(keyComparison), null)
    { }

    public ReadOnlyNameValueDictionary(IEnumerable<KeyValuePair<string, T>> dictionary, IEqualityComparer<string> keyComparer, IEqualityComparer<T> valueComparer)
        : base(dictionary, keyComparer ?? StringComparer.Ordinal, valueComparer)
    { }

    protected ReadOnlyNameValueDictionary(AP.Collections.Dictionary<string, T> inner) 
        : base(inner)
    { }

    public new ReadOnlyNameValueDictionary<T> Clone() => this;
}
