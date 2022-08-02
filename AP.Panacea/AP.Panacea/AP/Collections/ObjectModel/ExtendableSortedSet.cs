using AP.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP.Collections.ObjectModel
{
    [Serializable, Sorted(true)]
    public abstract class ExtendableSortedSet<T> : SetBase<T>, IComparerUser<T>
    {
        private readonly SortedSet<T> _inner;

        protected SortedSet<T> Inner
        {
            get { return _inner; }
        }

        protected ExtendableSortedSet()
            : this(null, null)
        { }

        protected ExtendableSortedSet(IEnumerable<T> collection)
            : this(collection, null)
        { }

        protected ExtendableSortedSet(IComparer<T> comparer)
            : this(null, comparer)
        { }
        
        protected ExtendableSortedSet(IEnumerable<T> collection, IComparer<T> comparer)
            : this(new SortedSet<T>(collection, comparer))
        { }

        protected ExtendableSortedSet(SortedSet<T> inner)
        {
            ExceptionHelper.AssertNotNull(() => inner);
            _inner = inner;
        }

        public override string ToString()
        {
            return _inner.ToString();
        }

        public new ExtendableSortedSet<T> Clone()
        {
            return (ExtendableSortedSet<T>)this.OnClone();
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
        
        #region IComparerUser<T> Members

        public IComparer<T> Comparer
        {
            get { return _inner.Comparer; }
        }

        #endregion
    }
}
