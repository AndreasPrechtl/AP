using System;
using SCG = System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Configuration;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;
using AP.Web.Mvc.Routing;
using AP.Collections;
using AP.Linq;

namespace AP.Web.Mvc
{
    public class SiteMaps : ProviderMapper<SiteMapProviderBase>
    {      
        private SiteMap _rootSiteMap;

        /// <summary>
        /// Builds the complete SiteMap and uses all providers available
        /// </summary>
        public SiteMap RootSiteMap
        {
            get
            {
                if (this.Providers.IsDefaultOrEmpty())
                    return null;

                lock (SyncRoot)
                {
                    if (_rootSiteMap == null)
                    {
                        SiteMap original = Providers.First().SiteMap;
                        _rootSiteMap = (SiteMap)original;

                        foreach (SiteMapProviderBase provider in this.Providers)
                        {
                            if (provider != _rootSiteMap.Provider)
                                _rootSiteMap.Children.Add(provider.SiteMap);
                        }
                    }
                    else
                        _rootSiteMap.Refresh(true);
                }
                return _rootSiteMap;
            }
        }
        //private const string _unknownSiteMapItemsCacheKey = "UnknownSiteMapItemsCacheKey";
        private readonly Dictionary<string, SiteMapItem> _unknownItems = new Dictionary<string, SiteMapItem>();


        private static void EnumerateItems(SiteMapItemGroup group, ref List<SiteMapItem> allItems)
        {
            allItems.Add(group);

            if (group is SiteMap)
                ((SiteMap)group).Refresh(false);

            foreach (SiteMapItemBase item in group.Children)
            {
                if (item is SiteMapItemGroup)
                {   
                    // add everything else as well - note the group will add itself
                    if (item is SiteMapItemGroup)
                        EnumerateItems((SiteMapItemGroup)item, ref allItems);
                }
                else if (item is SiteMapItem)
                    allItems.Add((SiteMapItem)item);
            }
        }
        
        public SiteMapItem CurrentItem
        {
            get
            {
                MvcHandler handler = HttpContext.Current.Handler as MvcHandler;

                if (handler == null)
                    return RootSiteMap;

                RouteValueDictionary routeValues = handler.RequestContext.RouteData.Values;

                // get the sitemap
                SiteMap siteMap = this.RootSiteMap;

                if (siteMap == null)
                    return null;

                List<SiteMapItem> allItems = new List<SiteMapItem>();

                lock (SyncRoot)
                {
                    EnumerateItems(siteMap, ref allItems);
                }

                // now find the siteMapItem
                foreach (SiteMap currentSiteMap in allItems.OfType<SiteMap>())
                {
                    SiteMapItem currentItem = currentSiteMap.CurrentLocalItem;

                    if (currentItem != null)
                        return currentItem;

                    // check if a provider can handle
                    if (currentSiteMap.HasProvider && currentSiteMap.Provider.CanCreateSiteMapItem(routeValues))
                    {
                        currentItem = currentSiteMap.Provider.CreateSiteMapItem(routeValues);

                        currentItem.Parent = currentSiteMap;
                        currentItem.IsVisible = true;

                        return currentItem;
                    }
                }

                // now if it wasn't in there
                SiteMapItem unknownSiteMapItem = null;
                
                unknownSiteMapItem = new SiteMapItemTemplate { RouteValues = routeValues, IsVisible = true };
                
                string key = unknownSiteMapItem.Url;

                SiteMapItem closestItem = null;
                lock (SyncRoot)
                {
                    if (_unknownItems.TryGetValue(key, out closestItem))
                        return closestItem;
                }
                
                SCG.IEnumerable<SiteMapItemTemplate> templates = allItems.OfType<SiteMapItemTemplate>();
                
                // check the virtual items
                foreach (SiteMapItem smi in templates)
                {
                    if (smi.Area == unknownSiteMapItem.Area && smi.Controller == unknownSiteMapItem.Controller)
                    {
                        bool requiresAction = !string.IsNullOrWhiteSpace(smi.Action);
                        if (!requiresAction || smi.Action == unknownSiteMapItem.Action && smi.ParentSiteMap.Provider.CanCreateSiteMapItem(routeValues))
                        {
                            unknownSiteMapItem = smi.ParentSiteMap.Provider.CreateSiteMapItem(routeValues);
                            unknownSiteMapItem.Parent = smi.Parent;

                            _unknownItems.Add(key, unknownSiteMapItem);

                            unknownSiteMapItem.IsVisible = true;

                            return unknownSiteMapItem;
                        }
                    }
                }

                // areas
                foreach (var sm1 in allItems)
                {
                    SiteMapItem cs1 = null;
                    if (sm1.Area == unknownSiteMapItem.Area)
                    {
                        cs1 = sm1;

                        // controllers
                        foreach (var sm2 in allItems)
                        {
                            SiteMapItem cs2 = null;
                            if (sm2.Controller == unknownSiteMapItem.Controller && sm2.Area == unknownSiteMapItem.Area)
                            {
                                cs2 = sm2;

                                // actions
                                foreach (var sm3 in allItems)
                                {
                                    if (sm3.Action == unknownSiteMapItem.Action && sm3.Controller == unknownSiteMapItem.Controller && sm3.Area == unknownSiteMapItem.Area)
                                        cs2 = sm3;                                    
                                }// actions
                                if (cs2 != null && closestItem == null)
                                {
                                    closestItem = cs2;
                                    break;
                                }
                            }
                        }// controllers
                        if (cs1 != null && closestItem == null)
                        {
                            closestItem = cs1;
                            break;
                        }
                    }
                }// areas

                if (closestItem != null)
                {
                    SiteMapItemGroup smg = closestItem as SiteMapItemGroup;
                    if (smg == null)
                        smg = closestItem.Parent;

                    var localSiteMap = closestItem.ParentSiteMap;

                    if (localSiteMap != null && closestItem.ParentSiteMap.Provider.CanCreateSiteMapItem(routeValues))                        
                        unknownSiteMapItem = closestItem.ParentSiteMap.Provider.CreateSiteMapItem(routeValues);
                    
                    // get an altnerate title
                    if (string.IsNullOrWhiteSpace(unknownSiteMapItem.Title) || unknownSiteMapItem.Title == unknownSiteMapItem.Action)
                    {
                        var smis = smg.Children.OfType<SiteMapItem>();
                        
                        var l = smis.Last();
                        // just to use the alternate title - I could use a property on SMITemplate to make it visible/hidden
                        if (unknownSiteMapItem.Action == Actions.Details)
                            unknownSiteMapItem.Title = Actions.Details;
                        else if (unknownSiteMapItem.Action == Actions.List && l.Action == Actions.List)
                            unknownSiteMapItem.Title = l.Title;                        
                    }
                    unknownSiteMapItem.Parent = smg;
                }

                lock(SyncRoot)                
                    _unknownItems[key] = unknownSiteMapItem;
                
                return unknownSiteMapItem;
            }
        }
    }
}
