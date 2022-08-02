using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AP.Collections.ObjectModel
{
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false)]
    public abstract class CollectionBase<T> : ICollection<T>
    {
        private readonly object _syncRoot = new object();

        protected virtual object SyncRoot { get { return _syncRoot; } }
        protected virtual bool IsSyncronized { get { return false; } }

        public override string ToString()
        {
            return CollectionsHelper.ToString(this);
        }

        #region ICollection<T> Members

        public abstract int Count { get; }
        
        public virtual void CopyTo(T[] array, int arrayIndex = 0)
        {
            CollectionsHelper.CopyTo<T>(this, array, arrayIndex);
        }

        public virtual bool Contains(T item)
        {
            foreach (T current in this)
                if (object.Equals(current, item))
                    return true;

            return false;
        }
        
        #endregion

        #region IEnumerable<T> Members

        public abstract IEnumerator<T> GetEnumerator();

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region ICloneable Members

        protected abstract CollectionBase<T> OnClone();

        public CollectionBase<T> Clone()
        {
            return this.OnClone();
        }

        object ICloneable.Clone()
        {
            return OnClone();
        }

        #endregion
    }
}
