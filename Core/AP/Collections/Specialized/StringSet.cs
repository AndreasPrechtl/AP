using System.Collections.Generic;
using System.ComponentModel;
using System;
using AP.Linq;

namespace AP.Collections.Specialized;

[TypeConverter(typeof(StringSetConverter))]
[Serializable]
public class StringSet : Set<string>, IStringEnumerable
{
    public StringSet(string value, string separator = StringEnumerable.DefaultSeparator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries, StringComparer? comparer = null)
        : this(value.Split(separator, options), comparer)
    { }

    public StringSet(IEnumerable<string> collection, IEqualityComparer<string> comparer)
        : base(collection, comparer ?? StringComparer.Ordinal)
    { }

    public StringSet(IEqualityComparer<string> comparer)
        : this(null, comparer)
    { }

    public StringSet(IEnumerable<string> collection, StringComparison comparisonType = StringComparison.Ordinal)
        : this(collection, Strings.GetComparer(comparisonType))
    { }

    public StringSet(StringComparison comparisonType = StringComparison.Ordinal)
        : this(null, Strings.GetComparer(comparisonType))
    { }

    public new StringSet Clone() => (StringSet)this.OnClone();

    protected override Set<string> OnClone() => new StringSet(this, this.Comparer);

    public override string ToString() => this.ToString(StringEnumerable.DefaultSeparator);

    #region IStringEnumerable Members

    public virtual string ToString(string separator = StringEnumerable.DefaultSeparator, bool filterWhitespaces = true) => StringEnumerableHelperInternal.ToString(this, separator, filterWhitespaces);

    public virtual bool Contains(string value, StringComparison comparisonType = StringComparison.Ordinal)
    {
        StringComparer sc = Strings.GetComparer(comparisonType);

        if (this.Comparer == sc)
            return this.Contains(value);

        return StringEnumerableHelperInternal.Contains(this, value, comparisonType);
    }

    #endregion
}

