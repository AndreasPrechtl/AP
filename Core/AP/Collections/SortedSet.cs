using AP.ComponentModel;
using System;
using System.Collections.Generic;

namespace AP.Collections
{
    [Serializable, Sorted]
    public class SortedSet<T> : ISet<T>, IComparerUser<T>
    {
        private readonly object SyncRoot = new object();
        private readonly System.Collections.Generic.SortedSet<T> _inner;
        private readonly IComparer<T> _comparer;

        private static System.Collections.Generic.SortedSet<T> CreateInnerSet(IEnumerable<T> collection, IComparer<T> comparer)
        {            
            if (collection != null)
            {
                if (comparer != null)
                    return new System.Collections.Generic.SortedSet<T>(collection, comparer);

                return new System.Collections.Generic.SortedSet<T>(collection);
            }

            if (comparer != null)
                return new System.Collections.Generic.SortedSet<T>(comparer);

            return new System.Collections.Generic.SortedSet<T>();
        }

        public SortedSet()
            : this(CreateInnerSet(null, null))
        { }

        public SortedSet(IEnumerable<T> collection)
            : this(CreateInnerSet(collection, null))
        { }

        public SortedSet(IComparer<T> comparer)
            : this(CreateInnerSet(null, comparer))
        { }
        
        public SortedSet(IEnumerable<T> collection, IComparer<T> comparer)
            : this(CreateInnerSet(collection, comparer))
        { }

        protected SortedSet(System.Collections.Generic.SortedSet<T> inner)
        {
            _comparer = inner.Comparer;
            _inner = inner;            
        }

        public static SortedSet<T> Wrap(System.Collections.Generic.SortedSet<T> set)
        {
            return new SortedSet<T>(set);
        }

        public static implicit operator SortedSet<T>(System.Collections.Generic.SortedSet<T> sortedSet)
        {
            return Wrap(sortedSet);
        }

        public static implicit operator System.Collections.Generic.SortedSet<T>(SortedSet<T> sortedSet)
        {
            return sortedSet._inner;
        }

        public override string ToString()
        {
            return _inner.ToString();
        }
        
        #region ISet<T> Members

        public bool Add(T item)
        {
            return _inner.Add(item);
        }

        public bool Remove(T item)
        {
            return _inner.Remove(item);
        }

        public void Add(IEnumerable<T> other)
        {
            _inner.UnionWith(other);
        }

        public void UnionWith(IEnumerable<T> other)
        {
            _inner.UnionWith(other);
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            _inner.IntersectWith(other);
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            _inner.ExceptWith(other);
        }

        public void Remove(IEnumerable<T> other)
        {
            _inner.ExceptWith(other);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            _inner.SymmetricExceptWith(other);
        }
        
        #endregion

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

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
        
        #region ICloneable Members

        public SortedSet<T> Clone()
        {
            return this.OnClone();
        }

        protected virtual SortedSet<T> OnClone()
        {
            return new SortedSet<T>(this, _comparer);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #region ICollection<T> Members

        void System.Collections.Generic.ICollection<T>.Add(T item)
        {
            ((System.Collections.Generic.ICollection<T>)_inner).Add(item);
        }

        bool System.Collections.Generic.ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        public void Clear()
        {
            _inner.Clear();
        }

        #endregion

        #region IComparerUser<T> Members

        public IComparer<T> Comparer
        {
            get { return _comparer; }
        }

        #endregion
    }
}
