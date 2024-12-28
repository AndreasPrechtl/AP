using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Collections;
using AP.UI;

namespace AP.Web.UI
{
    public class MasterPage : System.Web.UI.MasterPage, AP.Web.UI.IPage
    { 
        public MasterPage()
        {
            this.MergeSettings = AP.UI.MergeSettings.All;
        }

        public RobotSettings Robots { get; set; }        
        
        private PageMetaData _metaData;
        public PageMetaData MetaData
        {
            get
            {
                PageMetaData metaData = _metaData;

                if (metaData == null)
                    _metaData = metaData = new PageMetaData();

                return metaData;
            }
            set
            {
                _metaData = value;
            }
        }

        public MergeSettings MergeSettings { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.InitHelpers();
            base.OnInit(e);
            this.DataBind();
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            PageMerger.Merge(this);
        }

        public IHtmlHelper Html { get; protected set; }

        protected virtual void InitHelpers()
        {
            this.Html = HtmlHelper.Instance;
        }
        
        private HttpContext _context;

        public new HttpContext Context
        {
            get
            {
                var context = _context;

                if (context == null)
                    _context = context = new HttpContext(base.Context);

                return context;
            }
        }

        public new HttpServerUtility Server
        {
            get { return this.Context.Server; }
        }

        public new HttpApplicationState Application
        {
            get { return this.Context.Application; }
        }

        public new Cache Cache
        {
            get { return this.Context.Cache; }
        }

        public new HttpSessionState Session
        {
            get { return this.Context.Session; }
        }

        public new HttpRequest Request
        {
            get { return this.Context.Request; }
        }

        public new HttpResponse Response
        {
            get { return this.Context.Response; }
        }
    }
}
