using System.Net;
using System.Reflection;
using AP.Collections.ReadOnly;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AP.UniformIdentifiers;

public sealed class IPv6 : Host
{
    private static readonly IPv6 s_loopback;

    public override Host Loopback => s_loopback;

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
    }

    private static readonly BigInteger MaxValue;

    static IPv6()
    {
        MaxValue = ((BigInteger) 1 << 128) - 1;
        _numbersField = typeof(IPAddress).GetField("m_Numbers", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField)!;
        s_loopback = new IPv6(1);
    }

    private static bool IsOutOfRange(BigInteger value) => value < 0 || value > MaxValue;

    private static ushort[] ToBits(BigInteger value)
    {
        if (IsOutOfRange(value))
            ExceptionHelper.ThrowArgumentOutOfRangeException(() => value);

        return
        [
            (ushort)(value >> 112),
            (ushort)(value >> 96 & 0xffff),
            (ushort)(value >> 80 & 0xffff),
            (ushort)(value >> 64 & 0xffff),
            (ushort)(value >> 48 & 0xffff),
            (ushort)(value >> 32 & 0xffff),
            (ushort)(value >> 16 & 0xffff),
            (ushort)(value & 0xffff)
        ];
    }

    private static readonly FieldInfo _numbersField;

    private static ushort[] ToBits(string value)
    {
        // use .net built-in type for easier parsing
        IPAddress ip = IPAddress.Parse(value);

        if (ip.AddressFamily != System.Net.Sockets.AddressFamily.InterNetworkV6)
            ExceptionHelper.ThrowArgumentException(() => value, "not a valid IPv6 address");

        return (ushort[])_numbersField.GetValue(ip)!;
    }

    private static string ToString(IEnumerable<ushort> value)
    {
        StringBuilder sb = new(71);

        foreach (ushort b in value)
        {
            if (b != 0)
                sb.Append(b.ToString("h"));

            sb.Append(':');
        }
        sb.Remove(sb.Length - 1, 1);

        return sb.ToString();
    }


    public static new IPv6 Parse(string host) => new(host);

    public static new bool TryParse(string host, out Host? value)
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

    public BigInteger Address => _address;

    public override string Value => _value.Value;

    public ReadOnlyList<ushort> Bits => _bits.Value;

    public override bool IsLoopback => _address == s_loopback.Address;
}
