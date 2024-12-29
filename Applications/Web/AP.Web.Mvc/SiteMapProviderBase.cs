using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;
using System.Collections.Specialized;
using AP.ComponentModel.ObjectManagement;
using AP.Configuration;
using AP.Data;
using AP.Web.Mvc.Routing;

namespace AP.Web.Mvc
{
    public abstract class SiteMapProviderBase : ProviderBase
    {
        private SiteMap _siteMap;

        public SiteMapItem CreateSiteMapItem(RouteValueDictionary routeValues)
        {
            if (!CanCreateSiteMapItem(routeValues))
                throw new InvalidOperationException("Cannot create a SiteMapItem for the given routeValues");

            return this.OnCreateSiteMapItem(routeValues);
        }

        protected abstract SiteMapItem OnCreateSiteMapItem(RouteValueDictionary routeValues);

        public abstract bool CanCreateSiteMapItem(RouteValueDictionary routeValues);
        
        public virtual SiteMap SiteMap
        {
            get
            {
                //if (_siteMap == null)
                //    return null;

                //SiteMap[] indexedSiteMaps = _siteMap.IndexedItems.OfType<SiteMap>().ToArray();

                //int length = indexedSiteMaps.Length;

                //for (int i = 0; i < length; i++)
                //{
                //    SiteMap sm = indexedSiteMaps[i];

                //    // update only if the current SiteMap has a parent - otherwise it's the RootSiteMap
                //    if (sm.HasParent)
                //    {
                //        SiteMapItemGroup p = sm.Parent;

                //        int realIndex = p.Children.IndexOf(sm);
                //        p.Children.Insert(realIndex, sm.Provider.LocalSiteMap);
                //        p.Children.RemoveAt(realIndex + 1);
                //    }
                //}
                return _siteMap;
            }
            protected set
            {
                _siteMap = value;
            }
        }

        //public SiteMap RootSiteMap
        //{
        //    get
        //    {
        //        if (this.IsRegistered)
        //            return ((SiteMaps)this.Mapper).RootSiteMap;
        //        else
        //            return this.LocalSiteMap;
        //    }
        //}

        //public SiteMapItem CurrentItem
        //{
        //    get 
        //    {
        //        if (this.IsRegistered)
        //            return ((SiteMaps)this.Mapper).CurrentItem;
        //        else
        //            return this.CurrentLocalItem;
        //    }
        //}

        //public SiteMapItem CurrentLocalItem 
        //{
        //    get
        //    {
        //        return this.LocalSiteMap.CurrentLocalItem;
        //    }
        //}
    }
}
