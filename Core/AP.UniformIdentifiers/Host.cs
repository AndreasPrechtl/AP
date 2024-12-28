using System;

namespace AP.UniformIdentifiers;

public abstract class Host : IComparable<Host>, IEquatable<Host>
{
    public static bool TryParse(string host, out Host? value)
    {
        if (IPv4.TryParse(host, out value))
            return true;
        
        if (IPv6.TryParse(host, out value))
            return true;
        
        if (NamedHost.TryParse(host, out value))
            return true;
        
        value = null;

        return false;
    }

    public static Host Parse(string host)
    {
        if (IPv4.TryParse(host, out var value))
            return value!;

        if (IPv6.TryParse(host, out value))
            return value!;
     
        return new NamedHost(host);
    }

    public static implicit operator string(Host host)
    {
        return host.Value;
    }

    public static explicit operator Host(string host)
    {
        return Parse(host);
    }

    public abstract string Value { get; }
    private readonly IComparable _valueInternal;
    
    public abstract bool IsLoopback
    {
        get;
    }
    
    internal Host(IComparable valueInternal)
    {
        _valueInternal = valueInternal;
    }

    public sealed override string ToString() => this.Value;

    public override bool Equals(object? obj)
    {
        if (obj is Host host)
            return this.Equals(host);

        return false;
    }

    public override int GetHashCode() => _valueInternal.GetHashCode();

    #region IEquatable<Host> Members

    public virtual bool Equals(Host? other)
    {
        if (other is null)
            return false;

        if (other == this)
            return true;
        
        return _valueInternal.Equals(other._valueInternal);
    }

    #endregion

    #region IComparable<Host> Members

    public virtual int CompareTo(Host? other)
    {
        if (other is null)
            return -1;

        if (other == this)
            return 0;
        
        if (other.GetType() == this.GetType())
            return _valueInternal.CompareTo(other._valueInternal);

        return this.Value.CompareTo(other.Value, StringComparison.InvariantCultureIgnoreCase);
    }
    
    #endregion

    public abstract Host Loopback { get; }
}    
