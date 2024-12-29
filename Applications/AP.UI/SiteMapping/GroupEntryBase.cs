using System;
using System.Linq;
using AP.UniversalIdentifiers;

namespace AP.UI.SiteMapping
{
    public abstract partial class GroupEntryBase : EntryBase, IPageEntryInternal, IPageMetaDataProvider
    {
        protected GroupEntryBase(IUri uri, PageMetaData metaData)
            : this(uri != null ? new PageEntry(EntryType.GroupPage, uri, metaData, false) : null)
        { }

        private GroupEntryBase(PageEntry page)
            : base(page != null ? EntryType.GroupPage : EntryType.Group)
        {
            this.Page = page;
        }

        public abstract EntryCollection Children { get; }

        public bool HasCurrentEntry(IUri uri, out PageEntry entry, bool returnTemporary = true, bool useRecursion = true)
        {
            // check if the page might actually be the correct one
            if (this.Page != null && this.Page.Uri.Equals(uri))
            {
                entry = this.Page;

                return true;
            }
            
            // check each and every uri within the children
            foreach (var child in this.Children)
            {
                PageEntry pe = null;

                bool isGroup = (child.Type & EntryType.Group) == EntryType.Group;

                if (isGroup)
                    pe = ((GroupEntryBase)child).Page;
                else if (child.Type == EntryType.Page)
                    pe = (PageEntry)child;
                
                if (pe != null && pe.Uri.Equals(uri))
                {
                    entry = pe;
                    return true;
                }
                
                // check if recursion will yield the wanted result
                if (isGroup && useRecursion && ((GroupEntryBase)child).HasCurrentEntry(uri, out entry, returnTemporary, true))
                    return true;
            }

            PageMetaData meta = null;
            if (this.IsPotentialEntry(uri, out meta))
            {
                entry = new PageEntry(EntryType.Page, uri, meta, true) { Parent = this };
                return true;
            }
            entry = null;

            return false;
        }

        /// <summary>
        /// Checks if the uri would fit into the group and creates the metadata accordingly
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="metaData"></param>
        /// <returns></returns>
        protected abstract bool IsPotentialEntry(IUri uri, out PageMetaData metaData);

        #region IPageEntryInternal Members
        
        public PageEntry Page { get; private set; }

        public IUri Uri
        {
            get { return this.Page != null ? this.Page.Uri : null; }
        }

        public PageMetaData MetaData
        {
            get { return this.Page != null ? this.Page.MetaData : null; }
        }

        #endregion

        #region IPageMetaDataProvider Members

        PageMetaData IPageMetaDataProvider.GetMetaData(IUri uri)
        {
            PageMetaData meta = null;
            PageEntry page = null;

            if (this.HasCurrentEntry(uri, out page, true, true))
                return page.MetaData;

            return null;
        }

        #endregion
    }
}