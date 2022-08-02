using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using AP.ComponentModel;

namespace AP.Collections.ReadOnly
{
    [Serializable, System.ComponentModel.ReadOnly(true), Sorted(true)]
    public class ReadOnlySortedSet<T> : ISetView<T>, IComparerUser<T>, System.Collections.Generic.ISet<T>
    {
        private readonly AP.Collections.SortedSet<T> _inner;
        private static volatile ReadOnlySortedSet<T> _empty;

        public static ReadOnlySortedSet<T> Empty
        {
            get
            {
                ReadOnlySortedSet<T> empty = _empty;

                if (empty == null)
                    _empty = empty = new ReadOnlySortedSet<T>(new HashSet<T>());

                return empty;
            }
        }

        private static AP.Collections.SortedSet<T> CreateInner(IEnumerable<T> collection, IComparer<T> comparer)
        {
            ExceptionHelper.AssertNotNull(() => collection);

            return new SortedSet<T>(collection, comparer);
        }

        public ReadOnlySortedSet(IEnumerable<T> collection)
            : this(collection, null)
        { }

        public ReadOnlySortedSet(IEnumerable<T> collection, IComparer<T> comparer)
            : this(CreateInner(collection, comparer))
        { }

        protected ReadOnlySortedSet(AP.Collections.SortedSet<T> inner)
        {
            if (inner == null)
                throw new ArgumentNullException("inner");

            _inner = inner;
        }

        public override string ToString()
        {
            return _inner.ToString();
        }

        #region ISetView<T> Members

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return _inner.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return _inner.IsSupersetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return _inner.IsProperSupersetOf(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return _inner.IsProperSubsetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return _inner.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return _inner.SetEquals(other);
        }

        #endregion

        #region ICollection<T> Members

        public int Count
        {
            get { return _inner.Count; }
        }

        public void CopyTo(T[] array, int arrayIndex = 0)
        {
            _inner.CopyTo(array, arrayIndex);
        }

        public bool Contains(T item)
        {
            return _inner.Contains(item);
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

        public virtual ReadOnlySortedSet<T> Clone()
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

        #region ISet<T> Members

        bool System.Collections.Generic.ISet<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void System.Collections.Generic.ISet<T>.ExceptWith(IEnumerable<T> other)
        {
            throw new NotSupportedException();
        }

        void System.Collections.Generic.ISet<T>.IntersectWith(IEnumerable<T> other)
        {
            throw new NotSupportedException();
        }

        void System.Collections.Generic.ISet<T>.SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotSupportedException();
        }

        void System.Collections.Generic.ISet<T>.UnionWith(IEnumerable<T> other)
        {
            throw new NotSupportedException();
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

        bool System.Collections.Generic.ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
