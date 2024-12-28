using AP.Security.Cryptography;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;

namespace AP.Web.Handlers
{
    public sealed class StylesCombiner : IHttpHandler, IReadOnlySessionState
    {
        //public const string FileExtension = ".css";
        public const string DefaultUrl = "/Styles";
        public const string Salt = "PowPowPow66PewPewPew8822";
        public const string DefaultTemporaryFilesFolder = "/Content/Temp/Styles";

        public bool IsReusable { get { return true; } }

        private string _temporaryFilesFolder;

        public string TemporaryFilesFolder
        {
            get
            {
                string tmp = _temporaryFilesFolder;
                if (string.IsNullOrEmpty(tmp))
                    _temporaryFilesFolder = tmp = DefaultTemporaryFilesFolder;

                return tmp;
            }
            set
            {
                _temporaryFilesFolder = value;
            }
        }

        public void ProcessRequest(System.Web.HttpContext context)
        {            
            HttpResponse response = context.Response;
            response.ContentType = "text/css";            

            HttpSessionState session = context.Session;
            SymmetricCryptoServiceBase service = new DESCryptoService { Salt = Salt };
           
            foreach (string current in service.Decrypt(context.Request.QueryString.ToString()).Split(new [] { '&' }, StringSplitOptions.RemoveEmptyEntries).Distinct().Select(p => HttpUtility.UrlDecode(p)))
            {
                // removed the caching mechanizm - only session wise now - helps debugging
                string s = session[current] as string;

                // removed as some resources don't need an extension
                //if (!current.EndsWith(FileExtension))
                //    throw new HttpException("Extension wrong or missing");
                
                if (s != null)
                {
                    response.Write(s);
                    response.Write(Environment.NewLine);
                }
                else
                {
                    string url = HttpUtility.UrlDecode(current);
                    FileInfo file = new FileInfo(context.Server.MapPath(url));

                    if (file.Exists)
                    {
                        using (StreamReader reader = new StreamReader(file.OpenRead()))
                        {
                            session[current] = s = reader.ReadToEnd();
                            response.Write(s);

                            //response.Write(reader.ReadToEnd());
                            response.Write("\r\n");
                        }
                        
                        // delete the temporary file
                        if (url.StartsWith(this.TemporaryFilesFolder + "/"))
                            File.Delete(file.FullName);
                    }
                }
            }
        }        
    }
}

