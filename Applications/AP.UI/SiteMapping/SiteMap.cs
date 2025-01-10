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

        public bool FindEntry(TKey key, out SiteMapEntry<TKey>? entry, bool returnTemporaryEntry = true)
        {
            return this.Root.FindEntry(key, out entry, returnTemporaryEntry);
        }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        protected SiteMap() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


        public SiteMap(SiteMapEntry<TKey> root)
            : this()
        {
            ArgumentNullException.ThrowIfNull(root);

            _root = root;
        }
    }
}