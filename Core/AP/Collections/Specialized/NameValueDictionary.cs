using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AP.Reflection;

namespace AP.Collections.Specialized
{
    public class NameValueDictionary<T> : AP.Collections.Dictionary<string, T>
    {
        public static NameValueDictionary<T> FromObject(object obj)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);

            NameValueDictionary<T> dictionary = new NameValueDictionary<T>(properties.Count);

            Type t = typeof(T);

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                if (descriptor.PropertyType.Is(t))
                    dictionary.Add(descriptor.Name, (T)descriptor.GetValue(obj));
            }

            return dictionary;
        }

        public NameValueDictionary(StringComparison keyComparison = StringComparison.Ordinal, IEqualityComparer<T> valueComparer = null)
            : this(0, Strings.GetComparer(keyComparison), valueComparer)
        { }

        public NameValueDictionary(IEqualityComparer<string> keyComparer, IEqualityComparer<T> valueComparer) 
            : this(0, keyComparer, valueComparer)
        { }

        public NameValueDictionary(int capacity, StringComparison keyComparison = StringComparison.Ordinal, IEqualityComparer<T> valueComparer = null)
            : this(capacity, Strings.GetComparer(keyComparison), valueComparer)
        { }

        public NameValueDictionary(int capacity, IEqualityComparer<string> keyComparer, IEqualityComparer<T> valueComparer) 
            : base(capacity, keyComparer ?? StringComparer.Ordinal, valueComparer)
        { }
        
        public NameValueDictionary(IEnumerable<KeyValuePair<string, T>> dictionary, StringComparison keyComparison = StringComparison.Ordinal, IEqualityComparer<T> valueComparer = null)
            : this(dictionary, Strings.GetComparer(keyComparison), valueComparer)
        { }

        public NameValueDictionary(IEnumerable<KeyValuePair<string, T>> dictionary, IEqualityComparer<string> keyComparer, IEqualityComparer<T> valueComparer)
            : base(dictionary, keyComparer ?? StringComparer.Ordinal, valueComparer)
        { }
        
        protected NameValueDictionary(System.Collections.Generic.Dictionary<string, T> inner) 
            : base(inner)
        { }

        public new NameValueDictionary<T> Clone()
        {
            return (NameValueDictionary<T>)this.OnClone();
        }

        protected override Dictionary<string, T> OnClone()
        {
            return new NameValueDictionary<T>(this, this.KeyComparer, this.ValueComparer);
        }
    }
}
