using System;
using System.Collections.Generic;
using System.Text;

namespace AP.UniformIdentifiers;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly"), Serializable]
public sealed class UnixUrl : UriBase, IAbsoluteOrRelativeUri, IHierarchicalUri, IFileUri
{
    public UnixUrl(string path)
        : this([path])
    {
        this.OriginalString = path;
    }

    public UnixUrl(IEnumerable<string> segments)
    {
        StringBuilder sb = new();

        foreach (string s in segments)
        {
            sb.Append(s);
            sb.Append('/');
        }

        if (sb.Length > 0)
            sb.Remove(sb.Length - 1, 1);

        sb.Trim();
        string unclean = sb.ToString();

        // check for illegal characters? nay - it's a posix url - almost anything goes...
        // now split it again and put in some values
        string[] splitted = sb.Split(['/'], StringSplitOptions.RemoveEmptyEntries);
        sb.Clear();

        foreach (string s in splitted)
        {
            sb.Append(s.Trim());
            sb.Append('/');
        }
        if (sb.Length > 0)
            sb.Remove(sb.Length - 1, 1);

        this.Path = unclean.Length > 0 && unclean[0] == '/' ? sb.Insert(0, '/').ToString() : sb.ToString();

        _parent = new(this.CreateParent);
    }

    public override string Scheme => @"/";

    protected override void BuildFullName(ref StringBuilder builder)
    {
        builder.Clear();
        builder.Append(this.Path);
    }

    #region IFileUri Members

    private string _extension;

    public string Extension
    {
        get
        {
            string ex = _extension;
            if (ex == null)
                _extension = ex = System.IO.Path.GetExtension(this.Path);

            return ex;
        }
        private set => _extension = value;
    }

    private string _name;

    public string Name
    {
        get
        {
            string name = _name;
            if (name == null)
                _name = name = System.IO.Path.GetFileName(this.Path);

            return name;
        }
    }
    #endregion

    #region IHierarchicalUri Members

    public string Path 
    { 
        get;
        private init;
    }

    #endregion

    #region IAbsoluteOrRelativeUri Members

    public bool IsAbsolute => this.Path.StartsWith("/") || this.Path.StartsWith("~/");

    #endregion

    #region IHierarchicalUri Members

    private readonly Lazy<UnixUrl?> _parent;

    public UnixUrl? Parent => _parent.Value;

    private UnixUrl? CreateParent()
    {
        if (!this.IsAbsolute)
            return new UnixUrl("../" + this.Path);

        if (this.Path.Equals("/"))
            return null;

        int index = this.Path.LastIndexOf('/');

        if (index > -1)
        {
            if (index == 0)
                return new UnixUrl("/");
            
            if (this.Path.Length > 1)
                return new UnixUrl(this.Path.Remove(index));
        }
        return null;
    }

    IHierarchicalUri? IHierarchicalUri.Parent => this.Parent;

    public bool HasParent => Parent is not null;

    #endregion
}
