using System.Net.Mime;
using AP.IO;
using System.Text;
using AP.IO.MetaData;

namespace AP.Panacea.Web
{
    public sealed class FileResult
    {
        public File File { get; private set; }
        public string ContentType { get; private set; }
        public Encoding ContentEncoding { get; private set; }
        public Encoding HeaderEncoding { get; private set; }

        public static string GetContentType(File file)
        {
            return Create(file).ContentType;
        }

        public static Encoding GetHeaderEncoding(File file)
        {
            return Create(file).HeaderEncoding;
        }

        public static Encoding GetContentEncoding(File file)
        {
            return Create(file).ContentEncoding;
        }

        public static FileResult Create(File file)
        {
            return new FileResult(file);
        }

        private static void FillBlanks(FileResult result)
        {
            File file = result.File;
            string contentType = null;
            Encoding headerEncoding = null;
            Encoding contentEncoding = null;

            switch (file.Extension.ToLower())
            {
                case ".js":
                    contentType = Javascript.ContentType;
                    break;
                case ".css":
                    contentType = Css.ContentType;
                    break;
                case ".json":
                    contentType = Json.ContentType;
                    headerEncoding = Encoding.ASCII;
                    contentEncoding = Json.DefaultEncoding;
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".jpg":
                case ".jpeg":
                    contentType = System.Net.Mime.MediaTypeNames.Image.Jpeg;
                    break;
                case ".gif":
                    contentType = System.Net.Mime.MediaTypeNames.Image.Gif;
                    break;
                case ".tif":
                case ".tiff":
                    contentType = System.Net.Mime.MediaTypeNames.Image.Tiff;
                    break;
                default:
                    contentType = System.Net.Mime.MediaTypeNames.Application.Octet;
                    break;
            }

            result.ContentType = contentType;
            result.HeaderEncoding = headerEncoding ?? Encoding.ASCII;
            result.ContentEncoding = contentEncoding ?? Encoding.GetEncoding(1251);
        }

        public FileResult(File file, string contentType = null, Encoding headerEncoding = null, Encoding contentEncoding = null)
        {
            this.File = file;

            this.ContentType = contentType;
            this.HeaderEncoding = headerEncoding;
            this.ContentEncoding = contentEncoding;

            FillBlanks(this);
        }
    }
}