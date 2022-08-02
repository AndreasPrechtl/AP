using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP.Linq;

namespace AP.Collections.ObjectModel
{
    public abstract class ListBase<T> : CollectionBase<T>, AP.Collections.IList<T>
    {
        #region IList<T> Members
        
        public abstract int Add(T item);

        public virtual void Add(IEnumerable<T> items)
        {
            foreach (T item in items)
                this.Add(item);
        }

        public virtual void Remove(T item, SelectionMode mode = SelectionMode.First)
        {
            int index = -1;
            
            switch (mode)
            {
                case SelectionMode.First:
                case SelectionMode.Last:                    
                    if (this.Contains(item, out index, mode))
                        this.Remove(index, 1);
                    break;
                case SelectionMode.All:
                    while (this.Contains(item, out index, SelectionMode.First))
                        this.Remove(index, 1);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("mode");
            }            
        }

        public abstract void Remove(int index, int count = 1);

        public virtual IEnumerable<T> this[int index, int count = 1]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                    throw new ArgumentOutOfRangeException("index");

                if (index + count > this.Count)
                    throw new ArgumentOutOfRangeException("count");

                for (int i = 0; i < count; ++i)
                    yield return this[index++];
            }
        }
        
        public virtual void Clear()
        {
            this.Remove(0, this.Count);
        }

        #endregion

        #region IListView<T> Members
        
        public virtual void CopyTo(T[] array, int arrayIndex = 0, int listIndex = 0, int? count = null)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (array.Length == 0)
                throw new ArgumentOutOfRangeException("array.Length == 0");

            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new ArgumentOutOfRangeException("arrayIndex");
            
            int listCount = this.Count;
            if (listIndex < 0 || listIndex >= listCount)
                throw new ArgumentOutOfRangeException("listIndex");

            int c = count ?? listCount;
            if (c < 0 || c + listIndex > listCount)
                throw new ArgumentOutOfRangeException("count");
            
            if (arrayIndex + count > array.Length)
                throw new ArgumentOutOfRangeException("count");

            for (int i = 0; i < c; ++i)
                array[arrayIndex++] = this.GetItem(listIndex++);            
        }

        public int IndexOf(T item, SelectionMode mode = SelectionMode.First)
        {
            switch (mode)
            {
                case SelectionMode.First:
                    {
                        int i = 0;                    
                        foreach (T current in this)
                        {
                            if (current.Equals(item))
                                return i;

                            ++i;
                        }
                        return -1;
                    }
                case SelectionMode.Last:

                    for (int i = this.Count; --i > -1; )
                    {
                        if (this[i].Equals(item))
                            return i;
                    }
                    return -1;

                default:                    
                    throw new ArgumentOutOfRangeException("mode");
            }
        }

        public bool Contains(T item, out int index, SelectionMode mode = SelectionMode.First)
        {
            return (index = this.IndexOf(item, mode)) > -1;
        }

        protected abstract T GetItem(int index);
      
        public bool TryGetItem(int index, out T item)
        {
            bool b = index > 0 && index < this.Count;

            if (b)
                item = this.GetItem(index);
            else
                item = default(T);

            return b;
        }

        #endregion

        #region ICollection<T> Members
        
        public virtual bool Contains(T item, SelectionMode mode = SelectionMode.First)
        {
            return this.IndexOf(item, mode) > -1;
        }

        #endregion

        #region IReadOnlyList<T> Members

        public T this[int index]
        {
            get { return this.GetItem(index); }
        }        

        #endregion
              
        #region ICloneable Members

        public new ListBase<T> Clone()
        {
            return (ListBase<T>)this.OnClone();
        }

        #endregion

        #region System.Collections.Generic.ICollection<T> Members

        void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex, 0, null);
        }

        void System.Collections.Generic.ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        bool System.Collections.Generic.ICollection<T>.Remove(T item)
        {
            int index = -1;

            if (this.Contains(item, out index, SelectionMode.First))
            {
                this.Remove(index);
                return true;
            }

            return false;
        }
        
        protected virtual bool IsReadOnly
        {
            get { return false; }
        }

        bool System.Collections.Generic.ICollection<T>.IsReadOnly 
        { 
            get { return this.IsReadOnly; } 
        }
        
        bool System.Collections.Generic.ICollection<T>.Contains(T item)
        {
            return this.Contains(item, SelectionMode.First);
        }

        #endregion          
    }
}