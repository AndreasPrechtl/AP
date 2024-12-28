using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AP.Collections.ObjectModel;
using AP.ComponentModel;

namespace AP.Collections
{
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false), Sorted(true)]
    public class FreezableSortedList<T> : ExtendableSortedList<T>, IFreezable
    {
        public FreezableSortedList()
            : base()
        {
            _isFrozen = false;
        }

        public FreezableSortedList(IComparer<T> comparer)
            : base(comparer)
        {
            _isFrozen = false;
        }

        public FreezableSortedList(int capacity)
            : base(capacity)
        {
            _isFrozen = false;
        }

        public FreezableSortedList(int capacity, IComparer<T> comparer)
            : base(capacity, comparer)
        {
            _isFrozen = false;
        }

        public FreezableSortedList(IEnumerable<T> collection, bool isFrozen = false)
            : base(collection)
        {
            _isFrozen = isFrozen;
        }

        public FreezableSortedList(IEnumerable<T> collection, IComparer<T> comparer, bool isFrozen = false)
            : base(collection, comparer)
        {
            _isFrozen = isFrozen;
        }

        protected FreezableSortedList(AP.Collections.List<T> inner, bool isFrozen = false)
            : base(inner)
        {
            _isFrozen = isFrozen;
        }

        public new FreezableSortedList<T> Clone()
        {
            return (FreezableSortedList<T>)this.OnClone();
        }

        protected override CollectionBase<T> OnClone()
        {
            return new FreezableSortedList<T>(this, this.Comparer);
        }
        
        #region IFreezable Members

        private bool _isFrozen;

        public virtual bool IsFrozen
        {
            get { return _isFrozen; }
            set
            {
                this.AssertCanWrite();
                _isFrozen = value;
            }
        }

        protected virtual void AssertCanWrite()
        {
            FreezableHelper.AssertCanWrite(this);   
        }
        
        #endregion

        public override int Add(T item)
        {
            AssertCanWrite();
            return base.Add(item);
        }

        public override void Clear()
        {
            AssertCanWrite();
            base.Clear();
        }
        
        public override void Remove(int index, int count = 1)
        {
            AssertCanWrite();
            base.Remove(index, count);
        }

        public override void Remove(T item, SelectionMode mode = SelectionMode.First)
        {
            AssertCanWrite();
            base.Remove(item, mode);
        }

        public override void Add(IEnumerable<T> items)
        {
            AssertCanWrite();
            base.Add(items);
        }
    }
}
