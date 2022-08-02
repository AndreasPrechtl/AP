using System;
using System.Collections.Generic;

namespace AP.Collections.ObjectModel
{
    [Serializable]
    public abstract class ExtendableSet<T> : SetBase<T>, IEqualityComparerUser<T>
    {
        private readonly Set<T> _inner;
        
        protected Set<T> Inner
        {
            get { return _inner; }
        }

        protected ExtendableSet()
            : this(null, null)
        { }

        protected ExtendableSet(IEnumerable<T> collection)
            : this(collection, null)
        { }

        protected ExtendableSet(IEqualityComparer<T> comparer)
            : this(null, comparer)
        { }

        protected ExtendableSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : this(new Set<T>(collection, comparer))
        { }

        protected ExtendableSet(Set<T> inner)
        {
            if (inner == null)
                throw new ArgumentNullException("inner");

            _inner = inner;
        }

        public new ExtendableSet<T> Clone()
        {
            return (ExtendableSet<T>)this.OnClone();
        }

        public override string ToString()
        {
            return _inner.ToString();
        }
        
        #region ISet<T> Members

        public override bool Add(T item)
        {
            return _inner.Add(item);
        }

        public override void ExceptWith(IEnumerable<T> other)
        {
            _inner.ExceptWith(other);
        }

        public override void IntersectWith(IEnumerable<T> other)
        {
            _inner.IntersectWith(other);
        }

        public override bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return _inner.IsProperSubsetOf(other);
        }

        public override bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return _inner.IsProperSupersetOf(other);
        }

        public override bool IsSubsetOf(IEnumerable<T> other)
        {
            return _inner.IsSubsetOf(other);
        }

        public override bool IsSupersetOf(IEnumerable<T> other)
        {
            return _inner.IsSupersetOf(other);
        }

        public override bool Overlaps(IEnumerable<T> other)
        {
            return _inner.Overlaps(other);
        }

        public override bool SetEquals(IEnumerable<T> other)
        {
            return _inner.SetEquals(other);
        }

        public override void SymmetricExceptWith(IEnumerable<T> other)
        {
            _inner.SymmetricExceptWith(other);
        }

        public override void UnionWith(IEnumerable<T> other)
        {
            _inner.UnionWith(other);
        }

        public override void Clear()
        {
            _inner.Clear();
        }

        #endregion

        #region ICollection<T> Members

        public override bool Contains(T item)
        {
            return _inner.Contains(item);
        }

        public override void CopyTo(T[] array, int arrayIndex)
        {
            _inner.CopyTo(array, arrayIndex);
        }

        public override int Count
        {
            get { return _inner.Count; }
        }

        public override bool Remove(T item)
        {
            return _inner.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public override IEnumerator<T> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        #endregion

        #region IEqualityComparerUser<T> Members

        public IEqualityComparer<T> Comparer
        {
            get { return _inner.Comparer; }
        }

        #endregion        
    }
}
