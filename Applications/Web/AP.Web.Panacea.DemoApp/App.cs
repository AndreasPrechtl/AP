using AP.ComponentModel.ObjectManagement;
using AP.Routing;
using AP.UI;
using AP.UI.SiteMapping;
using AP.UniformIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Panacea.Web.DemoApp
{
    public class App : AP.Panacea.Web.ApplicationBase
    {
        protected override string SmtpAddress
        {
            get { return null; }
        }

        protected override System.Net.Mail.MailAddress DefaultFromEmailAddress
        {
            get { return null; }
        }

        protected override System.Net.Mail.MailAddressCollection DefaultToEmailAddresses
        {
            get { return null; }
        }

        public App()
            : base()
        { }

        protected override ApplicationCore CreateCore()
        {
            return new ApplicationCore(this, new ObjectManager(), CreateRouter(), CreateSiteMap());
        }

        private static Router<Request> CreateRouter()
        {
            var routes = new Routing.RouteTable<Request>();

            routes.Add(Route.Allowed<Request, object>(p => new PageResult(new HttpUrl("/Pages/Home.aspx")), new UriSegmentList("/")));
            routes.Add(new Route<Request, object>(p => new PageResult(new HttpUrl("/Pages/News.aspx")), new UriSegmentList("/news")));
            routes.Add(new Route<Request, object>(p => new PageResult(new HttpUrl("/Pages/About.aspx")), new UriSegmentList("/about")));

            routes.Add
            (
                new Route<Request, object>
                (   
                    expression: p => null, 
                    uriSegments: new UriSegmentList("/pages/{page}"),
                    action: RoutingAction.Deny
                )
            );

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
