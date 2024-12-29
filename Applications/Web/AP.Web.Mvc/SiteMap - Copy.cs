using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web;
using AP.Web.Mvc.Routing;
using AP.Linq;
using AP.Collections;
using System.Reflection;

namespace AP.Web.Mvc
{
    public class SiteMap : SiteMapItemGroup
    {
        public SiteMap(SiteMapProviderBase provider = null)
        {
            _providerDisposingHandler = HandleProviderDisposingEvent;
            this.Provider = provider;            
        }

        public NavigationData CurrentLocalItem
        {
            get
            {
                MvcHandler handler = HttpContext.Current.Handler as MvcHandler;
                
                if (handler == null)
                    return null;

                RouteValueDictionary routeValues = handler.RequestContext.RouteData.Values;

                string url = UrlHelperExtensions.Action(routeValues);

                if (this.Url.Equals(url, StringComparison.InvariantCultureIgnoreCase))
                    return this;

                foreach (NavigationData item in this.Children.OfType<NavigationData>())
                {
                    if (item.Url.Equals(UrlHelperExtensions.Action(routeValues)))
                        return item;
                }
                return null;
            }
        }
        
        private SiteMapProviderBase _provider;
        private DisposingEventHandler _providerDisposingHandler;

        //private bool _refreshOnEnumeration = false;

        /// <summary>
        /// Gets or sets a value that indicates if the SiteMap will be refreshed automatically when the SiteMapItemEnumerator is used
        /// </summary>
        //public bool RefreshOnEnumeration
        //{
        //    get { return _refreshOnEnumeration; }
        //    set { _refreshOnEnumeration = true; }
        //}

        /// <summary>
        /// Reloads the complete sitemap from the provider
        /// </summary>
        public virtual void Refresh(bool refreshChildSiteMaps = true)
        {
            if (this.HasProvider)
            {
                SiteMap sm = Provider.SiteMap;

                // copy the new values
                if (sm != this)
                {
                    _routeValues = sm._routeValues;
                    _title = sm._title;
                    _url = sm._url;
                    _protocol = sm._protocol;
                    _area = sm._area;
                    _action = sm._action;
                    _controller = sm._controller;
                    _description = sm._description;
                    _hostName = sm._hostName;
                    _imageUrl = sm._imageUrl;
                    _isVisible = sm._isVisible;
                    _children.Clear();

                    foreach (SiteMapItemBase item in sm._children)
                    {
                        item.Parent = null;
                        _children.Add(item);

                        if (refreshChildSiteMaps && item is SiteMap)
                            ((SiteMap)item).Refresh(refreshChildSiteMaps);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the SiteMapProvider that generated the SiteMap
        /// </summary>
        public SiteMapProviderBase Provider
        {
            get
            {
                return _provider;
            }
            set
            {
                SiteMapProviderBase p = _provider;

                if (p != value)
                {
                    // remove the eventhandler if p exists
                    if (p != null)
                        p.Disposing -= _providerDisposingHandler;

                    if (value != null)
                        value.Disposing += _providerDisposingHandler;

                    _provider = value;
                }
            }
        }

        public override SiteMapItemBase Clone()
        {
            SiteMap clone = (SiteMap)base.Clone();

            clone.Provider = this.Provider;

            return clone;
        }

        public bool HasProvider
        {
            get { return _provider != null; }
        }              
        
        private void HandleProviderDisposingEvent(object sender, EventArgs e)
        {
            SiteMapProviderBase p = sender as SiteMapProviderBase;

            if (p != null && p == _provider)
                _provider = null;            
        }
    }
}
