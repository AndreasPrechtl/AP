using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Collections;
using AP.UI;
using AP.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Reflection;

namespace AP.Web.UI
{
    public class Page : System.Web.UI.Page, IPage
    {
        public Page()
        {
            this.MergeSettings = AP.UI.MergeSettings.All;
        }

        public new string MetaKeywords
        {
            get { return this.MetaData.Tags.ToString(", "); }
            set
            {
                base.MetaKeywords = value;
                this.MetaData.Tags.Clear(); 
                
                if (!value.IsNullOrWhiteSpace())
                    this.MetaData.Tags.UnionWith(new StringSet(value, ",", StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()));
            }
        }

        public new string MetaDescription
        {
            get
            {
                return this.MetaData.Description;
            }
            set
            {
                base.MetaDescription = value;
                this.MetaData.Description = value;
            }
        }

        public new string Title
        {
            get
            {
                return this.MetaData.Title;
            }
            set
            {
                base.Title = value;
                this.MetaData.Title = value;
            }
        }

        public RobotSettings Robots { get; set; }
        public MergeSettings MergeSettings { get; set; }

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

        protected override void OnInit(EventArgs e)
        {
            this.InitHelpers();
            base.OnInit(e);
            this.DataBind();
            PageHelper.EnsureHeaderExistsOnInit(this, e);

            this.OnMerge(e);
        }

        protected virtual void OnMerge(EventArgs e)
        {
            PageMerger.Merge(this);
        }

        public IHtmlHelper Html { get; protected set; }

        protected virtual void InitHelpers()
        {
            this.Html = HtmlHelper.Instance;
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
