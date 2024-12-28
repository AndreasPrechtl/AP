using System.Collections.Generic;
using System.ComponentModel;
using System;
using AP.Collections.ObjectModel;
using AP.Collections.ReadOnly;
using AP.Linq;

namespace AP.Collections.Specialized
{
    [TypeConverter(typeof(StringSetConverter))]
    [Serializable, ReadOnly(true)]
    public class ReadOnlyStringSet : AP.Collections.ReadOnly.ReadOnlySet<string>, IStringEnumerable, IEqualityComparerUser<string>, IComparerUser<string>
    {
        public ReadOnlyStringSet(string value, string separator = StringEnumerable.DefaultSeparator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
            : this(value.Split(separator, options), StringComparison.Ordinal)
        { }

        public ReadOnlyStringSet(IEnumerable<string> collection)
            : this(collection, StringComparer.Ordinal)
        { }
        
        public ReadOnlyStringSet(IEnumerable<string> collection, StringComparison comparisonType = StringComparison.Ordinal)
            : base(collection, Strings.GetComparer(comparisonType))
        { }

        public ReadOnlyStringSet(IEnumerable<string> collection, StringComparer comparer)
            : base(collection, comparer)
        { }

        public new ReadOnlyStringSet Clone()
        {
            return this;
        }

        public override string ToString()
        {
            return this.ToString(StringEnumerable.DefaultSeparator);
        }

        #region IStringEnumerable Members

        public virtual string ToString(string separator = StringEnumerable.DefaultSeparator, bool filterWhitespaces = true)
        {
            return StringEnumerableHelperInternal.ToString(this, separator, filterWhitespaces);
        }

        public virtual bool Contains(string value, StringComparison comparisonType = StringComparison.Ordinal)
        {
            StringComparer sc = Strings.GetComparer(comparisonType);

            if (this.Comparer == sc)
                return this.Contains(value);
            
            return StringEnumerableHelperInternal.Contains(this, value, comparisonType);
        }

        #endregion

        public new StringComparer Comparer
        {
            get { return (StringComparer)base.Comparer; }
        }

        #region IComparerUser<string> Members

        IComparer<string> IComparerUser<string>.Comparer
        {
            get { return this.Comparer; }
        }

        #endregion

        #region IEqualityComparerUser<string> Members

        IEqualityComparer<string> IEqualityComparerUser<string>.Comparer
        {
            get { return this.Comparer; }
        }

        #endregion
    }
}

