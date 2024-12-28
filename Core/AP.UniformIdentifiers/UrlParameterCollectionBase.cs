using System;
using System.Collections.Generic;
using System.Diagnostics;
using AP.Collections;
using AP.Collections.ReadOnly;
using AP.Collections.Specialized;
using AP.Linq;
using System.Text;

namespace AP.UniformIdentifiers;

[DebuggerDisplay("{Value}")]
public abstract class UrlParameterCollectionBase : ReadOnlyDictionary<string, ISetView<string>>
{
    private readonly Deferrable<string> _value;

    public string Value => _value.Value;

    private static AP.Collections.Dictionary<string, ISetView<string>> GetDictionary(IEnumerable<KeyValuePair<string, IEnumerable<string>>> dictionary, StringComparer keyComparer, StringComparer valueComparer)
    {
        ArgumentNullException.ThrowIfNull(dictionary);

        keyComparer = keyComparer ?? StringComparer.InvariantCultureIgnoreCase;
        valueComparer = valueComparer ?? StringComparer.InvariantCultureIgnoreCase;

        Collections.Dictionary<string, ISetView<string>> inner = new(keyComparer, null);

        foreach (var kvp in dictionary)
        {
            string key = kvp.Key;

            if (inner.Contains(kvp.Key, out ISetView<string> sv))
                ((StringSet)sv).Add(kvp.Value);
            else
            {
                sv = new StringSet(kvp.Value, valueComparer);
                inner.Add(key, sv);
            }
        }

        return inner;
    }

    public UrlParameterCollectionBase(IEnumerable<KeyValuePair<string, IEnumerable<string>>> dictionary, StringComparison keyComparison = StringComparison.Ordinal, StringComparison valueComparison = StringComparison.Ordinal)
        : this(dictionary, Strings.GetComparer(keyComparison), Strings.GetComparer(valueComparison))
    { }

    public UrlParameterCollectionBase(IEnumerable<KeyValuePair<string, IEnumerable<string>>> dictionary, StringComparer keyComparer, StringComparer valueComparer)
        : this(GetDictionary(dictionary, keyComparer, valueComparer))
    { }

    public UrlParameterCollectionBase(string parameters)
        : this(FromString(parameters))
    { }

    protected UrlParameterCollectionBase(IDictionaryView<string, ISetView<string>> inner)
        : base(inner)
    {
        _value = new Deferrable<string>
        (
            () =>
            {
                StringBuilder sb = new();
                BuildValue(ref sb);

                return sb.ToString();
            }
        );
    }

    protected UrlParameterCollectionBase()
        : this(new AP.Collections.Dictionary<string, ISetView<string>>(0))
    { }

    public static new UrlParameterCollectionBase Empty => throw new InvalidOperationException();

    protected static AP.Collections.Dictionary<string, ISetView<string>> FromString(string parameters)
    {
        StringComparer keyComparer = StringComparer.InvariantCultureIgnoreCase;
        StringComparer valueComparer = StringComparer.CurrentCultureIgnoreCase;

        var dict = new AP.Collections.Dictionary<string, ISetView<string>>(keyComparer, null);

        string[] kvps = parameters.Split(['&'], StringSplitOptions.RemoveEmptyEntries);

        foreach (string kvp in kvps)
        {
            string[] skvp = kvp.Split(['='], StringSplitOptions.RemoveEmptyEntries);

            string key = Uri.UnescapeDataString(skvp[0]);
            string value = null;

            if (skvp.Length > 1)
                value = Uri.UnescapeDataString(skvp[1]);


            if (!dict.Contains(key, out ISetView<string> sv))
            {
                sv = new StringSet(valueComparer);
                dict.Add(key, sv);
            }

            if (!value.IsNullOrEmpty())
                ((StringSet)sv).Add(value);
        }
        return dict;
    }

    protected virtual void BuildValue(ref StringBuilder builder)
    {
        foreach (var kvp in this)
        {
            if (kvp.Value.IsEmpty())
                builder.Append(kvp.Key);
            else
            {
                string key = kvp.Key;
                foreach (var value in kvp.Value)
                {
                    builder.Append(Uri.EscapeDataString(key));
                    builder.Append("=");
                    builder.Append(Uri.EscapeDataString(value));
                    builder.Append("&");
                }

                int l = builder.Length;
                if (l > 0)
                    builder.Remove(l - 1, 1);
            }
        }
    }

    public override string ToString() => this.Value;
}