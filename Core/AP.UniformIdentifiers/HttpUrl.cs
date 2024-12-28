using System;
using System.Collections.Generic;

namespace AP.UniformIdentifiers;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly"), Serializable]
public class HttpUrl : WebResourceUrlBase, ISecurableUri
{
    //private char[] _charsToEncode = new char[] { '!', '*', '\'', '(', ')', ';', ':', '@', '&', '=', '+', '$', ',', '/', '?', '%', '#', '[', ']' };

    //public Regex _rex = new Regex(
    //    @"((https?|ftp)\:\/\/)?" // SCHEME
    //    + @"([a-z0-9+!*(),;?&=\$_.-]+(\:[a-z0-9+!*(),;?&=\$_.-]+)?@)?" // User and Pass
    //    + @"([a-z0-9-.]*)\.([a-z]{2,3})" // Host or IP (that is lacking ipv6)
    //    + @"(\:[0-9]{2,5})?" // Port
    //    + @"(\/([a-z0-9+\$_-]\.?)+)*\/?" // Path
    //    + @"(\?[a-z+&\$_.-][a-z0-9;:@&%=+\/\$_.-]*)?" // GET Query
    //    + @"(#[a-z_.-][a-z0-9+\$_.-]*)?" // Anchor
    //    , RegexOptions.Compiled);

    //public MatchCollection Matches
    //{
    //    get { return _rex.Matches(this.FullName); }
    //}
 
    public HttpUrl(IEnumerable<string> path)
        : this(path, null, null, null, null, null, null, null)
    { }

    public HttpUrl(IEnumerable<string> path, UrlQuery? query = null, UrlFragments? fragments = null, Host? host = null, bool? isSecure = null, ushort? port = null, string? userName = null, string? password = null)
        : base(path, query, fragments, host, port, userName, password)
    {
        IsSecure = false;
    }

    public HttpUrl(string url)
        : this(url, null, null, null, null, null, null, null)
    { }

    public HttpUrl(string url, UrlQuery? query = null, UrlFragments? fragments = null, Host? host = null, bool? isSecure = null, ushort? port = null, string? userName = null, string? password = null)
        : this(New.Array(url), query, fragments, host, isSecure, port, userName, password)
    {
        this.OriginalString = url;
    }

    /// <summary>
    /// Initializes a new HttpUrl with empty properties
    /// </summary>
    protected HttpUrl()
        : base()
    {
        IsSecure = false;
        this.Query = UrlQuery.Empty;
        this.Fragments = UrlFragments.Empty;
    }

    public static implicit operator string(HttpUrl url)
    {
        return url.FullName;
    }
    public static implicit operator HttpUrl(string url)
    {
        return new HttpUrl(url);
    }

    public override string Scheme => System.Uri.UriSchemeHttp;

    public bool IsSecure { get; protected set; }

    public override ushort DefaultPort
    {
        get
        {
            if (this.IsSecure)
                return 443;
            
            return 80;
        }
    }

    protected override WebResourceUrlBase CreateEmptyInstance() => new HttpUrl();

    public new HttpUrl Parent => (HttpUrl)base.Parent;
}
