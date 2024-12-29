using System;
using System.Collections.Generic;
using System.ComponentModel;
using AP.Reflection;

namespace AP.Collections.Specialized;

public class NameValueDictionary<T> : AP.Collections.Dictionary<string, T>
{
    public static NameValueDictionary<T> FromObject(object obj)
    {
        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);

        NameValueDictionary<T> dictionary = new(properties.Count);

        Type t = typeof(T);

        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
        {
            if (descriptor.PropertyType.Is(t))
                dictionary.Add(descriptor.Name, (T)descriptor.GetValue(obj)!);
        }

        return dictionary;
    }

    public NameValueDictionary()
        : this(0)
    { }

    public NameValueDictionary(StringComparison keyComparison = StringComparison.Ordinal, IEqualityComparer<T>? valueComparer = null)
        : this(0, Strings.GetComparer(keyComparison), valueComparer!)
    { }

    public NameValueDictionary(IEqualityComparer<string> keyComparer, IEqualityComparer<T> valueComparer) 
        : this(0, keyComparer, valueComparer)
    { }

    public NameValueDictionary(int capacity, StringComparison keyComparison = StringComparison.Ordinal, IEqualityComparer<T>? valueComparer = null)
        : this(capacity, Strings.GetComparer(keyComparison), valueComparer!)
    { }

    public NameValueDictionary(int capacity, IEqualityComparer<string> keyComparer, IEqualityComparer<T> valueComparer) 
        : base(capacity, keyComparer ?? StringComparer.Ordinal, valueComparer)
    { }

    public NameValueDictionary(params IEnumerable<KeyValuePair<string, T>> dictionary)
        : this(dictionary, null!, null!)
    { }

    public NameValueDictionary(IEnumerable<KeyValuePair<string, T>> dictionary, StringComparison keyComparison = StringComparison.Ordinal, IEqualityComparer<T>? valueComparer = null)
        : this(dictionary, Strings.GetComparer(keyComparison), valueComparer!)
    { }

    public NameValueDictionary(IEnumerable<KeyValuePair<string, T>> dictionary, IEqualityComparer<string> keyComparer, IEqualityComparer<T> valueComparer)
        : base(dictionary, keyComparer ?? StringComparer.Ordinal, valueComparer)
    { }
    
    protected NameValueDictionary(System.Collections.Generic.Dictionary<string, T> inner) 
        : base(inner)
    { }

    public new NameValueDictionary<T> Clone() => (NameValueDictionary<T>)this.OnClone();

    protected override Dictionary<string, T> OnClone() => new NameValueDictionary<T>(this, this.KeyComparer, this.ValueComparer);
}
