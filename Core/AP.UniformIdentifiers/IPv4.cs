using AP.Collections;
using AP.Collections.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AP.UniformIdentifiers
{
    public sealed class IPv4 : Host
    {
        private static readonly IPv4 _loopback;

        static IPv4()
        {
            _loopback = new IPv4(16777343);
        }

        private static uint ToAddress(byte[] value)
        {
            if (value.Length != 4)
                ExceptionHelper.ThrowArgumentOutOfRangeException(() => value);

            return
                ((uint)value[0]) +
                ((uint)value[1] << 0x8) +
                ((uint)value[2] << 0x10) +
                ((uint)value[3] << 0x18);
        }
        
        private static byte[] ToBits(uint value)
        {
            return BitConverter.GetBytes(value);
        }

        private static string ToString(uint value)
        {
            return ToString(ToBits(value));
        }

        private static string ToString(IEnumerable<byte> bits)
        {
            StringBuilder sb = new StringBuilder(13);

            foreach (byte b in bits)
            {
                sb.Append(b);
                sb.Append('.');
            }
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        private static byte[] ToBits(string value)
        {
            string v = value;

            //if (v.Equals("localhost", StringComparison.InvariantCultureIgnoreCase))
            //    return new byte[] { 127, 0, 0, 1 };

            string[] split = v.Split(new [] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (split.Length != 4)
                ExceptionHelper.ThrowArgumentException(() => value, "Invalid IPv4 Address");

            return new byte[]
            {
                byte.Parse(split[0]),
                byte.Parse(split[1]),
                byte.Parse(split[2]),
                byte.Parse(split[3])
            };
        }
        
        public override Host Loopback
        {
            get { return _loopback; }
        }

        private readonly Deferrable<ReadOnlyList<byte>> _bits;
        private readonly Deferrable<string> _value;
        private readonly uint _address;
        
        public static new IPv4 Parse(string host)
        {
            return new IPv4(host);
        }

        public static new bool TryParse(string host, out Host value)
        {
            try
            {
                value = new IPv4(host);
                return true;
            }
            catch
            { }

            value = null;
            return false;
        }
        
        public IPv4(string value)
            : this(ToBits(value = value.Trim()))
        {
            // safe - forced into becoming a uint - so if there's an error - it won't even use a crappy string
            _value.Value = value;
        }

        public IPv4(byte[] value)
            : this(ToAddress(value))
        {
            // safe to set - it is not readonly - and the deferrable has already been initialized by the uint ctor - and the byte array is already good to be used
            _bits.Value = new ReadOnlyList<byte>(value);
        }

        public IPv4(uint value)
            : base((IComparable)value)
        {
            _address = value;
            _bits = new Deferrable<ReadOnlyList<byte>>(() => new ReadOnlyList<byte>(ToBits(_address)));
            _value = new Deferrable<string>(() => ToString(this.Bits));
        }

        public uint Address
        {
            get { return _address; }
        }
        
        public override string Value
        {
            get { return _value.Value; }
        }

        public ReadOnlyList<byte> Bits
        {
            get { return _bits.Value; }
        }

        public override bool IsLoopback
        {
            get { return _address == 16777343; }
        }
    }
}
