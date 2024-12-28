using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.ComponentModel
{
    /// <summary>
    /// Wraps an object that could be used as an IIdentifier 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Identifier<T> : IdentifierBase, IWrapper<T>
        where T : IComparable<T>, IEquatable<T>, IComparable
    {
        private readonly T _value;

        public Identifier(T value)
        {
            if (value.ReferenceEquals(null))
                throw new ArgumentNullException("value");

            _value = value;
        }

        public static implicit operator T(Identifier<T> id)
        {
            return id._value;
        }

        public static implicit operator Identifier<T>(T value)
        {
            return new Identifier<T>(value);
        }

        #region IWrapper<T> Members

        public T Value
        {
            get { return _value; }
        }

        #endregion
        
        public override bool Equals(object obj)
        {
            bool b = base.Equals(obj);

            if (!b)
                b = _value.Equals(obj);

            return b;
        }

        public override bool Equals(IIdentifier other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public override int CompareTo(object obj)
        {
            int i = base.CompareTo(obj);
            
            if (i != 0)
                i = _value.CompareTo(obj);

            return i;
        }

        public override int CompareTo(IIdentifier other)
        {
            if (other is Identifier<T>)
                return _value.CompareTo(((Identifier<T>)other)._value);

            return base.CompareTo(other);
        }
    }
}
