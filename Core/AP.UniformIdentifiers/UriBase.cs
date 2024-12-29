using System;
using System.Text;

namespace AP.UniformIdentifiers;

public abstract class UriBase : IUri
{
    private string _originalString;

    /// <summary>
    /// Returns the string that Uri originally originates from
    /// </summary>
    public string OriginalString
    {
        get => _originalString;
        protected set => _originalString = value;
    }

    public bool HasOriginalString => _originalString != null;

    public abstract string Scheme
    {
        get; 
    }

    //public string FullName { get; protected set; }

    private readonly Lazy<string> _fullName;

    protected UriBase()
    {
        _fullName = new Lazy<string>
        (
            () =>
            {
                StringBuilder sb = new();
                this.BuildFullName(ref sb);
                return sb.ToString();
            },
            false
        );
    }

    public string FullName
    {
        get => _fullName.Value;     
    }

    protected virtual void BuildFullName(ref StringBuilder builder) => builder.Append(this.Scheme);

    #region IUri Members

    public sealed override string ToString() => this.FullName;

    public override bool Equals(object? obj) => this.Equals(obj, true);

    public virtual bool Equals(object? obj, bool ignoreCase = true)
    {   
        if (obj == this)
            return true;
        
        StringComparison sc = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        // just in case the implementation of ToString() may be flawed (by not being derived of UriBase)
        if (obj is IUri other)
            return this.FullName.Equals(other.FullName, sc);

        return false;
    }

    public override int GetHashCode() => this.FullName.GetHashCode();

    #endregion

    #region IEquatable<IUri> Members

    public virtual bool Equals(IUri? other)
    {
        if (other is null)
            return false;

        if (other == this)
            return true;

        return this.FullName.Equals(other.FullName, StringComparison.InvariantCultureIgnoreCase);
    }

    #endregion

    #region IComparable<UriBase> Members

    public int CompareTo(IUri? other)
    {
        if (other is null)
            return 1;

        if (other == this)
            return 0;
                
        return this.FullName.CompareTo(other.FullName, StringComparison.InvariantCultureIgnoreCase);
    }

    #endregion
}