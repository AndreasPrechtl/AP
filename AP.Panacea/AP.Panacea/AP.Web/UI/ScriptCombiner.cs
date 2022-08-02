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
    [ToolboxData("<{0}:ScriptCombiner runat=\"server\"></{0}:ScriptCombiner>")]
    public sealed class ScriptCombiner : Control
    {
        private string _handlerUrl;

        public const string DefaultHandlerUrl = ScriptsCombiner.DefaultUrl;
        public const string DefaultTemporaryFilesFolder = ScriptsCombiner.DefaultTemporaryFilesFolder;
        
        protected override void OnPreRender(EventArgs e)
        {
            //if (!(this.TemplateControl is MasterPage))
            //    throw new HttpException("ScriptCombiners are only allowed in MasterPages");

            base.OnPreRender(e);
        }

        protected override void AddParsedSubObject(object obj)
        {
            if ((obj is ScriptReference || obj is InlineScript) || obj is ScriptPlaceHolder)
                base.AddParsedSubObject(obj);
            else if (!(obj is LiteralControl))
                throw new HttpException("only ScriptReferences, InlineScripts or ScriptPlaceHolder (in MasterPages) are allowed");            
        }

        private void RenderChildren(StringBuilder scripts, ControlCollection controls, HttpSessionState session) //, Cache cache)
        {
           // string hash = this.Page.Request.Path.GetHashCode().ToString();

            for (int i = 0; i < controls.Count; i++)
            {
                System.Web.UI.Control control = controls[i];
                if (control is ScriptReference)
                {
                    scripts.Append(HttpUtility.UrlEncode(this.ResolveUrl(((ScriptReference)control).Source)));
                    scripts.Append("&");
                }
                else if (control is InlineScript)
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
                    
                    scripts.Append(key);
                    scripts.Append("&");
                }
                else if (control is ScriptPlaceHolder)
                    this.RenderChildren(scripts, control.Controls, session);
            }
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            StringBuilder scripts = new StringBuilder();
            HttpSessionState session = this.Context.Session;
            // Cache cache = this.Context.Cache;
            this.RenderChildren(scripts, this.Controls, session);
            string plainText = scripts.ToString();

            if (plainText.EndsWith("&"))
                plainText = plainText.Substring(0, plainText.Length - 1);
            
            plainText = new DESCryptoService { Salt = Handlers.ScriptsCombiner.Salt }.Encrypt(plainText);
            string url = this.ResolveUrl(DefaultHandlerUrl + "?" + plainText);

            writer.Write("<script type=\"text/javascript\" src=\"");
            writer.Write(url);

            if (string.IsNullOrWhiteSpace(this.ID))
            {
                writer.Write("\" id=\"");
                writer.Write(this.ID);                
            }
            writer.Write("\"></script>");
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

