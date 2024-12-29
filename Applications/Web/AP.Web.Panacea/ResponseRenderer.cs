using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;
using AP.UI;
using System.Net.Mime;

namespace AP.Panacea.Web
{
    public class ResponseRenderer : IResponseRenderer
    {
        #region IResponseRenderer<Response> Members

        void AP.Panacea.IResponseRenderer<Response>.Render(Response response)
        {
            this.Render(response, null);
        }

        #endregion

        #region IResponseRenderer<Response, AP.Web.HttpResponse> Members

        public virtual void Render(Response response, AP.Web.HttpResponse target = null)
        {
            object result = response.Result;

            if (result == null)
                this.RenderText("Error, result was null.");
            else if (result is System.Web.UI.Page)
                this.RenderPage((System.Web.UI.Page)result, target != null ? target.Output : null);
            else if (result is System.Web.UI.Control)
                this.RenderControl((System.Web.UI.Control)result, target != null ? target.Output : null);
            else if (result is PageResult)
                this.RenderPage((PageResult)result, target != null ? target.Output : null);
            else if (result is ControlResult)
                this.RenderControl((ControlResult)result, target != null ? target.Output : null);
            else if (result is Json)
                this.RenderJson((Json)result, target);
            else if (result is Javascript)
                this.RenderJavascript((Javascript)result, target);
            else if (result is Css)
                this.RenderCss((Css)result, target);
            else if (result is FileResult)
                this.RenderFile((FileResult)result, target);
            //else if (result is ForwardToHttpHandler)
            //    this.RenderHttpHandlerResult((ForwardToHttpHandler)result, target);            
            else if (result is string)
                this.RenderText((string)result, target);
            else
                this.RenderObject(result, target);
        }

        //private virtual void RenderHttpHandlerResult(ForwardToHttpHandler forwardToHttpHandler, AP.Web.HttpResponse target)
        //{
            
        //}

        public virtual void RenderObject(object result, AP.Web.HttpResponse target = null)
        {            
            this.RenderText(result.ToString(), target);
        }

        public void RenderText(string text, AP.Web.HttpResponse target = null)
        {
            target = target ?? AP.Web.HttpResponse.Current;

            //target.Clear();
            target.StatusCode = (int)HttpStatusCode.OK;
            target.ContentType = "text/plain";
            target.Write(text);
        }

        public void RenderFile(FileResult file, AP.Web.HttpResponse target = null)
        {
            // todo...
            AP.IO.File f = file.File;

            target = target ?? AP.Web.HttpResponse.Current;
            target.StatusCode = (int)HttpStatusCode.OK;
            
            string contentType = null;
            Encoding headerEncoding = null;
            Encoding contentEncoding = null;

            switch (f.Extension.ToLower())
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

            if (headerEncoding != null)
                target.HeaderEncoding = headerEncoding;

            if (contentEncoding != null)
                target.ContentEncoding = contentEncoding;

            target.ContentType = contentType;

            using (Stream fs = f.Open())
                fs.CopyTo(target.OutputStream);
        }

        public void RenderCss(Css css, AP.Web.HttpResponse target = null)
        {
            target = target ?? AP.Web.HttpResponse.Current;            
            target.StatusCode = (int)HttpStatusCode.OK;
            target.ContentType = Css.ContentType;
            target.Write(css.Value);
        }

        public void RenderJavascript(Javascript javascript, AP.Web.HttpResponse target = null)
        {
            target = target ?? AP.Web.HttpResponse.Current;                     
            target.StatusCode = (int)HttpStatusCode.OK;
            target.ContentType = Javascript.ContentType;
            target.Write(javascript.Value);
        }

        public void RenderJson(Json json, AP.Web.HttpResponse target = null)
        {
            target = target ?? AP.Web.HttpResponse.Current;            
            target.StatusCode = (int)HttpStatusCode.OK;
            target.ContentType = Json.ContentType;
            target.HeaderEncoding = Encoding.ASCII;
            target.ContentEncoding = json.Encoding;
            target.Write(json.Value);
        }

        public virtual void RenderControl(ControlResult control, TextWriter textWriter = null)
        {
            this.RenderControl((System.Web.UI.Control)New.Instance(BuildManager.GetCompiledType(control.Uri.FullName)));
        }

        public virtual void RenderPage(PageResult page, TextWriter textWriter = null)
        {
            this.RenderPage((System.Web.UI.Page)New.Instance(BuildManager.GetCompiledType(page.Uri.FullName)), textWriter);
        }

        public virtual void RenderPage(System.Web.UI.Page page, TextWriter textWriter = null)
        {
            // straight forward - could use the wrapped page.Response.Output instead - but that might be inconsistent with rendering controls
            textWriter = textWriter ?? AP.Web.HttpResponse.Current.Output;
            
            HttpContext ctx2 = new HttpContext(HttpContext.Current.Request, new HttpResponse(textWriter));
            ((IHttpHandler)page).ProcessRequest(ctx2);
        }

        public virtual void RenderControl(System.Web.UI.Control control, TextWriter textWriter = null)
        {
            textWriter = textWriter ?? HttpContext.Current.Response.Output;
            control.RenderControl(new HtmlTextWriter(textWriter));
        }

        #endregion
    }
}