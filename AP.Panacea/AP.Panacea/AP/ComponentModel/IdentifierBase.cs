namespace AP.ComponentModel
{
    public abstract class IdentifierBase : IIdentifier
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj == this)
                return true;

            if (obj is IIdentifier)
                return this.Equals((IIdentifier)obj);

            return false;
        }

        public abstract override int GetHashCode();

        #region IEquatable<SecurityId> Members

        public virtual bool Equals(IIdentifier other)
        {
            if (other == null)
                return false;

            if (other == this)
                return true;

            return false;
        }

        #endregion

        #region IComparable<SecurityId> Members

        public virtual int CompareTo(IIdentifier other)
        {
            if (other == null)
                return 1;

            if (other == this)
                return 0;

            return 1;
        }

        #endregion

        #region IComparable Members

        public virtual int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (obj == this)
                return 0;

            if (obj is IIdentifier)
                return this.CompareTo((IIdentifier)obj);

            return 1;
        }

        #endregion
    }
}