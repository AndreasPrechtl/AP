using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Serialization;

namespace AP.Web.Mvc
{
    public abstract class SiteMapItemBase : ICloneable, IEquatable<SiteMapItemBase>
    {
        protected bool _isVisible = true;
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        protected SiteMapItemGroup _parent;        

        public SiteMapItemGroup Parent
        {
            get
            {
                return _parent;
            }            
            internal set
            {
                _parent = value;
            }
        }

        public bool HasParent
        {
            get
            {
                return this.Parent != null;
            }
        }

        public SiteMap ParentSiteMap
        {
            get
            {
                var p = _parent;
                while (p != null)
                {
                    if (p is SiteMap)
                        return (SiteMap)p;
                    else
                        p = p._parent;
                }
                return null;
            }            
        }

        public SiteMap RootSiteMap
        {
            get
            {
                var p = _parent;
                SiteMap last = null;

                if (p == null)
                    return Application.SiteMaps.RootSiteMap;
                else
                {
                    while (p != null)
                    {
                        if (p is SiteMap)
                            last = (SiteMap)p;
                        p = p._parent;
                    }
                }
                return last;
            }
        }
            
        public SiteMapItem CurrentItem
        {
            get
            {
                SiteMap sm = RootSiteMap;

                if (sm != null)
                    return sm.CurrentLocalItem;

                return null;                
            }
        }

        public abstract SiteMapItemBase Clone();

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public virtual bool Equals(SiteMapItemBase other)
        {
            return this == other || this.GetType() == other.GetType();
        }
    }    
}
