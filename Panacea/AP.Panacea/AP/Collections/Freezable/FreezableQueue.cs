using AP.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCG = System.Collections.Generic;

namespace AP.Collections.Freezable
{
    [Serializable]
    public class FreezableQueue<T> : ExtendableQueue<T>, IFreezable
    {
        public FreezableQueue()
            : base()
        {
            _isFrozen = false;
        }

        public FreezableQueue(IEqualityComparer<T> comparer)
            : base(comparer)
        {
            _isFrozen = false;
        }

        public FreezableQueue(int capacity)
            : base(capacity)
        {
            _isFrozen = false;
        }
        
        public FreezableQueue(int capacity, IEqualityComparer<T> comparer)
            : base(capacity, comparer)
        {
            _isFrozen = false;
        }

        public FreezableQueue(IEnumerable<T> collection, bool isFrozen = false)
            : base(collection)
        {
            _isFrozen = isFrozen;
        }

        public FreezableQueue(IEnumerable<T> collection, IEqualityComparer<T> comparer, bool isFrozen = false)
            : base(collection, comparer)
        {
            _isFrozen = isFrozen;
        }
        
        protected FreezableQueue(Queue<T> inner, bool isFrozen = false)
            : base(inner)
        {
            _isFrozen = isFrozen;
        }

        public new FreezableQueue<T> Clone()
        {
            return (FreezableQueue<T>)this.OnClone();
        }

        protected override CollectionBase<T> OnClone()
        {
            return new FreezableQueue<T>(this, this.Comparer, this.IsFrozen);
        }

        public override void Clear()
        {
            AssertCanWrite();
            base.Clear();
        }
        public override T Dequeue()
        {
            AssertCanWrite();
            return base.Dequeue();
        }
        public override void Enqueue(IEnumerable<T> items)
        {
            AssertCanWrite();
            base.Enqueue(items);
        }
        public override void Enqueue(T item)
        {
            AssertCanWrite();
            base.Enqueue(item);
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
    }
}
