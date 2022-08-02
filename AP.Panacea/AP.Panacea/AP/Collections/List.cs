using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AP.Collections
{
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false)]
    public class List<T> : IUnsortedList<T>, IEqualityComparerUser<T>, IUnsortedList
    {
        private readonly System.Collections.Generic.List<T> _inner;
        private readonly IEqualityComparer<T> _comparer;
        private readonly bool _hasDefaultComparer;

        public List()
            : this(0, null)
        { }

        public List(IEqualityComparer<T> comparer)
            : this(0, comparer)
        { }

        public List(int capacity)
            : this(capacity, null)
        { }

        public List(int capacity, IEqualityComparer<T> comparer)
            : this(new System.Collections.Generic.List<T>(capacity), comparer ?? EqualityComparer<T>.Default)
        { }

        public List(IEnumerable<T> collection)
            : this(collection, null)
        { }

        public List(IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : this(collection == null ? new System.Collections.Generic.List<T>() : new System.Collections.Generic.List<T>(collection), comparer ?? EqualityComparer<T>.Default)
        { }

        protected List(System.Collections.Generic.List<T> inner, IEqualityComparer<T> comparer)
        {
            if (inner == null)
                throw new ArgumentNullException("inner");

            if (comparer == null)
                throw new ArgumentNullException("comparer");

            _inner = inner;
            _comparer = comparer;
            _hasDefaultComparer = comparer == EqualityComparer<T>.Default;
        }

        public static List<T> Wrap(System.Collections.Generic.List<T> list, IEqualityComparer<T> comparer = null)
        {
            return new List<T>(list, comparer ?? EqualityComparer<T>.Default);
        }

        public static implicit operator AP.Collections.List<T>(System.Collections.Generic.List<T> list)
        {
            return Wrap(list);
        }

        public static implicit operator System.Collections.Generic.List<T>(AP.Collections.List<T> list)
        {
            return list._inner;
        }

        public override string ToString()
        {
            return _inner.ToString();
        }

        #region IList<T> Members

        public void Remove(T item, SelectionMode mode = SelectionMode.First)
        {
            switch (mode)
            {
                case SelectionMode.First:
                    if (_hasDefaultComparer)
                    {
                        _inner.Remove(item);
                        break;
                    }
                    else
                    {
                        for (int i = 0, c = _inner.Count; i < c; ++i)
                        {
                            if (_comparer.Equals(_inner[i], item))
                            {
                                _inner.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    break;
                case SelectionMode.Last:
                    for (int i = _inner.Count; --i > -1; )
                    {
                        if (_comparer.Equals(_inner[i], item))
                        {
                            _inner.RemoveAt(i);
                            break;
                        }
                    }
                    break;
                case SelectionMode.All:
                    for (int i = 0, c = _inner.Count; i < c; ++i)
                    {
                        if (_comparer.Equals(_inner[i], item))
                            _inner.RemoveAt(i--);
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

        public void Move(int index, int newIndex, int count = 1)
        {
            int listCount = _inner.Count;

            if (count < 1)
                throw new ArgumentOutOfRangeException("count");

            if (index == newIndex)
                throw new ArgumentOutOfRangeException("index == newIndex");

            int maxIndex = _inner.Count - count;

            if (index < 0 || index > maxIndex)
                throw new ArgumentOutOfRangeException("index");

            if (newIndex < 0 || newIndex > maxIndex)
                throw new ArgumentOutOfRangeException("newIndex");

            // special case
            if (count == 1)
            {
                T item = _inner[index];
                _inner.Insert(newIndex, item);

                if (index > newIndex)
                    index--;

                _inner.RemoveAt(index);

                return;
            }
            else
            {
                T[] array = new T[count];

                for (int i = 0, j = index; i < count; ++i)
                    array[i] = _inner[j++];

                _inner.InsertRange(newIndex, array);

                // re-calculate the index - if necessary
                if (index > newIndex)
                    index += count;

                _inner.RemoveRange(index, count);
            }
        }

        public int Add(T item)
        {
            _inner.Add(item);
            return _inner.Count - 1;
        }

        public void Add(IEnumerable<T> items)
        {
            _inner.AddRange(items);
        }

        public void Insert(int index, T item)
        {
            _inner.Insert(index, item);
        }

        public void Insert(int index, IEnumerable<T> items)
        {
            _inner.InsertRange(index, items);
        }

        public void Replace(int index, IEnumerable<T> items)
        {
            if (index < 0 || index >= _inner.Count)
                throw new IndexOutOfRangeException("index");

            foreach (T item in items)
            {
                if (index < _inner.Count)
                    _inner[index] = item;
                else
                    _inner.Add(item);

                ++index;
            }
        }

        public T this[int index]
        {
            get
            {
                return _inner[index];
            }
            set
            {
                _inner[index] = value;
            }
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


        public void Clear()
        {
            _inner.Clear();
        }

        #endregion

        #region IListView<T> Members

        public int IndexOf(T item, SelectionMode mode = SelectionMode.First)
        {
            switch (mode)
            {
                case SelectionMode.First:
                    if (_hasDefaultComparer)
                        return _inner.IndexOf(item);
                    else
                    {
                        for (int i = 0, c = _inner.Count; i < c; ++i)
                        {
                            if (_comparer.Equals(_inner[i], item))
                                return i;
                        }
                    }
                    break;
                case SelectionMode.Last:
                    if (_hasDefaultComparer)
                        return _inner.LastIndexOf(item);
                    else
                    {
                        for (int i = _inner.Count; --i > -1; )
                        {
                            if (_comparer.Equals(_inner[i], item))
                                return i;
                        }
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

        public void CopyTo(T[] array, int arrayIndex = 0, int listIndex = 0, int? count = null)
        {
            if (count.HasValue)
                _inner.CopyTo(listIndex, array, arrayIndex, count.Value);
            else if (listIndex > 0)
                _inner.CopyTo(listIndex, array, arrayIndex, _inner.Count - listIndex);
            else
                _inner.CopyTo(array, arrayIndex);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex, 0, null);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        #endregion

        #region ICloneable Members

        public List<T> Clone()
        {
            return this.OnClone();
        }

        protected virtual List<T> OnClone()
        {
            return new List<T>(this, _comparer);
        }

        object ICloneable.Clone()
        {
            return this.OnClone();
        }

        #endregion

        #region System.Collections.Generic.IList<T> Members

        int System.Collections.Generic.IList<T>.IndexOf(T item)
        {
            return _inner.IndexOf(item);
        }

        void System.Collections.Generic.IList<T>.RemoveAt(int index)
        {
            _inner.RemoveAt(index);
        }

        #endregion

        #region System.Collections.Generic.ICollection<T> Members

        void System.Collections.Generic.ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        bool System.Collections.Generic.ICollection<T>.Remove(T item)
        {
            int index = this.IndexOf(item);

            if (index > -1)
            {
                this.Remove(index, 1);
                return true;
            }

            return false;
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

        #region IEqualityComparerUser<T> Members

        public IEqualityComparer<T> Comparer
        {
            get { return _comparer; }
        }

        #endregion

        #region IUnsortedList Members

        void IUnsortedList.Insert(int index, object item)
        {
            this.Insert(index, (T)item);
        }

        void IUnsortedList.Insert(int index, IEnumerable items)
        {
            this.Insert(index, (IEnumerable<T>)items);
        }

        void IUnsortedList.Move(int index, int newIndex, int count)
        {
            this.Move(index, newIndex, count);
        }

        void IUnsortedList.Replace(int index, IEnumerable items)
        {
            this.Replace(index, (IEnumerable<T>)items);
        }

        object IUnsortedList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (T)value;
            }
        }

        void IUnsortedList.Clear()
        {
            this.Clear();
        }

        #endregion

        #region IList Members

        int IList.Add(object item)
        {
            return this.Add((T)item);
        }

        void IList.Add(System.Collections.IEnumerable items)
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

        System.Collections.IEnumerable IListView.this[int index, int count]
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
            T tmp;
            if (this.TryGetItem(index, out tmp))
            {
                item = tmp;
                return true;
            }

            item = null;
            return false;
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            ((System.Collections.ICollection)_inner).CopyTo(array, arrayIndex);
        }

        bool ICollection.Contains(object item)
        {
            return ((IListView)this).Contains(item);
        }

        #endregion

        #region ICollection Members

        void System.Collections.ICollection.CopyTo(Array array, int index)
        {
            ((IListView)this).CopyTo(array, index, 0, null);
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

        #region IList Members

        int System.Collections.IList.Add(object value)
        {
            return ((System.Collections.IList)_inner).Add(value);
        }

        void System.Collections.IList.Clear()
        {
            this.Clear();
        }

        bool System.Collections.IList.Contains(object value)
        {
            return ((IListView)_inner).Contains(value);
        }

        int System.Collections.IList.IndexOf(object value)
        {
            return this.IndexOf((T)value, SelectionMode.First);
        }

        void System.Collections.IList.Insert(int index, object value)
        {
            this.Insert(index, (T)value);
        }

        bool System.Collections.IList.IsFixedSize
        {
            get { return false; }
        }

        bool System.Collections.IList.IsReadOnly
        {
            get { return false; }
        }

        void System.Collections.IList.Remove(object value)
        {
            this.Remove((T)value);
        }

        void System.Collections.IList.RemoveAt(int index)
        {
            this.Remove(index, 1);
        }

        object System.Collections.IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (T)value;
            }
        }

        #endregion
    }
}
