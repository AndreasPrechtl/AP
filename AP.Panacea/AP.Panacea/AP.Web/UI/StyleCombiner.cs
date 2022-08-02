using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using AP.Security.Cryptography;
using AP.Web.Handlers;
using System.Web.SessionState;

namespace AP.Web.UI
{
    [ToolboxData("<{0}:StyleCombiner runat=\"server\"></{0}:StyleCombiner>")]
    public sealed class StyleCombiner : Control
    {
        private string _handlerUrl;
        //public const string FileExtension = StylesCombiner.FileExtension;
        public const string DefaultHandlerUrl = StylesCombiner.DefaultUrl;
        public const string DefaultTemporaryFilesFolder = StylesCombiner.DefaultTemporaryFilesFolder;
        
        protected override void OnPreRender(EventArgs e)
        {
            //if (!(this.TemplateControl is MasterPage))
            //    throw new HttpException("StyleCombiners are only allowed in MasterPages");

            base.OnPreRender(e);
        }

        protected override void AddParsedSubObject(object obj)
        {
            if ((obj is StyleReference || obj is InlineStyle) || obj is StylePlaceHolder)
                base.AddParsedSubObject(obj);
            else if (!(obj is LiteralControl))
                throw new HttpException("Only StyleReferences, InlineStyles or StylePlaceHolder (in MasterPages) are allowed");            
        }

        private void RenderChildren(StringBuilder styles, ControlCollection controls, HttpSessionState session) //Cache cache)
        {
            //string hash = this.Page.Request.Path.GetHashCode().ToString();

            for (int i = 0; i < controls.Count; i++)
            {
                System.Web.UI.Control control = controls[i];
                if (control is StyleReference)
                {
                    styles.Append(HttpUtility.UrlEncode(this.ResolveUrl(((StyleReference)control).Source)));
                    styles.Append("&");
                }
                else if (control is InlineStyle)
                {
                    string tempFolder = this.TemporaryFilesFolder;

                    StringBuilder sbKey = new StringBuilder(this.ResolveUrl(tempFolder + "/" + Guid.NewGuid().ToString()));
                    sbKey.Append(".js");

                    StringBuilder sbControl = new StringBuilder();
                    using (HtmlTextWriter writer = new HtmlTextWriter(new StringWriter(sbControl)))
                    {
                        control.RenderControl(writer);
                    }

                    string key = sbKey.ToString();
                    string rendered = sbControl.ToString();

                    session[key] = rendered;

                    // write a temporary file - only valid until the page completed loading
                    string mappedTempFolder = this.Page.Server.MapPath(tempFolder);

                    if (!Directory.Exists(mappedTempFolder))
                        Directory.CreateDirectory(mappedTempFolder);

                    using (StreamWriter sw = File.CreateText(this.Page.Server.MapPath(key)))
                        sw.Write(rendered);

                    styles.Append(key);
                    styles.Append("&");
                }
                else if (control is StylePlaceHolder)
                    this.RenderChildren(styles, control.Controls, session);
            }
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            StringBuilder styles = new StringBuilder();
            var session = this.Context.Session;
            this.RenderChildren(styles, this.Controls, session);
            string plainText = styles.ToString();
            
            if (plainText.EndsWith("&"))
                plainText = plainText.Substring(0, plainText.Length - 1);
            
            plainText = new DESCryptoService { Salt = StylesCombiner.Salt }.Encrypt(plainText);
            string url = this.ResolveUrl(DefaultHandlerUrl + "?" + plainText);

            writer.Write("<link rel=\"stylesheet\" type=\"text/css\" href=\"");
            writer.Write(url);

            if (!string.IsNullOrWhiteSpace(this.ID))
            {
                writer.Write("\" id=\"");
                writer.Write(this.ID);
            }
            writer.Write("\"/>");
        }

        public string HandlerUrl
        {
            get
            {
                string url = _handlerUrl;
                if (string.IsNullOrEmpty(url))
                    _handlerUrl = url = DefaultHandlerUrl;
                
                return url;
            }
            set
            {
                _handlerUrl = value;
            }
        }

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
    }
}

