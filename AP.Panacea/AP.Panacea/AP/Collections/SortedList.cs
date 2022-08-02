using AP.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AP.Collections
{
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false), Sorted(true)]
    public class SortedList<T> : IList<T>, IComparerUser<T>, IList
    {
        private readonly IComparer<T> _comparer;
        internal readonly System.Collections.Generic.List<T> _inner = new System.Collections.Generic.List<T>();

        public SortedList()
            : this(0)
        { }

        public SortedList(int capacity)
            : this(capacity, null)
        { }

        public SortedList(int capacity, IComparer<T> comparer)
            : this(new System.Collections.Generic.List<T>(capacity), comparer ?? Comparer<T>.Default)
        { }

        public SortedList(IEnumerable<T> collection)
            : this(collection, null)
        { }

        public SortedList(IEnumerable<T> collection, IComparer<T> comparer)
            : this(collection == null ? new System.Collections.Generic.List<T>() : new System.Collections.Generic.List<T>(collection), comparer ?? Comparer<T>.Default)
        { }

        protected SortedList(System.Collections.Generic.List<T> inner, IComparer<T> comparer)
        {
            if (inner == null)
                throw new ArgumentNullException("inner");

            if (comparer == null)
                throw new ArgumentNullException("comparer");

            _inner = inner;
            inner.Sort(comparer);
            _comparer = comparer;
        }

        public override string ToString()
        {
            return _inner.ToString();
        }

        #region ISortedList<T> Members

        public int Add(T item)
        {
            int count = _inner.Count;

            // special case 1
            if (count == 0)
            {
                _inner.Add(item);
                return 0;
            }

            if (count == 1)
            {
                int index = _comparer.Compare(item, _inner[0]) <= 0 ? 0 : 1;
                _inner.Insert(index, item);
                return index;
            }

            // search 
            for (int i = 0, j = _inner.Count; i <= --j; ++i)
            {
                if (_comparer.Compare(item, _inner[i]) <= 0)
                {
                    _inner.Insert(i, item);
                    return i;
                }
                else if (_comparer.Compare(item, _inner[j]) >= 0)
                {
                    int index = j + 1;
                    _inner.Insert(index, item);
                    return index;
                }

                if (i + 1 == j)
                {
                    _inner.Insert(j, item);
                    return j;
                }
            }

            // will never get hit - only for compilation
            return count;
        }

        public void Add(IEnumerable<T> items)
        {
            foreach (T item in items)
                this.Add(item);
        }

        public void Remove(T item, SelectionMode mode = SelectionMode.First)
        {
            switch (mode)
            {
                case SelectionMode.First:
                    for (int c = _inner.Count, i = 0; i < c; ++i)
                    {
                        if (_comparer.Compare(_inner[i], item) == 0)
                        {
                            _inner.RemoveAt(i);
                            break;
                        }
                    }
                    break;
                case SelectionMode.Last:
                    for (int i = _inner.Count; --i > -1; )
                    {
                        if (_comparer.Compare(_inner[i], item) == 0)
                        {
                            _inner.RemoveAt(i);
                            break;
                        }
                    }
                    break;
                case SelectionMode.All:

                    for (int i = 0, j = _inner.Count; i <= --j; ++i)
                    {
                        bool r = _comparer.Compare(_inner[i], item) == 0;

                        if (r)
                        {
                            _inner.RemoveAt(i);
                            i--;
                        }

                        // already checked
                        if (i == j)
                            break;

                        r = _comparer.Compare(_inner[j], item) == 0;

                        if (r)
                        {
                            _inner.RemoveAt(j);
                            j--;
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
        }

        public void Remove(int index, int count = 1)
        {
            if (count == 1)
                _inner.RemoveAt(index);
            else
                _inner.RemoveRange(index, count);
        }

        public void Clear()
        {
            _inner.Clear();
        }

        #endregion

        #region IListView<T> Members

        public T this[int index]
        {
            get { return _inner[index]; }
        }

        public IEnumerable<T> this[int index, int count = 1]
        {
            get
            {
                if (index < 0 || index >= _inner.Count)
                    throw new ArgumentOutOfRangeException("index");

                if (index + count > _inner.Count)
                    throw new ArgumentOutOfRangeException("count");

                for (int i = 0; i < count; ++i)
                    yield return _inner[index++];
            }
        }

        public void CopyTo(T[] array, int arrayIndex = 0, int listIndex = 0, int? count = null)
        {
            if (count.HasValue)
                _inner.CopyTo(listIndex, array, arrayIndex, count.Value);
            else if (listIndex > 0)
                _inner.CopyTo(listIndex, array, arrayIndex, _inner.Count - listIndex);
            else
                _inner.CopyTo(array, arrayIndex);
        }

        public int IndexOf(T item, SelectionMode mode = SelectionMode.First)
        {
            switch (mode)
            {
                case SelectionMode.First:
                    for (int i = 0, c = _inner.Count; i < c; ++i)
                    {
                        if (_comparer.Compare(_inner[i], item) == 0)
                            return i;
                    }
                    break;
                case SelectionMode.Last:
                    for (int i = _inner.Count; --i > -1; )
                    {
                        if (_comparer.Compare(_inner[i], item) == 0)
                            return i;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }

            return -1;
        }

        public bool Contains(T item, out int index, SelectionMode mode = SelectionMode.First)
        {
            return (index = this.IndexOf(item, mode)) > -1;
        }

        public bool Contains(T item)
        {
            return this.IndexOf(item, SelectionMode.First) > -1;
        }

        public bool TryGetItem(int index, out T item)
        {
            bool b = index > 0 && index < _inner.Count;

            if (b)
                item = _inner[index];
            else
                item = default(T);

            return b;
        }

        #endregion

        #region ICollection<T> Members

        public int Count
        {
            get { return _inner.Count; }
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            _inner.CopyTo(array, arrayIndex);
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

        public SortedList<T> Clone()
        {
            return this.OnClone();
        }

        protected virtual SortedList<T> OnClone()
        {
            return new SortedList<T>(this, _comparer);
        }

        object ICloneable.Clone()
        {
            return this.OnClone();
        }

        #endregion

        #region System.Collections.Generic.ICollection<T> Members

        void System.Collections.Generic.ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        bool System.Collections.Generic.ICollection<T>.Remove(T item)
        {
            return ((System.Collections.Generic.ICollection<T>)_inner).Remove(item);
        }

        bool System.Collections.Generic.ICollection<T>.Contains(T item)
        {
            return this.Contains(item);
        }

        bool System.Collections.Generic.ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex, 0, null);
        }

        #endregion

        #region IComparerUser<T> Members

        public IComparer<T> Comparer
        {
            get { return _comparer; }
        }

        #endregion

        #region IList Members

        int IList.Add(object item)
        {
            if (CollectionsHelper.IsCompatible<T>(item))
                return this.Add((T)item);

            return -1;
        }

        void IList.Add(IEnumerable items)
        {
            this.Add((IEnumerable<T>)items);
        }

        void IList.Remove(int index, int count)
        {
            this.Remove(index, count);
        }

        void IList.Remove(object item, SelectionMode mode)
        {
            this.Remove((T)item, mode);
        }

        void IList.Clear()
        {
            this.Clear();
        }

        #endregion

        #region IListView Members

        IEnumerable IListView.this[int index, int count]
        {
            get { return this[index, count]; }
        }

        void IListView.CopyTo(Array array, int arrayIndex, int listIndex, int? count)
        {
            if (array is T[])
            {
                this.CopyTo((T[])array, arrayIndex, listIndex, count);
                return;
            }

            if (count.HasValue && count.Value < 1)
                throw new ArgumentOutOfRangeException("count");

            int innerCount = _inner.Count;
            int c = count ?? innerCount - listIndex;

            if (arrayIndex < 0 || arrayIndex + c > innerCount)
                throw new ArgumentOutOfRangeException("arrayIndex");

            if (listIndex < 0 || listIndex + c > innerCount)
                throw new ArgumentOutOfRangeException("listIndex");

            for (int i = 0; i < c; ++i)
                array.SetValue(_inner[listIndex++], arrayIndex++);
        }

        int IListView.IndexOf(object item, SelectionMode mode)
        {
            if (CollectionsHelper.IsCompatible<T>(item))
                return this.IndexOf((T)item, mode);

            return -1;
        }

        bool IListView.Contains(object item, out int index, SelectionMode mode)
        {
            if (CollectionsHelper.IsCompatible<T>(item))
                return this.Contains((T)item, out index, mode);

            index = -1;
            return false;
        }

        bool IListView.TryGetItem(int index, out object item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            ((System.Collections.ICollection)this).CopyTo(array, arrayIndex);
        }

        bool ICollection.Contains(object item)
        {
            return ((IListView)this).Contains(item);
        }

        #endregion

        #region ICollection Members

        void System.Collections.ICollection.CopyTo(Array array, int index)
        {
            ((System.Collections.ICollection)_inner).CopyTo(array, index);
        }

        bool System.Collections.ICollection.IsSynchronized
        {
            get { return ((System.Collections.ICollection)_inner).IsSynchronized; }
        }

        object System.Collections.ICollection.SyncRoot
        {
            get { return ((System.Collections.ICollection)_inner).SyncRoot; }
        }

        #endregion
    }
}
