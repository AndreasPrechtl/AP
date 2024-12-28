using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP.ComponentModel;
using AP.ComponentModel.Conversion;

namespace AP
{
    [NumericType]
    public interface INumeric<T> : IIdentifier
        where T : struct, INumeric<T>
    {
        /// <summary>
        /// Converts one numeric to another numeric type - as the current numeric should know best how to handle conversion to different types
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <returns></returns>
        TOutput Convert<TOutput>() where TOutput : struct, INumeric<TOutput>;

        T Add(T value);
        T Substract(T value);
        T Multiply(T value);
        T Divide(T value);

        // shifts a number of bytes - negative: left, positive: right
        T Shift(int bits);
        
        //INumeric Convert(INumeric value, Type outputType);
    }

    public interface IMatrix<T>: INumeric<T>
        where T : struct, IMatrix<T>
    {
        T Cross(T value);
        T Product(T value);
        bool IsIdentity { get; }
    }
    
    [NumericType]
    public struct Int32 : INumeric<Int32>
    {
        private readonly int _value;

        private Int32(int value)
        {
            _value = value;
        }

        #region IIdentifier Members

        bool IIdentifier.Equals(object other)
        {
            throw new NotImplementedException();
        }

        int IIdentifier.GetHashCode()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IComparable<IIdentifier> Members

        int IComparable<IIdentifier>.CompareTo(IIdentifier other)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEquatable<IIdentifier> Members

        bool IEquatable<IIdentifier>.Equals(IIdentifier other)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region INumeric<Int32> Members

        public TOutput Convert<TOutput>() where TOutput : struct, INumeric<TOutput>
        {
            throw new NotImplementedException();
        }

        public Int32 Add(Int32 value)
        {
            return new Int32(_value + value._value);
        }

        public Int32 Substract(Int32 value)
        {
            return new Int32(_value - value._value);
        }

        public Int32 Multiply(Int32 value)
        {
            throw new NotImplementedException();
        }

        public Int32 Divide(Int32 value)
        {
            throw new NotImplementedException();
        }

        public Int32 Shift(int bits)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEquatable<Int32> Members

        public bool Equals(Int32 other)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IComparable<Int32> Members

        public int CompareTo(Int32 other)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
