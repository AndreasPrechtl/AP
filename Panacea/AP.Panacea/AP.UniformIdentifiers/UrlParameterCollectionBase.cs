using System;
using System.Collections.Generic;
using System.Diagnostics;
using AP.Collections;
using AP.Collections.ReadOnly;
using AP.Collections.Specialized;
using AP.Linq;
using AP.Collections.ObjectModel;
using System.Text;

namespace AP.UniformIdentifiers
{
    [DebuggerDisplay("{Value}")]
    public abstract class UrlParameterCollectionBase : ReadOnlyDictionary<string, ISetView<string>>
    {
        private readonly Deferrable<string> _value;
        
        public string Value
        {
            get { return _value.Value; }
        }
        
        private static AP.Collections.Dictionary<string, ISetView<string>> GetDictionary(IEnumerable<KeyValuePair<string, IEnumerable<string>>> dictionary, StringComparer keyComparer, StringComparer valueComparer)
        {
            ExceptionHelper.AssertNotNull(() => dictionary);

            keyComparer = keyComparer ?? StringComparer.InvariantCultureIgnoreCase;
            valueComparer = valueComparer ?? StringComparer.InvariantCultureIgnoreCase;

            Collections.Dictionary<string, ISetView<string>> inner = new Collections.Dictionary<string, ISetView<string>>(keyComparer, null);

            foreach (var kvp in dictionary)
            {
                ISetView<string> sv = null;
                string key = kvp.Key;

                if (inner.Contains(kvp.Key, out sv))
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
                    StringBuilder sb = new StringBuilder();
                    BuildValue(ref sb);

                    return sb.ToString();
                }
            );
        }

        protected UrlParameterCollectionBase()
            : this(new AP.Collections.Dictionary<string, ISetView<string>>(0))
        { }

        public static new UrlParameterCollectionBase Empty
        {
            get 
            {
                throw new InvalidOperationException();
            }
        }

        protected static AP.Collections.Dictionary<string, ISetView<string>> FromString(string parameters)
        {
            StringComparer keyComparer = StringComparer.InvariantCultureIgnoreCase;
            StringComparer valueComparer = StringComparer.CurrentCultureIgnoreCase;

            var dict = new AP.Collections.Dictionary<string, ISetView<string>>(keyComparer, null);

            string[] kvps = parameters.Split(new [] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string kvp in kvps)
            {
                string[] skvp = kvp.Split(new [] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                string key = Uri.UnescapeDataString(skvp[0]);
                string value = null;

                if (skvp.Length > 1)
                    value = Uri.UnescapeDataString(skvp[1]);

                ISetView<string> sv = null;

                if (!dict.Contains(key, out sv))
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

        public override string ToString()
        {
            return this.Value;
        }
    }
}