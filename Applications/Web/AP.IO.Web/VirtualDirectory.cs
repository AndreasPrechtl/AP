using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using AP;
using AP.Web;

namespace AP.Web.IO
{
    public class VirtualDirectory
    {
        public VirtualDirectory(string virtualPath)
        {
            this.VirtualPath = virtualPath;
        }

        public string Name
        {
            get
            {
                return System.IO.Path.GetDirectoryName(VirtualPath);
            }
        }

        public string VirtualPath { get; private set; }

        public VirtualDirectory Directory
        {
            get
            {
                if (Exists)
                    return new VirtualDirectory(VirtualPath.Remove(VirtualPath.LastIndexOf(Name)));
                
                return null;
            }
        }

        public IEnumerable<VirtualFile> Files
        {
            get
            {
                string physicalPath = HttpContext.Current.Server.MapPath(VirtualPath);
                DirectoryInfo dir = new DirectoryInfo(physicalPath);

                if (dir.Exists)
                {
                    foreach (FileInfo file in dir.GetFiles())
                    {
                        yield return new VirtualFile(System.IO.Path.Combine(this.VirtualPath, file.Name));
                    }
                }
            }
        }

        public IEnumerable<VirtualDirectory> Directories
        {
            get
            {
                string physicalPath = Http.GetHttpContext().Server.MapPath(VirtualPath);
                DirectoryInfo dir = new DirectoryInfo(physicalPath);

                if (dir.Exists)
                {
                    foreach (DirectoryInfo directory in dir.GetDirectories())
                    {
                        yield return new VirtualDirectory(System.IO.Path.Combine(this.VirtualPath, directory.Name));
                    }
                }
            }
        }

        public bool Exists
        {
            get
            {
                return System.IO.Directory.Exists(Http.GetHttpContext().Server.MapPath(VirtualPath));
            }
        }
    }
}