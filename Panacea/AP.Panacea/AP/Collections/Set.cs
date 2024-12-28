using System;
using System.Collections.Generic;

namespace AP.Collections
{
    [Serializable]
    public class Set<T> : ISet<T>, ISet, IEqualityComparerUser<T>
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
        
        #region ICollection Members

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            int c = this.Count;
            if (arrayIndex < 0 || arrayIndex + array.Length > c)
                throw new ArgumentOutOfRangeException("arrayIndex");

            int i = 0;
            foreach (T item in this)
                array.SetValue(item, i++);
        }

        bool ICollection.Contains(object item)
        {
            if (CollectionsHelper.IsCompatible<T>(item))
                return this.Contains((T)item);

            return false;
        }

        #endregion

        #region ICollection Members

        void System.Collections.ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)this).CopyTo(array, index);
        }

        int System.Collections.ICollection.Count
        {
            get { return this.Count; }
        }

        bool System.Collections.ICollection.IsSynchronized
        {
            get { return false; }
        }

        object System.Collections.ICollection.SyncRoot
        {
            get { return this.SyncRoot; }
        }

        #endregion

        #region ISet Members

        void ISet.Add(System.Collections.IEnumerable items)
        {
            this.Add((IEnumerable<T>)items);
        }

        void ISet.Remove(System.Collections.IEnumerable items)
        {
            this.Remove((IEnumerable<T>)items);
        }

        bool ISet.Add(object item)
        {
            return CollectionsHelper.IsCompatible<T>(item) && this.Add((T)item);
        }

        bool ISet.Remove(object item)
        {
            return CollectionsHelper.IsCompatible<T>(item) && this.Remove((T)item);
        }

        void ISet.ExceptWith(System.Collections.IEnumerable other)
        {
            this.ExceptWith((IEnumerable<T>)other);
        }

        void ISet.IntersectWith(System.Collections.IEnumerable other)
        {
            this.IntersectWith((IEnumerable<T>)other);
        }

        void ISet.SymmetricExceptWith(System.Collections.IEnumerable other)
        {
            this.SymmetricExceptWith((IEnumerable<T>)other);
        }

        void ISet.UnionWith(System.Collections.IEnumerable other)
        {
            this.UnionWith((IEnumerable<T>)other);
        }

        #endregion
      
        #region ISetView Members

        bool ISetView.IsSubsetOf(System.Collections.IEnumerable other)
        {
            return other is IEnumerable<T> && this.IsSubsetOf((IEnumerable<T>)other);                
        }

        bool ISetView.IsSupersetOf(System.Collections.IEnumerable other)
        {
            return other is IEnumerable<T> && this.IsSupersetOf((IEnumerable<T>)other);
        }

        bool ISetView.IsProperSupersetOf(System.Collections.IEnumerable other)
        {
            return other is IEnumerable<T> && this.IsProperSupersetOf((IEnumerable<T>)other);
        }

        bool ISetView.IsProperSubsetOf(System.Collections.IEnumerable other)
        {
            return other is IEnumerable<T> && this.IsProperSubsetOf((IEnumerable<T>)other);
        }

        bool ISetView.Overlaps(System.Collections.IEnumerable other)
        {
            return other is IEnumerable<T> && this.Overlaps((IEnumerable<T>)other);
        }

        bool ISetView.SetEquals(System.Collections.IEnumerable other)
        {
            return other is IEnumerable<T> && this.SetEquals((IEnumerable<T>)other);
        }

        #endregion
    }
}
