using AP.Collections.ReadOnly;
using System;
using System.Collections.Generic;
using System.Text;

namespace AP.UniformIdentifiers;

public sealed class IPv4 : Host
{
    private static readonly IPv4 s_loopback = new(16777343);

    private static uint ToAddress(byte[] value)
    {
        if (value.Length != 4)
            ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, 4);

        return
            ((uint)value[0]) +
            ((uint)value[1] << 0x8) +
            ((uint)value[2] << 0x10) +
            ((uint)value[3] << 0x18);
    }

    private static byte[] ToBits(uint value) => BitConverter.GetBytes(value);

    private static string ToString(IEnumerable<byte> bits)
    {
        StringBuilder sb = new(13);

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

        string[] split = v.Split(['.'], StringSplitOptions.RemoveEmptyEntries);

        if (split.Length != 4)
            ArgumentOutOfRangeException.ThrowIfNotEqual(split.Length, 4, nameof(value));

        return
        [
            byte.Parse(split[0]),
            byte.Parse(split[1]),
            byte.Parse(split[2]),
            byte.Parse(split[3])
        ];
    }

    public override Host Loopback => s_loopback;

    private readonly Lazy<ReadOnlyList<byte>> _bits;
    private readonly Lazy<string> _value;
    private readonly uint _address;

    public static new IPv4 Parse(string host) => new(host);

    public static new bool TryParse(string host, out Host? value)
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
        _value = new(value);
    }

    public IPv4(byte[] value)
        : this(ToAddress(value))
    {
        // safe to set - it is not readonly - and the deferrable has already been initialized by the uint ctor - and the byte array is already good to be used
        _bits = new(new ReadOnlyList<byte>(value));
    }

    public IPv4(uint value)
        : base((IComparable)value)
    {
        _address = value;
        _bits = new(() => new ReadOnlyList<byte>(ToBits(_address)));
        _value = new(() => ToString(this.Bits));
    }

    public uint Address => _address;

    public override string Value => _value.Value;

    public ReadOnlyList<byte> Bits => _bits.Value;

    public override bool IsLoopback => _address == s_loopback.Address;
}
