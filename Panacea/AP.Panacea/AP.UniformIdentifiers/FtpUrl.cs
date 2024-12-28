﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.UniformIdentifiers
{
    /// <summary>
    /// Ftp Url. (e.G. ftp://my.server:21/dir1/dir12/)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable"), Serializable]
    public class FtpUrl : WebResourceUrlBase
    {
        //public enum SpecialSeparators 
        //{
        //    Path = '/',
        //    Query = '?',
        //    Fragment = '#',
        //    IPv6Begin = '[',
        //    IPv6End = ']'
        //}

        public FtpUrl(IEnumerable<string> path, Host host = null, ushort? port = 21, string userName = null, string password = null, UrlQuery query = null, UrlFragments fragments = null)
            : base(path, query, fragments, host, port, userName, password)
        { }

        public FtpUrl(string ftpUrl, Host host = null, ushort? port = 21, string userName = null, string password = null, UrlQuery query = null, UrlFragments fragments = null)
            : base(ftpUrl, query, fragments, host, port, userName, password)
        { }
        
        /// <summary>
        /// Parameterless constructor for empty properties / copying
        /// </summary>
        protected FtpUrl()
            : base()
        { }

        public override ushort DefaultPort
        {
            get
            {
                return 21;
            }
        }

        protected override WebResourceUrlBase CreateEmptyInstance()
        {
            return new FtpUrl();
        }

        public override string Scheme
        {
            get { return System.Uri.UriSchemeFtp; }
        }
        
        public new FtpUrl Parent
        {
            get { return (FtpUrl)base.Parent; }
        }
    }
}
