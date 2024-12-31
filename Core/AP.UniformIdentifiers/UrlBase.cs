using System;
using System.Text;

namespace AP.UniformIdentifiers;

// validation regex:
//          \b(((\S+)?)(@|mailto\:|(news|(ht|f)tp(s?))\://)\S+)\b 
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly"), Serializable]
public abstract class UrlBase : UriBase, IRemoteUri
{
    #region IRemotableUri Members

    public Host Host { get; protected set; } = null!;

    protected override void BuildFullName(ref StringBuilder builder)
    {            
        // only needed when it's absolute - should I make a distinction?
        if (this.IsAbsolute)
        {
            builder.Append(this.Scheme);
            builder.Append("://");
            builder.Append(this.Host.Value);
        }
    }

    // no longer needed - the original string should be cleaned within the ctor
    //protected override string CleanOriginalString(string original)
    //{
    //    StringBuilder cleaned = new StringBuilder(original);
    //    cleaned.Trim();

    //    string schemeAndDelemiter = this.Scheme + "://";

    //    bool hasScheme = cleaned.StartsWith(schemeAndDelemiter);

    //    if (hasScheme)
    //        cleaned.Remove(0, schemeAndDelemiter.Length);

    //    cleaned.Replace("//", "/");

    //    if (hasScheme)
    //        cleaned.Insert(0, schemeAndDelemiter);

    //    return cleaned.ToString();
    //}

    //protected override void FillProperties(string cleanedOriginal)
    //{
    //    base.FillProperties(cleanedOriginal);

    //    // if it's absolute - extract the host 
    //    // - note: this will not work like that for every url (e.G. mailto, UNC), but for ftp, http(s), news, etc.
    //    StringBuilder hostBuilder = new StringBuilder(cleanedOriginal);

    //    string schemeAndDelemiter = this.Scheme + "://";
    //    if (hostBuilder.StartsWith(schemeAndDelemiter))
    //    {
    //        // remove scheme and delimiter
    //        hostBuilder.Remove(0, schemeAndDelemiter.Length);

    //        // extract the host                                
    //        int hostEndIndex = hostBuilder.IndexOf('/');

    //        if (hostEndIndex > 0)
    //            hostBuilder.Substring(0, hostEndIndex + 1);

    //        this.Host = Host.FromString(hostBuilder.ToString());
    //    }
    //}

    public virtual bool IsRemote => this.Host != null;

    #endregion

    #region IAbsoluteOrRelativeUri Members

    public bool IsAbsolute => this.Host != null;

    #endregion
}
