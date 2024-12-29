using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AP.Linq;

namespace AP.UniformIdentifiers;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly"), Serializable]
public class Unc : UrlBase, IFileUri, IAbsoluteOrRelativeUri
{
    public const char Separator = '\\';
    public const char Root = '\\';
    public const string RemoteShort = @"\\";
    public const string RemoteLong = @"\\UNC\";
    public const string RemoteExtraLong = @"\\?\UNC\";
    public const string DriveSeparator = @":\";
    public const string CurrentDirectory = "./";
    public const string ParentDirectory = "..";
    public const char AlternativeSeparator = '/';
    
    private static readonly char[] _reservedCharacters = ['*', ':', '?', '"', '/', '\\', '<', '>', '|', '\0', '\t', '\r', '\n', '\b'];
    private readonly Lazy<Unc?> _parent;

    public static char[] ReservedCharacters => (char[])_reservedCharacters.Clone();

    public Unc(IEnumerable<string> pathSegments, Host? host = null, string? shareName = null, bool? isSecure = null, ushort? port = null)
        : this()
    {
        string[] segments = pathSegments.ToArray();

        // or throw an exception if there's literally no path provided?
        if (segments.IsDefaultOrEmpty())
        {
            this.Path = string.Empty;
            return;
        }
        
        const char separator = Separator;

        StringBuilder sb = new();

        foreach (string s in segments)
        {
            sb.Append(s);
            sb.Append(separator);
        }

        // exclude the first character from splitting - might be a "/" or a "~"
        sb.TrimStart();
        sb.Replace(AlternativeSeparator, separator);
        
        Collections.List<string> parts = new();

        string root = separator.ToString();
        bool isRoot = sb.StartsWith(root);
        bool isRemote = isRoot && sb.StartsWith(RemoteShort);

        // if it's just a root but not a remote then add a part
        if (isRoot && !isRemote)
            parts.Add(root);

        isRoot = !isRemote;

        // if it's remote - extract the shareName
        // if not .... screw it ;D

        char[] reserved = _reservedCharacters;
        char driveLetter = '\0';
        
        string[] cleaned = sb.Split([separator]);

        int i = 0;

        if (cleaned.Length > 1)
        {
            if (cleaned[i].Trim().Equals("?"))
                i++;

            if (cleaned.Length > 2 && cleaned[i].Trim().Equals("UNC", StringComparison.InvariantCultureIgnoreCase))
                i++;
        }

        int start = i;

        // get the first thing
        foreach (string clean in cleaned)
        {
            string segment = clean.Trim();
            bool validateSegment = true;

            if (!segment.IsEmpty())
            {
                if (isRemote)
                {
                    // get the host, but that's only a part of
                    if (i == start)
                    {
                        Host extractedHost = ExtractAuthority(segment, out bool extractedIsSecure, out ushort extractedPort);

                        if (host == null)
                            host = extractedHost;

                        if (!port.HasValue)
                            port = extractedPort;

                        if (!isSecure.HasValue)
                            isSecure = extractedIsSecure;
                    }
                    else if (i == 1 && shareName == null)
                        this.ShareName = shareName = segment;
                }
                else if (isRoot && i == start)
                {
                    if (segment.Length == 2 && segment[1] == ':' && char.IsLetter(segment[0]))
                    {
                        driveLetter = segment[0];
                        validateSegment = false;
                    }
                    parts.Add(segment);
                }
                else
                {
                    validateSegment = true;
                    parts.Add(segment);
                }
                if (validateSegment && segment.IndexOfAny(reserved) > -1)
                    throw new ArgumentException("Contains illegal characters", paramName: nameof(pathSegments));
            }
            i++;
        }

        // now build the path 
        sb = new StringBuilder();

        foreach (string s in parts)
        {
            sb.Append(s);
            sb.Append(separator);
        }

        int sbl = sb.Length - 1;
        if (sb.Length > 0)
            sb.Remove(sbl, 1);

        this.Host = host;
        this.DriveLetter = driveLetter;

        this.Port = port ?? 137;
        this.IsSecure = isSecure ?? false;

        string path = sb.ToString();
        this.Path = path;

        int l = path.LastIndexOf('.');
        if (l > -1)
            this.Extension = path.Substring(l);

        l = path.LastIndexOf(Separator);
        if (l > -1)
            this.Name = path.Substring(l + 1);
    }

    public Unc(string unc, Host? host = null, string? shareName = null, bool? isSecure = null, ushort? port = null)
        : this([unc], host, shareName, isSecure, port)
    {
        this.OriginalString = unc;
    }
    
    private Unc()
    {        
        _parent = new(CreateParent);
    }

    protected override void BuildFullName(ref StringBuilder builder)
    {
        builder.Clear();

        if (this.IsRemote)
        {
            builder.Append(RemoteShort);
            builder.Append(this.Host);

            if (this.IsSecure)
                builder.Append("@SSL");
            if (this.Port != 137)
                builder.Append(this.Port);
        }

        builder.Append(this.Path);
    }

    private static Host ExtractAuthority(string extendedHost, out bool isSecure, out ushort port)
    {
        int lastAtIndex = extendedHost.LastIndexOf('@');

        isSecure = false;
        port = 137;

        if (lastAtIndex > -1)
        {
            bool sslFound = false;
            bool portFound = false;

            string[] split = extendedHost.Split('@');

            if (split.Length > 1)
            {
                for (int i = 1; i < 3; i++)
                {
                    string current = split[^i];

                    if (!sslFound && current.Equals("SSL", StringComparison.InvariantCultureIgnoreCase))
                    {
                        sslFound = isSecure = true;
                    }
                    else if (!portFound && ushort.TryParse(current, out port))
                    {
                        portFound = true;
                    }
                }
            }

            // rebuild the splitpot
            StringBuilder sb = new(extendedHost);

            if (sslFound)
                sb.Remove(sb.LastIndexOf('@'));

            if (portFound)
                sb.Remove(sb.LastIndexOf('@'));

            extendedHost = sb.ToString();
        }

        return Host.Parse(extendedHost);
    }

    public char DriveLetter { get; private set; }

    #region IPossibleFileUri Members

    [IgnoreDataMember]
    public string Extension
    {
        get;
        private set;
    }

    [IgnoreDataMember]
    public string Name
    {
        get;
        private set;
    }

    #endregion

    #region IHierarchicalUri Members

    public string Path { get; private set; }

    #endregion
    
    public ushort Port { get; private set; } = 0;

    public string ShareName { get; private set; } = string.Empty;

    public string LocalPath { get; private set; } = string.Empty;

    public bool IsSecure { get; private set; } = false;

    public override string Scheme => @"\\";

    #region IHierarchicalUri Members

    public Unc? Parent => _parent.Value;

    protected Unc? CreateParent() =>
        // todo:
        // rip that code from httpUrl
        null;

    public bool HasParent => Parent is not null;

    IHierarchicalUri? IHierarchicalUri.Parent => this.Parent;

    #endregion
}
