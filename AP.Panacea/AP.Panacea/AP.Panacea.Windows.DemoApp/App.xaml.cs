using AP.Collections.Specialized;
using AP.ComponentModel.ObjectManagement;
using AP.Routing;
using AP.UI;
using AP.UI.SiteMapping;
using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AP.Panacea.Windows.DemoApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : AP.Panacea.Windows.ApplicationBase
    {
        public App()
            : base()
        { }

        protected override ContentControl LoadRenderingTarget()
        {
            if (this.MainWindow != null && this.MainWindow.Content != null)
                return ((MasterPage)this.MainWindow.Content).MainContent;

            return null;
        }

        protected override ApplicationCore CreateCore()
        {
            return new ApplicationCore(this, CreateObjectManager(), CreateRouter(), CreateSiteMap());
        }

        private static IObjectManager CreateObjectManager()
        {
            return new ObjectManager();
        }

        private static Router<Request> CreateRouter()
        {
            var routes = new Routing.RouteTable<Request>();

            routes.Add(new Route<Request, object>(p => new HomePage(), new UriSegmentList("/")));
            routes.Add(new Route<Request, object>(p => new NewsPage(), new UriSegmentList("/news")));
            routes.Add(new Route<Request, object>(p => new AboutPage(), new UriSegmentList("/about")));

            return new Router<Request>(routes);
        }

        private static SiteMap<Request> CreateSiteMap()
        {
            var root = new SiteMapEntry<Request>
            (
                target: new Request(new HttpUrl("/")),
                data: new PageMetaData("home", "this is the homepage"),
                children: New.ReadOnlyList
                (
                    new SiteMapEntry<Request>(new Request(new HttpUrl("/news"), new PageMetaData("News", "this is the news page"))),
                    new SiteMapEntry<Request>(new Request(new HttpUrl("/about"), new PageMetaData("About", "this is the about page")))                
                )                
            );
                        
            return new SiteMap<Request>(root);
        }
    }
}
