using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.UniformIdentifiers
{
    public abstract class Host : IComparable<Host>, IEquatable<Host>
    {
        public static bool TryParse(string host, out Host value)
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
            Host value = null;
            if (IPv4.TryParse(host, out value))
                return value;
            
            if (IPv6.TryParse(host, out value))
                return value;
         
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

        public sealed override string ToString()
        {
            return this.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Host)
                return this.Equals((Host)obj);

            return false;
        }

        public override int GetHashCode()
        {
            return _valueInternal.GetHashCode();
        }

        #region IEquatable<Host> Members

        public virtual bool Equals(Host other)
        {
            if (other == this)
                return true;
            
            if (other == null)
                return false;

            return _valueInternal.Equals(other._valueInternal);
        }

        #endregion

        #region IComparable<Host> Members

        public virtual int CompareTo(Host other)
        {
            if (other == this)
                return 0;

            if (other == null)
                return -1;

            if (other.GetType() == this.GetType())
                return _valueInternal.CompareTo(other._valueInternal);

            return this.Value.CompareTo(other.Value, StringComparison.InvariantCultureIgnoreCase);
        }
        
        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is Host)
                return this.CompareTo((Host)obj);

            if (obj is IComparable)
                return _valueInternal.CompareTo(obj);

            return -1;
        }

        #endregion

        public abstract Host Loopback { get; }
    }    
}
