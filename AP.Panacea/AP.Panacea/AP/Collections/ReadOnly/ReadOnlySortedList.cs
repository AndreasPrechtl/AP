using AP.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace AP.Collections.ReadOnly
{
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false), System.ComponentModel.ReadOnly(true), Sorted(true)]
    public class ReadOnlySortedList<T> : IListView<T>, IComparerUser<T>, IListView, System.Collections.Generic.IList<T>, System.Collections.IList
    {
        private readonly AP.Collections.SortedList<T> _inner;
        private static volatile ReadOnlySortedList<T> _empty;

        public static ReadOnlySortedList<T> Empty
        {
            get
            {
                ReadOnlySortedList<T> empty = _empty;

                if (empty == null)
                    _empty = empty = new ReadOnlySortedList<T>((IListView<T>)new List<T>(0));

                return empty;
            }
        }

        private static AP.Collections.SortedList<T> CreateInner(IEnumerable<T> collection, IComparer<T> comparer)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            return new SortedList<T>(collection, comparer);
        }

        public ReadOnlySortedList(IEnumerable<T> collection)
            : this(collection, null)
        { }

        public ReadOnlySortedList(IEnumerable<T> collection, IComparer<T> comparer)
            : this(CreateInner(collection, comparer))
        { }

        protected ReadOnlySortedList(AP.Collections.SortedList<T> inner)
        {
            if (inner == null)
                throw new ArgumentNullException("inner");

            _inner = inner;
        }

        public override string ToString()
        {
            return _inner.ToString();
        }
        
        #region IListView<T> Members

        public T this[int index]
        {
            get { return _inner[index]; }
        }

        public IEnumerable<T> this[int index, int count = 1]
        {
            get { return _inner[index, count]; }
        }

        public void CopyTo(T[] array, int arrayIndex = 0, int listIndex = 0, int? count = null)
        {
            _inner.CopyTo(array, arrayIndex, listIndex, count);
        }

        public int IndexOf(T item, SelectionMode mode = SelectionMode.First)
        {
            return _inner.IndexOf(item, mode);
        }

        public bool Contains(T item)
        {
            return _inner.Contains(item);
        }

        public bool Contains(T item, out int index, SelectionMode mode = SelectionMode.First)
        {
            return _inner.Contains(item, out index, mode);
        }
        
        public bool TryGetItem(int index, out T item)
        {
            return _inner.TryGetItem(index, out item);
        }

        #endregion

        #region ICollection<T> Members

        public int Count
        {
            get { return _inner.Count; }
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            ((ICollection<T>)_inner).CopyTo(array, arrayIndex);
        }

        bool ICollection<T>.Contains(T item)
        {
            return ((ICollection<T>)_inner).Contains(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region ICloneable Members

        public ReadOnlySortedList<T> Clone()
        {
            return this;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #region IComparerUser<T> Members

        public IComparer<T> Comparer
        {
            get { return _inner.Comparer; }
        }

        #endregion

        #region IList<T> Members

        int System.Collections.Generic.IList<T>.IndexOf(T item)
        {
            return this.IndexOf(item, SelectionMode.First);
        }

        void System.Collections.Generic.IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        void System.Collections.Generic.IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        T System.Collections.Generic.IList<T>.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region ICollection<T> Members

        void System.Collections.Generic.ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void System.Collections.Generic.ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        bool System.Collections.Generic.ICollection<T>.IsReadOnly
        {
            get { return true; }
        }

        void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int index)
        {
            this.CopyTo(array, index, 0, null);
        }

        bool System.Collections.Generic.ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IList Members

        int System.Collections.IList.Add(object value)
        {
            throw new NotSupportedException();
        }

        void System.Collections.IList.Clear()
        {
            throw new NotSupportedException();
        }

        bool System.Collections.IList.Contains(object value)
        {
            return CollectionsHelper.IsCompatible<T>(value) && this.Contains((T)value);
        }

        int System.Collections.IList.IndexOf(object value)
        {
            return CollectionsHelper.IsCompatible<T>(value) ? this.IndexOf((T)value, SelectionMode.First) : -1;
        }

        void System.Collections.IList.Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        bool System.Collections.IList.IsFixedSize
        {
            get { return true; }
        }

        bool System.Collections.IList.IsReadOnly
        {
            get { return true; }
        }

        void System.Collections.IList.Remove(object value)
        {
            throw new NotSupportedException();
        }

        void System.Collections.IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        object System.Collections.IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region ICollection Members

        void System.Collections.ICollection.CopyTo(Array array, int index)
        {
            ((System.Collections.ICollection)_inner).CopyTo(array, index);
        }

        bool System.Collections.ICollection.IsSynchronized
        {
            get { return true; }
        }

        object System.Collections.ICollection.SyncRoot
        {
            get { return ((System.Collections.ICollection)_inner).SyncRoot; }
        }

        #endregion

        #region IListView Members

        IEnumerable IListView.this[int index, int count]
        {
            get { return ((IListView)_inner)[index, count]; }
        }

        void IListView.CopyTo(Array array, int arrayIndex, int listIndex, int? count)
        {
            ((IListView)_inner).CopyTo(array, arrayIndex, listIndex, count);
        }

        int IListView.IndexOf(object item, SelectionMode mode)
        {
            return ((IListView)_inner).IndexOf(item, mode);
        }

        bool IListView.Contains(object item, out int index, SelectionMode mode)
        {
            return ((IListView)_inner).Contains(item, out index, mode);
        }

        bool IListView.TryGetItem(int index, out object item)
        {
            return ((IListView)_inner).TryGetItem(index, out item);
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            ((ICollection)_inner).CopyTo(array, arrayIndex);
        }

        bool ICollection.Contains(object item)
        {
            return ((ICollection)_inner).Contains(item);
        }

        #endregion
    }
}
