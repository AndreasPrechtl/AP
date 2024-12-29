using System;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Routing;
using AP.Web;
using AP.Web.Mvc;
using AP.Web.Mvc.Security;
using AP.Web.Mvc.Routing;
using AP.Data;
//using AP.Website.Models;
using AP.Configuration;
using AP.Data.EntityFramework;
using AP.Web.ComponentModel.ObjectManagement;
using System.Data.Entity;

namespace AP.Website
{
    public class App : AP.Web.Mvc.Application
    {
        public static new App Current
        {
            get
            {
                return (App)AP.Web.Mvc.Application.Current;
            }
        }

        private class EntitySetProviderInternal : AP.Data.IEntitySetProvider
        {
            public IEntitySet<TEntity> GetEntitySet<TEntity>() where TEntity : class
            {
                return null;
                //return (IEntitySet<TEntity>)Activator.CreateInstance(typeof(EntitySet<>).MakeGenericType(typeof(TEntity)), ObjectManager.GetInstance<EntityFrameworkContext>(), false);
            }
        }

        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            App.SiteMaps.Register(new SiteMapProvider());
            //App.ObjectManager.Register(new RequestLifetime<EntityFrameworkContext>(() => new EntityFrameworkContext(new ApplicationModel(), true)));
            //    App.EntitySetProvider = new EntitySetProviderInternal();
        }


        //private class EntitySetProviderInternal : AP.Data.IEntitySetProvider
        //{
        //    public IEntitySet<TEntity> GetEntitySet<TEntity>() where TEntity : class
        //    {
        //        return (IEntitySet<TEntity>)Activator.CreateInstance(typeof(EntitySet<>).MakeGenericType(typeof(TEntity)), new EntityFrameworkContext(new ApplicationModel()));
        //    }
        //}

        //protected override void InitializeComponent()
        //{
        //    base.InitializeComponent();
        //    App.EntitySetProvider = new EntitySetProviderInternal(); 
        //}


        protected override void RegisterRoutes(RouteCollection routeCollection)
        {
            base.RegisterRoutes(routeCollection);

            routeCollection.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routeCollection.IgnoreRoute("Views/Shared/Error.aspx");
            routeCollection.IgnoreRoute("favicon.ico");

            routeCollection.MapRoute("Error", "Error", new { controller = "Error", action = "Error" });

            routeCollection.MapRoute("About", "About", new { controller = "Home", action = "About" });
            routeCollection.MapRoute("Blog", "Blog", new { controller = "Blog", action = "Index" });

            routeCollection.MapRoute("ListIndex", "{controller}/List/{page}/{size}", new { controller = "Data", action = "List", page = 0, size = Settings.PagedViewModelSize });
            routeCollection.MapRoute("EmptyResults", "{controller}/Empty", new { controller = "Data", action = "Empty" });

            //routeCollection.MapRoute("Galleries", "Galleries", new { controller = "Galleries", action = "List", page = 0, size = Settings.PagedViewModelSize });
            //routeCollection.MapRoute("News", "News", new { controller = "News", action = "List", page = 0, size = Settings.PagedViewModelSize });
            //routeCollection.MapRoute("SpecialOffers", "SpecialOffers", new { controller = "SpecialOffers", action = "List", page = 0, size = Settings.PagedViewModelSize });

            //routeCollection.MapRoute("EditFiles", "{controller}/EditFiles/{id}/{*directoryName}", new { controller = "Data", action = "EditFiles" });
            //routeCollection.MapRoute("DeleteFile", "{controller}/DeleteFile/{id}/{*fileName}", new { controller = "Data", action = "DeleteFile" });
            //routeCollection.MapRoute("DeleteDirectory", "{controller}/DeleteDirectory/{id}/{*directoryName}", new { controller = "Data", action = "DeleteDirectory" });
            //routeCollection.MapRoute("StoreFiles", "{controller}/StoreFiles/{id}/{*directoryName}", new { controller = "Data", action = "StoreFiles" });

            routeCollection.MapRoute("LogOn", "Account/LogOn/{*returnUrl}", new { controller = "Account", action = "LogOn", returnUrl = RouteValues.Empty });
            routeCollection.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = RouteValues.Empty });
        }
    }
}

