using System;
using System.Linq;
using System.Text;
using AP.Collections;

namespace AP.UniformIdentifiers;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly"), Serializable]
public class MailUrl : UrlBase, IUriWithUserName
{
    public MailUrl(string userName, Host host, string? subject = null, string? body = null)
    {
        this.Host = host;
        this.UserName = userName;
        this.Subject = subject;
        this.Body = body;
    }

    public MailUrl(string mailto)
    {
        this.OriginalString = mailto;

        StringBuilder cleaned = new(mailto);
        cleaned.Trim();

        if (cleaned.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase))
            cleaned.Remove(0, 7);

        string[] split = cleaned.Split(['@']);

        if (split.Length != 2)
            throw new ArgumentException("Invalid mailto format");

        this.UserName = split[0].Trim();

        string hostExtended = split[1];
        string host = hostExtended;

        int queryIndex = hostExtended.IndexOf('?');
        
        if (queryIndex > -1)
        {
            host = Uri.UnescapeDataString(hostExtended.Substring(0, queryIndex));
            string query = hostExtended.Substring(queryIndex + 1);
        
            UrlQuery q = new(query);

            // retrieve the subject parameter(s)
            if (q.Contains("subject", out ISetView<string> subject))
                this.Subject = Uri.UnescapeDataString(subject.First());

            // retrieve the body parameter(s)
            if (q.Contains("body", out ISetView<string> body))
                this.Body = Uri.UnescapeDataString(body.First());
        }
        this.Host = Host.Parse(host);
    }

    protected override void BuildFullName(ref StringBuilder builder)
    {
        builder.Clear();
        builder.Append("mailto:");
        builder.Append(this.UserName);
        builder.Append('@');
        builder.Append(this.Host.Value);
        
        string subject = this.Subject;
        bool hasSubject = subject != null;

        if (this.Subject != null)
        {
            builder.Append("?subject=");
            builder.Append(Uri.EscapeDataString(this.Subject));
        }

        if (this.Body != null)
        {
            builder.Append(hasSubject ? '&' : '?');
            builder.Append("body=");
            builder.Append(Uri.EscapeDataString(this.Body));
        }
    }

    public override string Scheme => System.Uri.UriSchemeMailto;

    public string? UserName { get; protected init; }

    public string? Body { get; protected init; }

    public string? Subject { get; protected init; }
}
