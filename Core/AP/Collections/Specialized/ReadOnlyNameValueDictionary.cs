﻿using System;
using System.Collections.Generic;

namespace AP.Collections.Specialized;

public class ReadOnlyNameValueDictionary<T> : AP.Collections.ReadOnly.ReadOnlyDictionary<string, T>
{
    private static readonly ReadOnlyNameValueDictionary<T> s_empty = new([]);

    public new static ReadOnlyNameValueDictionary<T> Empty => s_empty;
    
    public static ReadOnlyNameValueDictionary<T> FromObject(object obj) => new(NameValueDictionary<T>.FromObject(obj));

    public ReadOnlyNameValueDictionary(params IEnumerable<KeyValuePair<string, T>> dictionary)
        : this(dictionary, null!, null!)
    { }

    public ReadOnlyNameValueDictionary(IEnumerable<KeyValuePair<string, T>> dictionary, StringComparison keyComparison = StringComparison.Ordinal) 
        : this(dictionary, Strings.GetComparer(keyComparison), null!)
    { }

    public ReadOnlyNameValueDictionary(IEnumerable<KeyValuePair<string, T>> dictionary, IEqualityComparer<string> keyComparer, IEqualityComparer<T> valueComparer)
        : base(dictionary, keyComparer ?? StringComparer.Ordinal, valueComparer)
    { }

    protected ReadOnlyNameValueDictionary(AP.Collections.Dictionary<string, T> inner) 
        : base(inner)
    { }

    public new ReadOnlyNameValueDictionary<T> Clone() => this;
}
