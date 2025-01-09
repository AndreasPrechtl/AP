using System;

namespace AP.UI.SiteMapping
{
    public partial class SiteMap<TKey>
    {
        private SiteMapEntry<TKey> _root;

        public virtual SiteMapEntry<TKey> Root 
        { 
            get { return _root; }
            protected set { _root = value; }
        }

        public bool FindEntry(TKey key, out SiteMapEntry<TKey> entry, bool returnTemporaryEntry = true)
        {
            return this.Root.FindEntry(key, out entry, returnTemporaryEntry);
        }

        protected SiteMap()
        { }

        public SiteMap(SiteMapEntry<TKey> root)
            : this()
        {
            ArgumentNullException.ThrowIfNull(root);

            _root = root;
        }
    }
}