using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AP.Web.Html
{
    [Serializable]
    public class HtmlString : IHtmlString, IComparable, ICloneable, IComparable<string>, IComparable<HtmlString>, IEquatable<string>, IEquatable<HtmlString>
    {
        private readonly string _value;

        public static readonly HtmlString Empty = new HtmlString(string.Empty);

        public HtmlString(string value)
        {
            _value = value;
        }

        #region IHtmlString Members

        string IHtmlString.ToHtmlString()
        {
            return _value;
        }

        #endregion

        public static implicit operator string(HtmlString htmlString)
        {
            return htmlString._value;
        }

        public static implicit operator HtmlString(string value)
        {
            return new HtmlString(value);
        }

        public override string ToString()
        {
            return _value;
        }

        public override bool Equals(object obj)
        {
            return _value.Equals(obj);
        }

        public override int GetHashCode()
        {
            string v = _value;

            return v != null ? v.GetHashCode() : 0;
        }

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            if (_value == null)
                return 1;
            
            return _value.CompareTo(obj);
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return this;
        }

        #endregion

        #region IComparable<string> Members

        int IComparable<string>.CompareTo(string other)
        {
            return _value.CompareTo(other);
        }

        #endregion

        #region IComparable<HtmlString> Members

        public int CompareTo(HtmlString other)
        {
            return _value.CompareTo(other._value);
        }

        #endregion

        #region IEquatable<HtmlString> Members

        public bool Equals(HtmlString other)
        {
            return _value.Equals(other._value);
        }

        #endregion

        #region IEquatable<string> Members

        public bool Equals(string other)
        {
            return _value.Equals(other);
        }

        #endregion
    }
}
