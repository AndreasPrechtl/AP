using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

using AP;
using AP.Web;


namespace AP.Web.IO
{
    public class VirtualFile
    {
        public VirtualFile(string virtualPath)
        {
            this.VirtualPath = virtualPath;
        }

        public string Name
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(VirtualPath);
            }
        }
        public string Extension
        {
            get
            {
                return System.IO.Path.GetExtension(VirtualPath);
            }
        }

        public string VirtualPath { get; private set; }

        public VirtualDirectory ParentDirectory
        {
            get 
            {                
                if (Exists)
                    return new VirtualDirectory(this.VirtualPath.Remove(VirtualPath.LastIndexOf(Name)));

                return null;
            }
        }

        public bool Exists
        {
            get
            {
                return File.Exists(Http.GetHttpContext().Server.MapPath(VirtualPath));
            }
        }
    }
}