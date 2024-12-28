using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AP.Collections.ObjectModel
{
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false)]
    public abstract class CollectionBase<T> : ICollection<T>, ICollection
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
            get { return this.IsSyncronized; }
        }

        object System.Collections.ICollection.SyncRoot
        {
            get { return this.SyncRoot; }
        }

        #endregion
    }
}
