using System;
using System.Collections.Generic;

namespace AP.Collections
{
    [Serializable]
    public class Set<T> : ISet<T>, IEqualityComparerUser<T>
    {
        private readonly object SyncRoot = new object();
        private readonly System.Collections.Generic.HashSet<T> _inner;
        private readonly IEqualityComparer<T> _comparer;

        private static System.Collections.Generic.HashSet<T> CreateInnerSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            if (collection != null)
            {
                if (comparer != null)
                    return new System.Collections.Generic.HashSet<T>(collection, comparer);

                return new System.Collections.Generic.HashSet<T>(collection);
            }

            if (comparer != null)
                return new System.Collections.Generic.HashSet<T>(comparer);

            return new System.Collections.Generic.HashSet<T>();
        }

        public Set()
            : this(CreateInnerSet(null, null))
        { }

        public Set(IEnumerable<T> collection)
            : this(CreateInnerSet(collection, null))
        { }

        public Set(IEqualityComparer<T> comparer)
            : this(CreateInnerSet(null, comparer))
        { }

        public Set(IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : this(CreateInnerSet(collection, comparer))
        { }

        protected Set(System.Collections.Generic.HashSet<T> inner)
        {
            if (inner == null)
                throw new ArgumentNullException("inner");

            _comparer = inner.Comparer;
            _inner = inner;
        }

        public static Set<T> Wrap(HashSet<T> set)
        {
            return new Set<T>(set);
        }

        public static implicit operator Set<T>(System.Collections.Generic.HashSet<T> set)
        {
            return Wrap(set);
        }

        public static implicit operator System.Collections.Generic.HashSet<T>(Set<T> set)
        {
            return set._inner;
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

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            _inner.SymmetricExceptWith(other);
        }

        public void Remove(IEnumerable<T> other)
        {
            _inner.ExceptWith(other);
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

        public Set<T> Clone()
        {
            return this.OnClone();
        }

        protected virtual Set<T> OnClone()
        {
            return new Set<T>(this, _comparer);
        }

        object ICloneable.Clone()
        {
            return this.OnClone();
        }

        #endregion

        #region System.Collections.Generic.ICollection<T> Members

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

        #region IEqualityComparerUser<T> Members

        public IEqualityComparer<T> Comparer
        {
            get { return _comparer; }
        }

        #endregion        
    }
}
