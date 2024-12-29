using System;
using System.Collections.Generic;

namespace AP.UI.SiteMapping
{
    public partial class SiteMapEntry<TKey>
    {
        private bool _isTemporary;
        private readonly TKey _target;
        private readonly PageMetaData _data;

        internal SiteMap<TKey> _siteMap;
        internal SiteMapEntry<TKey> _parent;
        internal SiteMapEntry<TKey> _next;
        internal SiteMapEntry<TKey> _previous;
        private EntryList _children;

        /// <summary>
        /// Gets the root entry
        /// </summary>
        public SiteMapEntry<TKey> Root
        {
            get
            {
                SiteMap<TKey> sm = this.SiteMap;

                if (sm != null)
                    return sm.Root;

                if (_parent == null)
                    return this;

                return _parent.Root;
            }
        }

        public bool HasData { get { return _data != null; } }

        public bool IsRoot
        {
            get
            {
                return _parent == null || this.Root == this;
            }
        }

        /// <summary>
        /// Returns true when the Entry has a parent.
        /// </summary>
        public bool HasParent { get { return _parent != null; } }

        // this might be(come) a problem...

        // do I need to set that as an internal - probably best if you want to find out where you currently are
        // but it doesn't have to be added into the parent's entry list
        public SiteMapEntry<TKey> Parent 
        { 
            get { return _parent; }           
            protected set 
            {
                if (!value.IsTemporary)
                    throw new ArgumentException("entry has to be temporary in order to to set the parent");

                _parent = value; 
            }
        }

        public SiteMapEntry<TKey> Next { get { return _next; } }
        public SiteMapEntry<TKey> Previous { get { return _previous; } }

        public TKey Target { get { return _target; } }
        public PageMetaData Data { get { return _data; } }
        public bool IsTemporary { get { return _isTemporary; } }

        /// <summary>
        /// Gets the SiteMap. Returns null if the Entry wasn't added to a SiteMap.
        /// </summary>
        public SiteMap<TKey> SiteMap
        {
            get
            {
                SiteMap<TKey> sm = _siteMap;

                if (sm == null && _parent != null)
                    _siteMap = sm = _parent.SiteMap;

                return sm;
            }
            internal set
            {
                _siteMap = value;
            }
        }

        public virtual EntryList Children
        {
            get { return _children; }
            protected set { _children = value ?? EntryList.Empty; }
        }

        public virtual bool HasChildren
        {
            get { return _children != EntryList.Empty && _children != null && _children.Count > 0; }
        }

        public bool HasTarget
        {
            get { return _target != null; }
        }

        public SiteMapEntry(TKey target = default(TKey), PageMetaData data = default(PageMetaData), IEnumerable<SiteMapEntry<TKey>> children = null)
        {
            _target = target;
            _data = data;

            // Use the virtual member or not? I think it is a better design without doing that.
            _children = children != null ? new EntryList(this, children) : EntryList.Empty;

            //_isTemporary = isTemporary;
        }

        public virtual bool FindEntry(TKey target, out SiteMapEntry<TKey> entry, bool returnTemporaryEntry = true)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            entry = null;

            // if the targets already match - return the entry itself.
            if (this.HasTarget && _target.Equals(target))
            {
                entry = this;
                return true;
            }

            EntryList children = _children;

            if (returnTemporaryEntry && (children == EntryList.Empty || children == null))
                return this.TryCreateTemporaryEntry(target, out entry);

            // create a list of temporary entries to further narrow down the results
            AP.Collections.List<SiteMapEntry<TKey>> temporaryEntries = new AP.Collections.List<SiteMapEntry<TKey>>();

            foreach (SiteMapEntry<TKey> current in children)
            {
                // if the recursion yields results - analyse the result, if it's just a temporary item - add it to the list - otherwise return the found entry directly
                if (current.FindEntry(target, out entry, returnTemporaryEntry))
                {
                    // that will only happen if returnTemporaryEntry == true
                    if (!entry.IsTemporary)
                        temporaryEntries.Add(entry);
                    else
                        return true;
                }
            }

            // don't filter if it isn't supposed to have temporary entries
            if (!returnTemporaryEntry)
                return false;

            // see if some temporary entries exist
            if (this.FilterTemporaryEntries(temporaryEntries, out entry))
                return true;

            // if not - and this item here is the root - create an entry for an unknown entry - routing should have thrown an error when the target doesn't exist.
            if (!this.HasParent)
                return this.TryCreateTemporaryEntry(target, out entry);

            // it's not the root - keep it clear
            entry = null;
            return false;
        }

        protected virtual bool FilterTemporaryEntries(AP.Collections.IListView<SiteMapEntry<TKey>> temporaryEntries, out SiteMapEntry<TKey> entry)
        {
            // change that algorithm to analyze the depth or just make it return the first entry
            if (temporaryEntries.Count > 0)
            {
                entry = temporaryEntries[0];
                return true;
            }

            entry = null;
            return false;
        }

        /// <summary>
        /// Makes a parentless entry temporary and assigns the parent entry.
        /// Note: The entry will not have siblings.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <exception cref="System.ArgumentException">Thrown when the entry already has a parent.</exception>
        protected void MakeTemporary(SiteMapEntry<TKey> entry)
        {
            if (entry.HasParent)
                throw new ArgumentException("entry already has a parent");

            entry._parent = this;
            entry._isTemporary = true;
        }

        /// <summary>
        /// Tries to create a new temporary entry if no entry could be found among the children.
        /// When overridden and an entry could be created, use RegisterTemporary on the entry to assign parent and the temporary status.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        protected virtual bool TryCreateTemporaryEntry(TKey target, out SiteMapEntry<TKey> entry)
        {
            entry = new SiteMapEntry<TKey>(target);
            this.MakeTemporary(entry);

            return true;            
        }

        public override string ToString()
        {
            return this.HasTarget ? this.Target.ToString() : base.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;

            if (obj == null)
                return false;

            // don't need to compare it when I don't have anything else to compare it with
            if (!this.HasTarget)
                return false;

            if (obj is SiteMapEntry<TKey>)
            {
                SiteMapEntry<TKey> other = (SiteMapEntry<TKey>)obj;

                return other.HasTarget && this.Target.Equals(other.Target);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.HasTarget ? this.Target.GetHashCode() : base.GetHashCode();
        }
    }
}