using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AP.UniformIdentifiers
{
    public sealed class NamedHost : Host
    {
        private readonly string _value;
        public override string Value { get { return _value; } }

        private static readonly Regex _hostRegex;
        private static readonly NamedHost _loopback;

        static NamedHost()
        {
            _hostRegex = new Regex(@"^(([a-zA-Z]|[a-zA-Z][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z]|[A-Za-z][A-Za-z0-9\-]*[A-Za-z0-9])$", RegexOptions.Compiled);
            _loopback = new NamedHost("localhost");
        }

        public override Host Loopback
        {
            get { return _loopback; }
        }

        public static new NamedHost Parse(string host)
        {
            return new NamedHost(host);
        }
        public static new bool TryParse(string host, out Host value)
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
                ExceptionHelper.ThrowArgumentException(() => value, "invalid hostName");

            _value = value;
        }

        public override bool IsLoopback
        {
            get { return _value.Equals("localhost", StringComparison.InvariantCultureIgnoreCase); }
        }
    }
}
