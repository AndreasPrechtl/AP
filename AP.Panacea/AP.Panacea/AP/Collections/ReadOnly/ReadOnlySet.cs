using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace AP.Collections.ReadOnly
{
    [Serializable, System.ComponentModel.ReadOnly(true)]
    public class ReadOnlySet<T> : ISetView<T>, IEqualityComparerUser<T>, ISetView, System.Collections.Generic.ISet<T>
    {
        private readonly AP.Collections.Set<T> _inner;
        private static volatile ReadOnlySet<T> _empty;

        public static ReadOnlySet<T> Empty
        {
            get
            {
                ReadOnlySet<T> empty = _empty;

                if (empty == null)
                    _empty = empty = new ReadOnlySet<T>(new HashSet<T>());

                return empty;
            }
        }

        private static AP.Collections.Set<T> CreateInner(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            ExceptionHelper.AssertNotNull(() => collection);

            return new Set<T>(collection, comparer);
        }

        public ReadOnlySet(IEnumerable<T> collection)
            : this(collection, null)
        { }

        public ReadOnlySet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : this(CreateInner(collection, comparer))
        { }

        protected ReadOnlySet(AP.Collections.Set<T> inner)
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

        public ReadOnlySet<T> Clone()
        {
            return this;
        }
        
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #region IEqualityComparerUser<T> Members

        public IEqualityComparer<T> Comparer
        {
            get { return _inner.Comparer; }
        }

        #endregion

        #region ISetView Members

        bool ISetView.IsSubsetOf(IEnumerable other)
        {
            return ((ISetView)_inner).IsSubsetOf(other);
        }

        bool ISetView.IsSupersetOf(IEnumerable other)
        {
            return ((ISetView)_inner).IsSupersetOf(other);
        }

        bool ISetView.IsProperSupersetOf(IEnumerable other)
        {
            return ((ISetView)_inner).IsProperSupersetOf(other);
        }

        bool ISetView.IsProperSubsetOf(IEnumerable other)
        {
            return ((ISetView)_inner).IsProperSubsetOf(other);
        }

        bool ISetView.Overlaps(IEnumerable other)
        {
            return ((ISetView)_inner).Overlaps(other);
        }

        bool ISetView.SetEquals(IEnumerable other)
        {
            return ((ISetView)_inner).SetEquals(other);
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
