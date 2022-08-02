using System.Net;
using System.Reflection;
using AP.Collections;
using AP.Collections.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AP.UniformIdentifiers
{
    public sealed class IPv6 : Host
    {
        private static readonly IPv6 _loopback;

        public override Host Loopback
        {
            get { return _loopback; }
        }

        private readonly Deferrable<ReadOnlyList<ushort>> _bits;
        private readonly Deferrable<string> _value;
        private readonly BigInteger _address;

        private static BigInteger ToAddress(ushort[] value)
        {
            if (value.Length != 8)
                ExceptionHelper.ThrowArgumentOutOfRangeException(() => value);

            return
                ((BigInteger)value[0] << 112) +
                ((BigInteger)value[1] << 96) +
                ((BigInteger)value[2] << 80) +
                ((BigInteger)value[3] << 64) +
                ((BigInteger)value[4] << 48) +
                ((BigInteger)value[5] << 32) +
                ((BigInteger)value[6] << 16) +
                ((BigInteger)value[7]);

            // compressed but ... slower
            //BigInteger big;
            //for (int i = 0; i < 8; i++)
            //    big += (BigInteger)value[i] << (128 - ((i + 1) * 16));
        }

        private static readonly BigInteger MaxValue;

        static IPv6()
        {
            MaxValue = ((BigInteger) 1 << 128) - 1;
            _numbersField = typeof(IPAddress).GetField("m_Numbers", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField);
            _loopback = new IPv6(1);
        }

        private static bool IsOutOfRange(BigInteger value)
        {
            return value < 0 || value > MaxValue;
        }

        private static ushort[] ToBits(BigInteger value)
        {
            if (IsOutOfRange(value))
                ExceptionHelper.ThrowArgumentOutOfRangeException(() => value);

            return new[]
            {
                (ushort)(value >> 112),
                (ushort)(value >> 96 & 0xffff),
                (ushort)(value >> 80 & 0xffff),
                (ushort)(value >> 64 & 0xffff),
                (ushort)(value >> 48 & 0xffff),
                (ushort)(value >> 32 & 0xffff),
                (ushort)(value >> 16 & 0xffff),
                (ushort)(value & 0xffff)
            };
        }

        private static readonly FieldInfo _numbersField;

        private static ushort[] ToBits(string value)
        {
            //string v = value.Trim();

            //if (v.Equals("localhost", StringComparison.InvariantCultureIgnoreCase))
            //    return new ushort[] { 0, 0, 0, 0, 0, 0, 0, 1 };

            // why roll your own when u can exploit some reflection for that?
            //IPAddress ip = IPAddress.Parse(v);

            IPAddress ip = IPAddress.Parse(value);

            if (ip.AddressFamily != System.Net.Sockets.AddressFamily.InterNetworkV6)
                ExceptionHelper.ThrowArgumentException(() => value, "not a valid IPv6 address");

            return (ushort[])_numbersField.GetValue(ip);
        }

        private static string ToString(BigInteger value)
        {
            return ToString(ToBits(value));
        }

        private static string ToString(IEnumerable<ushort> value)
        {
            StringBuilder sb = new StringBuilder(71);

            foreach (ushort b in value)
            {
                if (b != 0)
                    sb.Append(b.ToString("h"));

                sb.Append(':');
            }
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }


        public static new IPv6 Parse(string host)
        {
            return new IPv6(host);
        }

        public static new bool TryParse(string host, out Host value)
        {
            try
            {
                value = new IPv6(host);
                return true;
            }
            catch { }

            value = null;
            return false;
        }
        
        public IPv6(string value)
            : this(ToBits(value = value.Trim()))
        {
            _value.Value = value;
        }

        public IPv6(ushort[] value)
            : this(ToAddress(value))
        {
            _bits.Value = new ReadOnlyList<ushort>(value);
        }

        public IPv6(BigInteger value)
            : base((IComparable)value)
        {
            if (IsOutOfRange(value))
                ExceptionHelper.ThrowArgumentOutOfRangeException(() => value);
            
            _address = value;
            _bits = new Deferrable<ReadOnlyList<ushort>>(() => new ReadOnlyList<ushort>(ToBits(_address)));
            _value = new Deferrable<string>(() => ToString(this.Bits));
        }

        public BigInteger Address
        {
            get { return _address; }
        }

        public override string Value
        {
            get { return _value.Value; }
        }

        public ReadOnlyList<ushort> Bits
        {
            get { return _bits.Value; }
        }

        public override bool IsLoopback
        {
            get { return _address.Equals((BigInteger)1); }
        }
    }
}
