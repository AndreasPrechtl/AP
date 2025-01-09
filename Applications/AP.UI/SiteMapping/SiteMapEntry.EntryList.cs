using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Collections;

namespace AP.UI.SiteMapping
{
    public partial class SiteMapEntry<TKey>
    {
        public sealed class EntryList : AP.Collections.ReadOnly.ReadOnlyList<SiteMapEntry<TKey>>
        {
            private static volatile EntryList _empty;
            
            public static new EntryList Empty 
            { 
                get 
                { 
                    EntryList empty = _empty;

                    if (empty == null)
                        _empty = empty = new EntryList(AP.Collections.ReadOnly.ReadOnlyList<SiteMapEntry<TKey>>.Empty);

                    return empty;
                }
            }

            private static AP.Collections.IListView<SiteMapEntry<TKey>> CreateInnerList(SiteMapEntry<TKey> entry, IEnumerable<SiteMapEntry<TKey>> collection)
            {
                ArgumentNullException.ThrowIfNull(entry);

                AP.Collections.List<SiteMapEntry<TKey>> entries = new AP.Collections.List<SiteMapEntry<TKey>>();

                SiteMapEntry<TKey> last = null;

                // iterate and set the necessary relations
                foreach (SiteMapEntry<TKey> current in collection)
                {
                    // just a short test - more than 1 parent isn't working
                    if (current.HasParent)
                        throw new ArgumentException("collection contains elements that already have parents");

                    //// probably good to test that as well...
                    //if (current.IsTemporary)
                    //    throw new ArgumentException("collection cannot contain temporary items");

                    current._parent = entry;

                    if (last != null)
                    {
                        current._previous = last;
                        last._next = current;
                    }
                    last = current;

                    entries.Add(current);
                }

                return entries;
            }

            private EntryList(IListView<SiteMapEntry<TKey>> collection)
                : base(collection)
            { }

            public EntryList(SiteMapEntry<TKey> entry, IEnumerable<SiteMapEntry<TKey>> children)
                : this(CreateInnerList(entry, children))
            { }
        }
    }
}
