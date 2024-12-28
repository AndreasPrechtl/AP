using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AP.UniformIdentifiers;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly"), Serializable]
public abstract class WebResourceUrlBase : UrlBase, IUriWithUserName, IPasswordProtectableUri, IPortUsingUri, IFileUri, IQueryableUri, IFragmentableUri
{
    protected string ParsedScheme { get; private set; }

    protected WebResourceUrlBase(string url, UrlQuery? query = null, UrlFragments? fragments = null, Host? host = null, ushort? port = null, string? userName = null, string? password = null)
        : this(New.Array(url), query, fragments, host, port, userName, password)
    {
        this.OriginalString = url;
    }

    protected WebResourceUrlBase(IEnumerable<string> path, UrlQuery? query = null, UrlFragments? fragments = null, Host? host = null, ushort? port = null, string? userName = null, string? password = null)
        : this()
    {
        StringBuilder sb = new();
        foreach (string s in path)
        {
            sb.Append(Uri.UnescapeDataString(s.Trim()));
            sb.Append('/');
        }
        if (sb.Length > 1)
            sb.Remove(sb.Length - 1, 1);

        // that trimage should be obsolete as I alsready trim all the parts
        //sb.Trim();

        int endIndexScheme = -1;
        bool hasScheme = false;
        
        // a quick check if a scheme is specified - if not it might be a schemeless but still absolute url
        if (sb.StartsWith("//"))
        {
            hasScheme = true;
            endIndexScheme = 2;
        }
        else
        {
            endIndexScheme = sb.IndexOf("://") + 3;
            hasScheme = endIndexScheme > 3;
        }
      
        // remove the scheme - no longer needed
        if (hasScheme)
        {
            string[] split = sb.Split(endIndexScheme);

            if (split.Length > 1)
                sb = new StringBuilder(split[1]);
            else
                sb.Clear();

            this.ParsedScheme = split[0];
        }


        // now: get messy;[
        int credentialsEnd = sb.IndexOf('@');

        bool requiresHost = hasScheme;
        bool hostExtracted = false;
        
        if (credentialsEnd > -1)
        {
            string[] splitted = sb.Split(credentialsEnd);

            string credentials = splitted[0];
            int passwordStart = credentials.IndexOf(':');

            // see if a password was specified
            if (passwordStart > -1)
            {
                splitted = credentials.Split(passwordStart);
                userName = userName ?? splitted[0];
                password = password ?? splitted[1];
            }
            else
                userName = userName ?? credentials;

            userName = userName != null ? Uri.UnescapeDataString(userName.Trim()) : null;
            password = password != null ? Uri.UnescapeDataString(password.Trim()) : null;
            
            if (userName.IsNullOrEmpty() && !password.IsNullOrEmpty())
                throw new ArgumentException("password cannot be used without a username");
            
            // remove the credentials from the builder
            sb = new StringBuilder(splitted[1].Trim());

            requiresHost = true;
        }

        // initialize a possible port index with -1 just to fill it later on
        int portStart = -1;

        // remove the IPv6 Host - this can be done without checking for requiresHost
        if (sb.Length > 0 && sb[0] == '[')
        {
            string[] splitted = sb.Split(sb.IndexOf(']') + 1);
            host = host ?? new IPv6(splitted[0]);
            
            // remove from the builder
            sb = new StringBuilder(splitted[1].Trim());

            hostExtracted = true;
        }
        
        portStart = sb.IndexOf(':');
        
        if (portStart > -1)
        {
            // if there is a port - there is a path or a query or a fragment or just a means to an end... this is awful.
            
            // first things first - split the builder - extract the host - that way we can recycle that stringbuilder and add digits to a separate port string
            string[] splitted = sb.Split(portStart);
           
            host = host ?? UniformIdentifiers.Host.Parse(splitted[0]);

            string right = splitted[1].Substring(1);

            int i = 1;
            for (; i < right.Length; ++i)
            {
                if (!char.IsDigit(right[i]))
                    break;
            }

            splitted = right.Split(i);

            // last digit index was retrieved - split the string and remove it from the stringbuilder
            port = port ?? ushort.Parse(splitted[0]);
            
            sb = new StringBuilder(right.Substring(i).Trim());

            hostExtracted = true;
        }

        string parsedPath = null;
        int queryStart = -2;
        int fragmentsStart = -2;
        
        // lets see if a host is needed - if one is needed I need to make sure that it stays within certain boundaries
        if (!hostExtracted && !sb.IsNullOrEmpty())
        {
            if (requiresHost)
            {
                int splitIndex = sb.IndexOf('/');

                if (splitIndex == -1)
                    splitIndex = queryStart = sb.IndexOf('?');

                if (splitIndex == -1)
                    splitIndex = fragmentsStart = sb.IndexOf('#');

                if (splitIndex > -1)
                {
                    string[] split = sb.Split(splitIndex);
                    host = host ?? UniformIdentifiers.Host.Parse(split[0]);

                    sb = new StringBuilder(split[1].Trim());
                }
                else
                {
                    host = host ?? UniformIdentifiers.Host.Parse(sb.ToString());
                    sb.Clear();
                }
            }
        
        //    else if (!sb.IsNullOrEmpty())
        //    {   
        //        // see if it can be a host
        //        int splitIndex = sb.IndexOf('/');

        //        if (splitIndex == -1)
        //            splitIndex = queryStart = sb.IndexOf('?');

        //        if (splitIndex == -1)
        //            splitIndex = fragmentsStart = sb.IndexOf('#');

        //        string left = null;
        //        string right = null;

        //        if (splitIndex > -1)
        //        {
        //            string[] splitted = sb.Split(splitIndex);
        //            left = splitted[0];
        //            right = splitted[1];
        //        }
        //        else
        //        {
        //            left = sb.ToString();
        //        }

        //        try
        //        {                    
        //            // if it was successful - remove the host from the builder
        //            sb = right != null ? new StringBuilder(right.Trim()) : null;
        //        }
        //        catch (Exception)
        //                { }
        //            }
                
        //    }
        }
        
        // after that it should be a piece of cake pulling out the next segments
        // see if the path is up next
        if (!sb.IsNullOrEmpty() && (sb[0] != '?' && sb[0] != '#'))
        {
            int splitIndex = queryStart == -2 ? sb.IndexOf('?') : queryStart;

            if (splitIndex == -1)
                splitIndex = fragmentsStart = sb.IndexOf('#');

            // again - clear out the left part - however if the splitting result just contains one value - clear out the builder - we are finished 
            // note: checking for an index above 1 ... or is it possible to have urls like ?# ...
            if (splitIndex > 0)
            {
                string[] splitted = sb.Split(splitIndex);
                parsedPath = splitted[0];
                sb = new StringBuilder(splitted[1].Trim());
            }
            else
            {
                parsedPath = sb.ToString();
                sb = null;
            }

            // clean the parsedPath
            StringBuilder pathBuilder = new(parsedPath.Length);

            char separator = '/';
            string[] cleanup = parsedPath.Split([separator], StringSplitOptions.RemoveEmptyEntries);

            if (parsedPath.Length > 0 && parsedPath[0] == separator)
                pathBuilder.Append(separator);

            foreach (string s in cleanup)
            {
                pathBuilder.Append(Uri.UnescapeDataString(s.Trim()));
                pathBuilder.Append(separator);
            }
            if (pathBuilder.Length > 1)
                pathBuilder.Remove(pathBuilder.Length - 1, 1);

            parsedPath = pathBuilder.ToString();
        }
        
        // extract the query
        if (!sb.IsNullOrEmpty() && sb[0] == '?')
        {
            if (fragmentsStart == -2)
                fragmentsStart = sb.IndexOf('#');

            if (fragmentsStart > 0)
            {
                string[] splitted = sb.Split(fragmentsStart);
                query = query ?? new UrlQuery(splitted[0].Substring(1));
                sb = new StringBuilder(splitted[1].Trim());
            }
            else
            {
                query = query ?? new UrlQuery(sb.Substring(1).ToString());
                sb = null;
            }
        }

        // at long last - extract the fragments
        if (!sb.IsNullOrEmpty() && sb[0] == '#')
            fragments = fragments ?? new UrlFragments(sb.Substring(1).ToString());
        
        this.Path = parsedPath ?? string.Empty;
        this.Host = host;
        this.Port = port;
        this.Query = query ?? UrlQuery.Empty;
        this.Fragments = fragments ?? UrlFragments.Empty;
        this.Password = password;
        this.UserName = userName;

        //this.Name = System.IO.Path.GetFileName(pathName);
        //this.Extension = System.IO.Path.GetExtension(pathName);
    }

    /// <summary>
    /// Initializes a new WebResourceUrl with empty properties
    /// </summary>
    protected WebResourceUrlBase()
        : base()
    {
        _parent = new Deferrable<WebResourceUrlBase>(CreateParent);
        this.Query = UrlQuery.Empty;
        this.Fragments = UrlFragments.Empty;
    }
    
    protected override void BuildFullName(ref StringBuilder builder)
    {
        //   base.BuildFullName(ref builder);
        builder.Clear();

        if (this.IsAbsolute)
        {
            builder.Append(this.Scheme);
            builder.Append("://");

            int userNamePasswordEndIndex = -1;

            string userName = this.UserName;
            if (userName != null)
            {
                userName = Uri.EscapeDataString(userName);
                int index = builder.IndexOf("://") + 3;

                userNamePasswordEndIndex = index + userName.Length;
                builder.Insert(index, userName);
                builder.Insert(userNamePasswordEndIndex, '@');
            }

            // insert a password if necessary
            string password = this.Password;
            if (password != null)
            {
                int insert = userNamePasswordEndIndex;

                builder.Insert(insert, ':');
                builder.Insert(++insert, Uri.EscapeDataString(password));
            }
            builder.Append(Host.Value);
        }

        if (!this.Path.IsNullOrEmpty())
            builder.Append(Uri.EscapeDataString(this.Path));

        if (this.Query != UrlQuery.Empty)
        {
            builder.Append('?');
            builder.Append(this.Query.Value);
        }

        if (this.Fragments != UrlFragments.Empty)
        {
            builder.Append('#');
            builder.Append(this.Fragments.Value);
        }
    }

    #region IRemotableUri Members

    public ushort? Port
    {
        get;
        protected set;
    }

    public virtual ushort DefaultPort => 80;

    #endregion

    #region IUriWithUserName Members

    public string UserName
    {
        get;
        protected set;
    }

    #endregion

    #region IHierarchicalUri Members

    public string Path { get; protected set; }
    
    #endregion

    #region IPossibleFileUri Members

    private string _extension;

    [IgnoreDataMember]
    public virtual string Extension
    {
        get
        {
            string ex = _extension;
            if (ex == null)
                _extension = ex = System.IO.Path.GetExtension(this.Path);

            return ex;
        }
        protected set => _extension = value;
    }

    private string _name;

    [IgnoreDataMember]
    public virtual string Name
    {
        get
        {
            string name = _name;
            if (name == null)
                _name = name = System.IO.Path.GetFileName(this.Path);

            return name;
        }
        protected init => _name = value;
    }

    #endregion

    #region IHierarchicalUri Members

    protected readonly Deferrable<WebResourceUrlBase> _parent;

    IHierarchicalUri IHierarchicalUri.Parent => _parent.Value;

    public WebResourceUrlBase Parent => _parent.Value;

    public bool HasParent => _parent.IsValueActive;

    /// <summary>
    /// Creates an empty instance used for simplified parent creation
    /// </summary>
    /// <returns></returns>
    protected abstract WebResourceUrlBase CreateEmptyInstance();

    protected virtual WebResourceUrlBase? CreateParent()
    {
        // that thing should always be absolute - shouldn't it?

        WebResourceUrlBase? url = null;

        string? newPath = null;
        string path = this.Path;

        if (!this.IsAbsolute)
            newPath = "../" + path;
        
        if (newPath == null && path.Equals("/"))
            return null;
        
        int index = path.LastIndexOf('/');

        if (index > -1)
        {
            if (index == 0)
                newPath = "/";
            else if (path.Length > 1)
                newPath = path.Remove(index);
        }
        
        if (newPath != null)
        {
            url = this.CreateEmptyInstance();
            url.Path = newPath;
            url.UserName = this.UserName;
            url.Password = this.Password;
            url.Host = this.Host;
            url.Port = this.Port;
            url.Query = this.Query;
            url.Fragments = this.Fragments;
        }

        return url;
    }
    
    #endregion
    
    #region IQueryableUri Members

    public UrlQuery Query
    {
        get;
        protected set;
    }

    #endregion

    #region IFragmentableUri Members

    public UrlFragments Fragments
    {
        get;
        protected set;
    }

    #endregion

    #region IPasswordProtectableUri Members

    public string Password
    {
        get;
        protected set;
    }

    #endregion
}