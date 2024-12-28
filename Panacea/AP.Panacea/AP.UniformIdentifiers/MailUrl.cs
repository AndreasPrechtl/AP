using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Collections;

namespace AP.UniformIdentifiers
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly"), Serializable]
    public class MailUrl : UrlBase, IUriWithUserName
    {
        public MailUrl(string userName, Host host, string subject = null, string body = null)
        {
            this.UserName = userName;
            this.Host = host;
            this.Subject = subject;
            this.Body = body;
        }

        public MailUrl(string mailto)
        {
            this.OriginalString = mailto;

            StringBuilder cleaned = new StringBuilder(mailto);
            cleaned.Trim();

            if (cleaned.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase))
                cleaned.Remove(0, 7);

            string[] split = cleaned.Split(New.Array('@')).ToArray();

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
            
                UrlQuery q = new UrlQuery(query);

                // retrive the subject parameter(s)
                ISetView<string> subject = null;
                if (q.Contains("subject", out subject))
                    this.Subject = Uri.UnescapeDataString(subject.First());
                
                // retrieve the body parameter(s)
                ISetView<string> body = null;
                if (q.Contains("body", out body))
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

        public override string Scheme
        {
            get { return System.Uri.UriSchemeMailto; }
        }

        public string UserName { get; protected set; }

        public string Body { get; protected set; }

        public string Subject { get; protected set; }
    }
}
