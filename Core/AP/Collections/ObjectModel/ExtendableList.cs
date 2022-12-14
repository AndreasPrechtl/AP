using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SC = System.Collections;
using SCG = System.Collections.Generic;

namespace AP.Collections.ObjectModel
{
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false)]
    public abstract class ExtendableList<T> : ListBase<T>, IUnsortedList<T>, IEqualityComparerUser<T>
    {
        private readonly AP.Collections.List<T> _inner;

        public ExtendableList()
            : this(0, null)
        { }

        public ExtendableList(IEqualityComparer<T> comparer)
            : this(0, comparer)
        { }

        public ExtendableList(int capacity)
            : this(capacity, null)
        { }

        public ExtendableList(int capacity, IEqualityComparer<T> comparer)
            : this(new AP.Collections.List<T>(capacity, comparer))
        { }

        public ExtendableList(IEnumerable<T> collection)
            : this(collection, null)
        { }

        public ExtendableList(IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : this(new AP.Collections.List<T>(collection, comparer))
        { }

        protected ExtendableList(AP.Collections.List<T> inner)
        {
            if (inner == null)
                throw new ArgumentNullException("inner");

            _inner = inner;
        }

        public new ExtendableList<T> Clone()
        {
            return (ExtendableList<T>)this.OnClone();
        }

        protected List<T> Inner
        {
            get { return _inner; }
        }

        public override string ToString()
        {
            return _inner.ToString();
        }

        public override int Add(T item)
        {
            return _inner.Add(item);
        }

        public override void Add(IEnumerable<T> items)
        {
            _inner.Add(items);
        }

        public virtual void Insert(int index, T item)
        {
            _inner.Insert(index, item);
        }

        public virtual void Insert(int index, IEnumerable<T> items)
        {
            _inner.Insert(index, items);
        }

        public override void Remove(int index, int count = 1)
        {
            _inner.Remove(index, count);
        }

        public virtual void Replace(int index, IEnumerable<T> items)
        { }

        public override void CopyTo(T[] array, int arrayIndex = 0, int listIndex = 0, int? count = null)
        {
            _inner.CopyTo(array, arrayIndex, listIndex, count);
        }

        public virtual void Move(int index, int newIndex, int count = 1)
        {
            _inner.Move(index, newIndex, count);
        }

        public override void Remove(T item, SelectionMode mode = SelectionMode.First)
        {
            _inner.Remove(item, mode);
        }

        public override void Clear()
        {
            _inner.Clear();
        }

        protected override T GetItem(int index)
        {
            return _inner[index];
        }

        protected virtual void SetItem(int index, T item)
        {
            _inner[index] = item;
        }

        public new T this[int index]
        {
            get
            {
                return this.GetItem(index);
            }
            set
            {
                this.SetItem(index, value);
            }
        }

        public override IEnumerable<T> this[int index, int count = 1]
        {
            get
            {
                return _inner[index, count];
            }
        }

        public override int Count
        {
            get { return _inner.Count; }
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        #region System.Collections.Generic.IList<T> Members

        void System.Collections.Generic.IList<T>.RemoveAt(int index)
        {
            this.Remove(index, 1);
        }

        int System.Collections.Generic.IList<T>.IndexOf(T item)
        {
            return this.IndexOf(item, SelectionMode.First);
        }

        #endregion

        #region IEqualityComparerUser<T> Members

        public IEqualityComparer<T> Comparer
        {
            get { return _inner.Comparer; }
        }

        #endregion
    }
}
