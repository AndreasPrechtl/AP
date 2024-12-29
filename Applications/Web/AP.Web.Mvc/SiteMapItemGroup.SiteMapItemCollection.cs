using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;

namespace AP.Web.Mvc
{
    public partial class SiteMapItemGroup
    {       
        public sealed class SiteMapItemCollection : IList<SiteMapItemBase>, ICollection<SiteMapItemBase>, IList, ICollection
        {
            private readonly SiteMapItemGroup _group;
            internal readonly List<SiteMapItemBase> _items;

            internal SiteMapItemCollection(SiteMapItemGroup group)
            {
                _group = group;
                _items = new List<SiteMapItemBase>();
            }

            public IEnumerator<SiteMapItemBase> GetEnumerator()
            {
                return _items.GetEnumerator();
            }

            #region IList<SiteMapItemBase> Members

            public int IndexOf(SiteMapItemBase item)
            {
                return _items.IndexOf(item);
            }

            public void Insert(int index, SiteMapItemBase item)
            {
                if (item.HasParent)
                    throw new InvalidOperationException("Parent");

                item.Parent = _group;

                _items.Insert(index, item);
            }

            public void RemoveAt(int index)
            {
                SiteMapItemBase smi = _items[index];
                smi.Parent = null;
                
                _items.RemoveAt(index);
            }

            public SiteMapItemBase this[int index]
            {
                get
                {
                    return _items[index];
                }
                set
                {
                    _items[index] = value;
                }
            }

            #endregion

            #region ICollection<SiteMapItemBase> Members

            public void Add(SiteMapItemBase item)
            {
                this.Insert(this.Count, item);
            }

            public void Clear()
            {
                foreach (SiteMapItemBase item in this)
                    item.Parent = null;

                _items.Clear();
            }

            public bool Contains(SiteMapItemBase item)
            {
                return _items.Contains(item);
            }

            void ICollection<SiteMapItemBase>.CopyTo(SiteMapItemBase[] array, int arrayIndex)
            {
                _items.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get { return _items.Count; }
            }

            bool ICollection<SiteMapItemBase>.IsReadOnly
            {
                get { return ((ICollection<SiteMapItemBase>)_items).IsReadOnly; }
            }

            public bool Remove(SiteMapItemBase item)
            {
                bool b = _items.Remove(item);
                if (b)
                    item.Parent = null;

                return b;
            }
            
            #endregion

            #region IEnumerable Members

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            #endregion

            #region IList Members

            int IList.Add(object value)
            {
                this.Add((SiteMapItemBase)value);

                return this.Count - 1;
            }

            void IList.Clear()
            {
                this.Clear();
            }

            bool IList.Contains(object value)
            {
                return this.Contains((SiteMapItemBase)value);
            }

            int IList.IndexOf(object value)
            {
                return this.IndexOf((SiteMapItemBase)value);
            }

            void IList.Insert(int index, object value)
            {
                this.Insert(index, (SiteMapItemBase)value);
            }

            bool IList.IsFixedSize
            {
                get { return ((IList)_items).IsFixedSize; }
            }

            bool IList.IsReadOnly
            {
                get { return ((IList)_items).IsReadOnly; }
            }

            void IList.Remove(object value)
            {
                this.Remove((SiteMapItemBase)value);
            }

            object IList.this[int index]
            {
                get
                {
                    return this[index];
                }
                set
                {
                    this[index] = (SiteMapItemBase)value;
                }
            }

            #endregion

            #region ICollection Members

            void ICollection.CopyTo(Array array, int index)
            {
                ((ICollection)_items).CopyTo(array, index);
            }

            int ICollection.Count
            {
                get { return this.Count; }
            }

            bool ICollection.IsSynchronized
            {
                get { return ((ICollection)_items).IsSynchronized; }
            }

            object ICollection.SyncRoot
            {
                get { return ((ICollection)_items).SyncRoot; }
            }

            #endregion
        }
    }
}