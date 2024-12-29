using System;
using System.Text.RegularExpressions;

namespace AP.UniformIdentifiers;

public sealed class NamedHost : Host
{
    private readonly string _value;
    public override string Value => _value;

    private static readonly Regex _hostRegex;
    private static readonly NamedHost _loopback;

    static NamedHost()
    {
        _hostRegex = new Regex(@"^(([a-zA-Z]|[a-zA-Z][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z]|[A-Za-z][A-Za-z0-9\-]*[A-Za-z0-9])$", RegexOptions.Compiled);
        _loopback = new NamedHost("localhost");
    }

    public override Host Loopback => _loopback;

    public static new NamedHost Parse(string host) => new(host);
    public static new bool TryParse(string host, out Host? value)
    {
        if (_hostRegex.IsMatch(host))
        {
            value = new NamedHost(host);
            return true;
        }
        value = null;

        return false;
    }

    public NamedHost(string value)
        : base(value = value.Trim())
    {
        if (!_hostRegex.IsMatch(value))
            throw new ArgumentException($"invalid hostName: {value}", paramName: nameof(value));

        _value = value;
    }

    public override bool IsLoopback => _value.Equals("localhost", StringComparison.InvariantCultureIgnoreCase);
}
