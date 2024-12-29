using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;

using AP;
using AP.Web;
using AP.Drawing;

namespace AP.Web.IO
{
    public class VirtualImage : VirtualFile
    {
        public VirtualImage(string virtualPath)
            : base(virtualPath)
        { }

        public static readonly string[] Extensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        public const string ThumbnailFileMarker = ".thumb";

        public static readonly Size DefaultThumbnailSize = new Size(200, 150);
        
        public VirtualFile Thumbnail 
        {
            get
            {
                string extension = System.IO.Path.GetExtension(this.VirtualPath);

                return new VirtualFile(this.VirtualPath.Remove(this.VirtualPath.LastIndexOf(extension)) + ThumbnailFileMarker + extension);
            }        
        }
        public string Title { get; set; }
        public string Description { get; set; }
        
        ///// <summary>
        ///// Custom RouteValues if you need content beyond a fullsize Image
        ///// </summary>
        //public object RouteValues { get; set; }

        public static void CreateThumbnailImage(string virtualFilePath, Size? maximumSize = null)
        {
            HttpContextBase httpContext = Http.GetHttpContext();

            FileInfo fi = new FileInfo(httpContext.Server.MapPath(virtualFilePath));

            if (fi.Exists)
            {
                using (Image img = Image.FromFile(fi.FullName))
                {                    
                    using (Image thumbnail = img.Scale(maximumSize ?? DefaultThumbnailSize))
                    {
                        thumbnail.Save(System.IO.Path.Combine(fi.DirectoryName, GetThumbnailName(fi.Name)));
                    }
                }
            }
        }

        public static string GetThumbnailName(string imageName)
        {
            string extension = System.IO.Path.GetExtension(imageName);
            string s = imageName.Remove(imageName.LastIndexOf(extension));

            return s + VirtualImage.ThumbnailFileMarker + extension;
        }

        public static string GetImageName(string thumbnailName)
        {
            string extension = System.IO.Path.GetExtension(thumbnailName);
            string s = thumbnailName.Remove(thumbnailName.LastIndexOf(ThumbnailFileMarker));

            return s + extension;
        }

        public static bool IsImageFile(string fileName)
        {
            return Extensions.Contains(System.IO.Path.GetExtension(fileName).ToLowerInvariant());
        }

        public static bool IsThumbnailFile(string fileName)
        {
            return Extensions.Contains(System.IO.Path.GetExtension(fileName).ToLowerInvariant()) && System.IO.Path.GetFileNameWithoutExtension(fileName).EndsWith(ThumbnailFileMarker);
        }

        //public static IEnumerable<ImageInfo> GetImages(string virtualDirectoryPath, string title = null, string description = null, object routeValues = null)
        //{
        //    DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath(virtualDirectoryPath));

        //    var q = from f in di.GetFiles()
        //            where f.Name.Count(p => p == '.') == 1 && Extensions.Contains(f.Extension.ToLower())
        //            let imageInfo = new Func<System.IO.FileInfo, ImageInfo>(fi =>
        //            {
        //                return new ImageInfo
        //                {
        //                    ImageUrl = virtualDirectoryPath + fi.Name,
        //                    ThumbnailUrl = virtualDirectoryPath + System.IO.Path.GetFileNameWithoutExtension(fi.Name) + ThumbnailFileMarker + fi.Extension,
        //                    Title = title,
        //                    Description = description,
        //                    RouteValues = routeValues
        //                };
        //            })
        //            select imageInfo.Invoke(f);

        //    return q;
        //}

        public static bool TumbnailExists(string file)
        {
            string tsName = GetThumbnailName(file);

            return new VirtualFile(tsName).Exists;
        }
    }
}