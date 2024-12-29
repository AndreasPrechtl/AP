using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.Web.Mvc
{
    public partial class SiteMapItemGroup : SiteMapItem
    {
        internal SiteMapItemCollection _children;
        
        //public IEnumerable<SiteMapItem> IndexedItems
        //{
        //    get
        //    {
        //        yield return this;

        //        foreach (var item in _children.OfType<SiteMapItem>())
        //        {                    
        //            SiteMapItemGroup group = item as SiteMapItemGroup;

        //            if (group != null)
        //            {
        //                IEnumerable<SiteMapItem> children = group.IndexedItems;

        //                foreach (var child in children)
        //                    yield return child;
        //            }
        //            else
        //                yield return item;
        //        }

        //        //List<SiteMapItem> items = new List<SiteMapItem>();

        //        //items.Add(this);

        //        //foreach (var item in _children.OfType<SiteMapItem>())
        //        //{                    
        //        //    items.Add(item);

        //        //    if (item is SiteMapItemGroup)
        //        //        items.AddRange(((SiteMapItemGroup)item).IndexedItems);                    
        //        //}

        //        //foreach (var item in items)
        //        //    yield return item;
        //    }
        //}

        public SiteMapItemCollection Children
        {
            get
            {
                return _children;
            }
        }

        public bool HasChildren
        {
            get
            {
                return this.Children.Count > 0;
            }
        }

        public SiteMapItemGroup()
        {
            _children = new SiteMapItemCollection(this);            
        }

        public override SiteMapItemBase Clone()
        {
            SiteMapItemGroup clone = (SiteMapItemGroup)base.Clone();
            
            // shallow?
            //clone._children = this._children;

            // or deep?
            foreach (SiteMapItemBase item in this.Children)
                clone.Children.Add(item.Clone());

            return clone;
        }
    }
}
