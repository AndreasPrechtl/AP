using System;
using System.Collections.Generic;
using AP.Linq;

namespace AP.Collections.Specialized;

public class StringList : List<string>, IStringEnumerable
{
    public StringList(string value, string separator = StringEnumerable.DefaultSeparator, IEqualityComparer<string>? comparer = null)
        : this(value.Split(separator), comparer!)
    { }

    public StringList(StringComparison comparison = StringComparison.Ordinal)
        : this(0, Strings.GetComparer(comparison))
    { }

    public StringList(IEqualityComparer<string> comparer)
        : this(0, comparer)
    { }

    public StringList(int capacity, StringComparison comparison = StringComparison.Ordinal)
        : this(capacity, Strings.GetComparer(comparison))
    { }

    public StringList(int capacity, IEqualityComparer<string> comparer)
        : base(capacity, comparer ?? StringComparer.Ordinal)
    { }

    public StringList(IEnumerable<string> collection)
        : this(collection, null!)
    { }

    public StringList(IEnumerable<string> collection, StringComparison comparison = StringComparison.Ordinal)
        : this(collection, Strings.GetComparer(comparison))
    { }
    
    public StringList(IEnumerable<string> collection, IEqualityComparer<string> comparer)
        : base(collection, comparer ?? StringComparer.Ordinal)
    { }

    public new StringList Clone() => (StringList)this.OnClone();

    protected override List<string> OnClone() => new StringList(this, this.Comparer);

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
