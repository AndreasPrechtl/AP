using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using AP.Collections;
using System.Runtime.Serialization;
using AP.ComponentModel;

namespace AP.UniformIdentifiers
{
    [Serializable]
    public abstract class UriBase : IUri, ISerializable //System.Uri, IUri
    {
        private string _originalString;

        /// <summary>
        /// Returns the string that Uri originally originates from
        /// </summary>
        public string OriginalString
        {
            get { return _originalString; }
            protected set { _originalString = value; }
        }

        public bool HasOriginalString
        {
            get { return _originalString != null; }
        }

        public abstract string Scheme
        {
            get; 
        }

        //public string FullName { get; protected set; }

        private readonly Deferrable<string> _fullName;

        protected UriBase()
        {
            _fullName = new Deferrable<string>
            (
                () =>
                {
                    StringBuilder sb = new StringBuilder();
                    this.BuildFullName(ref sb);
                    return sb.ToString();
                },
                false
            );
        }

        public string FullName
        {
            get { return _fullName.Value; }
            protected set
            {
                _fullName.Value = value;
                //_fullName.IsReadOnly = true;
            }
        }

        protected virtual void BuildFullName(ref StringBuilder builder)
        {
            builder.Append(this.Scheme);
        }
     
        #region IUri Members
        
        public sealed override string ToString()
        {
            return this.FullName;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, true);
        }

        public virtual bool Equals(object obj, [DefaultValue(true)] bool ignoreCase)
        {            
            if (obj == null)
                return false;

            if (obj == this)
                return true;
            
            StringComparison sc = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            // just in case the implementation of ToString() may be flawed (by not being derived of UriBase)
            if (obj is IUri)
                return this.FullName.Equals(((IUri)obj).FullName, sc);
            
            // obsolete conversions -> ToString() should return what we need - without killing performance.
            // whereas - IUri's ToString implementation could be faulty and not return the fullname instead.
            
            //if (obj is IUri)
            //    return comparer.Equals(this.FullName, ((IUri)other).FullName);
            
            //if (obj is string)
            //    return comparer.Equals(this.FullName, (string)obj);

            //if (obj is System.Uri)
            //    return comparer.Equals(this.FullName, obj.ToString(), comparer);

            return this.FullName.Equals(obj.ToString(), sc);
        }

        public override int GetHashCode()
        {
            return this.FullName.GetHashCode();
        }
              
        #endregion

        //#region IEquatable<IUri> Members

        //public virtual bool Equals(IUri other)
        //{
        //    if (other == null)
        //        return false;
            
        //    if (other == this)
        //        return true;
            
        //    // removed - might be causing fishy behavior
        //    //if (!other.GetType().Is(this.GetType()))
        //    //    return false;

        //    return this.FullName.Equals(other.FullName, StringComparison.InvariantCultureIgnoreCase);
        //}

        //#endregion

        #region ISerializable Members

        protected virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            // adding the scheme should not be required as the typename itself should clarify which sort of uri was used
           // info.AddValue("Scheme", this.Scheme);
            info.AddValue("FullName", this.FullName);
        }

        void ISerializable.GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            this.GetObjectData(info, context);
        }

        #endregion

        //#region IComparable<UriBase> Members

        //public int CompareTo(IUri other)
        //{
        //    if (other == null)
        //        return 1;

        //    if (other == this)
        //        return 0;

        //    // removed - might be causing fishy behavior
        //    //if (!other.GetType().Is(this.GetType()))
        //    //    return 1;

        //    return this.FullName.CompareTo(other.FullName, StringComparison.InvariantCultureIgnoreCase);
        //}

        //#endregion

        //#region IComparable Members

        //public int CompareTo(object obj)
        //{
        //    if (obj == null)
        //        return 1;

        //    if (obj == this)
        //        return 0;

        //    if (obj is IUri)
        //        return this.CompareTo((IUri)obj);

        //    if (obj is System.Uri)
        //        return this.FullName.CompareTo(((System.Uri) obj).ToString(), StringComparison.InvariantCultureIgnoreCase);

        //    if (obj is string)
        //        return this.FullName.CompareTo((string)obj, StringComparison.InvariantCultureIgnoreCase);

        //    return this.FullName.CompareTo(obj.ToString(), StringComparison.InvariantCultureIgnoreCase);
        //}

        //#endregion

        //#region IComparable<Identifier> Members

        //int IComparable<ComponentModel.IIdentifier>.CompareTo(ComponentModel.IIdentifier other)
        //{
        //    return this.CompareTo(other);
        //}

        //#endregion

        //#region IEquatable<Identifier> Members

        //bool IEquatable<ComponentModel.IIdentifier>.Equals(ComponentModel.IIdentifier other)
        //{
        //    return this.Equals(other);
        //}

        //#endregion
    }
}
